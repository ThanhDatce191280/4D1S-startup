using System;
using System.Collections;
using HuyetNguyetDaiNgan.Combat;
using HuyetNguyetDaiNgan.Inventory;
using HuyetNguyetDaiNgan.Player;
using UnityEngine;

namespace HuyetNguyetDaiNgan.World
{
    /// <summary>
    /// Nút tài nguyên trong môi trường (Cây gỗ, Mỏ đá, Bụi quả).
    /// Triển khai IDamageable để tương thích hoàn toàn với hệ thống chiến đấu.
    /// Có hiệu ứng rung lắc khi trúng đòn, sinh ra vật phẩm khi cạn kiệt và tự động mọc lại (Respawn).
    /// </summary>
    public class ResourceNode : MonoBehaviour, IDamageable
    {
        [Header("Resource Settings")]
        [Tooltip("Cấu hình vật phẩm rơi (ItemDataSO)")]
        [SerializeField] private ItemDataSO dropItemData;

        [Tooltip("Loại tài nguyên")]
        [SerializeField] private ResourceType resourceType = ResourceType.Wood;

        [Tooltip("Số nhát đánh cần thiết để khai thác cạn kiệt")]
        [SerializeField] private int hitsToDeplete = 3;

        [Tooltip("Thời gian tự hồi sinh / mọc lại sau khi cạn kiệt (giây)")]
        [SerializeField] private float respawnTime = 25f;

        [Tooltip("Số lượng vật phẩm rơi ra khi cạn kiệt (tối thiểu, tối đa)")]
        [SerializeField] private Vector2Int dropCountRange = new Vector2Int(2, 3);

        [Header("Prefabs & Visuals")]
        [Tooltip("Prefab vật phẩm rơi (nếu để trống sẽ tự sinh primitive tương ứng)")]
        [SerializeField] private GameObject dropPrefab;

        [Tooltip("Transform chứa Model nguyên vẹn")]
        [SerializeField] private Transform intactVisual;

        [Tooltip("Transform chứa Model gốc cây/mỏ cạn (tùy chọn)")]
        [SerializeField] private Transform depletedVisual;

        [Tooltip("Collider nhận đòn của nút tài nguyên")]
        [SerializeField] private Collider nodeCollider;

        // IDamageable State
        public float CurrentHealth { get; private set; }
        public float MaxHealth => hitsToDeplete;
        public bool IsDead { get; private set; }

        public ResourceType Type => resourceType;
        public float RespawnProgress { get; private set; } // 0 đến 1 cho UI hoặc Shader

        // Events
        public event Action<ResourceNode> OnNodeHarvested;
        public event Action<ResourceNode> OnNodeDepleted;
        public event Action<ResourceNode> OnNodeRespawned;

        private Coroutine _wobbleCoroutine;
        private Coroutine _respawnCoroutine;
        private Quaternion _originalRotation;

        private void Awake()
        {
            CurrentHealth = hitsToDeplete;
            IsDead = false;

            if (nodeCollider == null)
            {
                nodeCollider = GetComponent<Collider>();
            }

            if (intactVisual == null)
            {
                Transform foundVisual = transform.Find("Visual");
                intactVisual = foundVisual != null ? foundVisual : transform;
            }

            _originalRotation = intactVisual.localRotation;

            if (dropItemData == null)
            {
                dropItemData = ItemDatabase.GetItemByResourceType(resourceType);
            }

            if (depletedVisual != null)
            {
                depletedVisual.gameObject.SetActive(false);
            }
        }

        public void TakeDamage(DamageInfo info)
        {
            if (IsDead) return;

            // Mỗi nhát chém hợp lệ trừ 1 độ bền
            CurrentHealth = Mathf.Max(0f, CurrentHealth - 1f);

            // Hiệu ứng rung lắc khi bị chặt/đập
            TriggerWobble();

            OnNodeHarvested?.Invoke(this);

            if (CurrentHealth <= 0f)
            {
                DepleteNode();
            }
        }

        public void Heal(float amount)
        {
            if (IsDead) return;
            CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
        }

        public void ForceHarvest()
        {
            if (IsDead) return;
            TakeDamage(new DamageInfo(1f, DamageType.Physical));
        }

        private void TriggerWobble()
        {
            if (intactVisual == null) return;

            if (_wobbleCoroutine != null)
            {
                StopCoroutine(_wobbleCoroutine);
                intactVisual.localRotation = _originalRotation;
            }

            _wobbleCoroutine = StartCoroutine(WobbleRoutine());
        }

        private IEnumerator WobbleRoutine()
        {
            float duration = 0.16f;
            float elapsed = 0f;
            float wobbleAngle = 7f;

            // Lắc sang 2 bên
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / duration;
                float angle = Mathf.Sin(progress * Mathf.PI * 4f) * wobbleAngle * (1f - progress);
                intactVisual.localRotation = _originalRotation * Quaternion.Euler(angle, 0f, -angle * 0.5f);
                yield return null;
            }

            intactVisual.localRotation = _originalRotation;
            _wobbleCoroutine = null;
        }

        private void DepleteNode()
        {
            IsDead = true;

            // Ẩn model nguyên vẹn, hiện gốc cây/đá vỡ nếu có
            if (intactVisual != null)
            {
                intactVisual.gameObject.SetActive(false);
            }

            if (depletedVisual != null)
            {
                depletedVisual.gameObject.SetActive(true);
            }

            if (nodeCollider != null)
            {
                nodeCollider.enabled = false;
            }

            // Sinh các vật phẩm rơi ra đất
            SpawnDrops();

            OnNodeDepleted?.Invoke(this);

            // Bắt đầu đếm ngược hồi sinh
            if (respawnTime > 0f)
            {
                _respawnCoroutine = StartCoroutine(RespawnRoutine());
            }
        }

        private void SpawnDrops()
        {
            int count = UnityEngine.Random.Range(dropCountRange.x, dropCountRange.y + 1);

            for (int i = 0; i < count; i++)
            {
                Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * 1.5f;
                Vector3 spawnPos = transform.position + new Vector3(randomCircle.x, 0.4f, randomCircle.y);

                if (dropPrefab != null)
                {
                    GameObject dropObj = Instantiate(dropPrefab, spawnPos, Quaternion.identity);
                    if (dropObj.TryGetComponent<WorldItemPickup>(out var pickup))
                    {
                        pickup.Initialize(resourceType, 1, spawnPos, dropItemData);
                    }
                }
                else
                {
                    // Fallback tạo procedural pickup nếu không gán prefab
                    CreateProceduralPickup(spawnPos);
                }
            }
        }

        private void CreateProceduralPickup(Vector3 spawnPos)
        {
            GameObject pickupObj;
            Color itemColor;

            switch (resourceType)
            {
                case ResourceType.Wood:
                    pickupObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    pickupObj.transform.localScale = new Vector3(0.2f, 0.35f, 0.2f);
                    pickupObj.transform.rotation = Quaternion.Euler(90f, UnityEngine.Random.Range(0f, 180f), 0f);
                    itemColor = new Color(0.55f, 0.32f, 0.15f); // Nâu gỗ
                    break;
                case ResourceType.Stone:
                    pickupObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    pickupObj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    itemColor = new Color(0.6f, 0.62f, 0.65f); // Xám đá
                    break;
                case ResourceType.Food:
                default:
                    pickupObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    pickupObj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    itemColor = new Color(0.9f, 0.2f, 0.2f); // Đỏ quả mọng
                    break;
            }

            pickupObj.name = $"Pickup_{resourceType}";
            pickupObj.transform.position = spawnPos;

            // Gán material màu URP Lit
            Renderer rend = pickupObj.GetComponent<Renderer>();
            if (rend != null)
            {
                Shader shader = Shader.Find("Universal Render Pipeline/Lit");
                if (shader == null) shader = Shader.Find("Standard");
                Material mat = new Material(shader) { color = itemColor };
                rend.material = mat;
            }

            // Collider kích hoạt trigger để không cản trở di chuyển vật lý
            Collider col = pickupObj.GetComponent<Collider>();
            if (col != null) col.isTrigger = true;

            WorldItemPickup pickupScript = pickupObj.AddComponent<WorldItemPickup>();
            pickupScript.Initialize(resourceType, 1, spawnPos, dropItemData);
        }

        private IEnumerator RespawnRoutine()
        {
            RespawnProgress = 0f;
            float timer = 0f;

            while (timer < respawnTime)
            {
                timer += Time.deltaTime;
                RespawnProgress = timer / respawnTime;
                yield return null;
            }

            Respawn();
        }

        public void Respawn()
        {
            if (_respawnCoroutine != null)
            {
                StopCoroutine(_respawnCoroutine);
                _respawnCoroutine = null;
            }

            CurrentHealth = hitsToDeplete;
            IsDead = false;
            RespawnProgress = 0f;

            if (intactVisual != null)
            {
                intactVisual.gameObject.SetActive(true);
                intactVisual.localRotation = _originalRotation;
            }

            if (depletedVisual != null)
            {
                depletedVisual.gameObject.SetActive(false);
            }

            if (nodeCollider != null)
            {
                nodeCollider.enabled = true;
            }

            OnNodeRespawned?.Invoke(this);
        }
    }
}

