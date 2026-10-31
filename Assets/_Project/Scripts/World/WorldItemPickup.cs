using HuyetNguyetDaiNgan.Inventory;
using HuyetNguyetDaiNgan.Player;
using UnityEngine;

namespace HuyetNguyetDaiNgan.World
{
    /// <summary>
    /// Thực thể vật phẩm rơi trên mặt đất (Gỗ, Đá, Quả).
    /// Tự động xoay bập bồng và bay hút vào người chơi khi đến gần (Magnetic Pickup).
    /// </summary>
    public class WorldItemPickup : MonoBehaviour
    {
        [Header("Item Data")]
        [Tooltip("Cấu hình vật phẩm gắn kèm (ItemDataSO)")]
        [SerializeField] private ItemDataSO itemData;

        [Tooltip("Loại tài nguyên")]
        [SerializeField] private ResourceType resourceType = ResourceType.Wood;

        [Tooltip("Số lượng nhận được khi nhặt")]
        [SerializeField] private int amount = 1;

        [Header("Pickup Settings")]
        [Tooltip("Bán kính hút vào người chơi (mét)")]
        [SerializeField] private float pickupRadius = 2.5f;

        [Tooltip("Tốc độ bay về phía người chơi")]
        [SerializeField] private float flySpeed = 12f;

        [Tooltip("Độ cao bập bồng")]
        [SerializeField] private float bobHeight = 0.15f;

        [Tooltip("Tốc độ bập bồng")]
        [SerializeField] private float bobSpeed = 3.5f;

        [Tooltip("Tốc độ tự xoay quanh trục Y (độ/giây)")]
        [SerializeField] private float rotateSpeed = 90f;

        // State & Buffers
        private Vector3 _spawnPosition;
        private float _timeOffset;
        private PlayerStatsComponent _targetPlayer;
        private static readonly Collider[] OverlapBuffer = new Collider[8];

        public ItemDataSO ItemData => itemData;
        public ResourceType Type => resourceType;
        public int Amount => amount;

        private void Awake()
        {
            _spawnPosition = transform.position;
            _timeOffset = Random.Range(0f, 6.28f); // Random phase để các item không nhấp nhô cùng lúc
        }

        public void Initialize(ResourceType type, int itemAmount, Vector3 initialPosition, ItemDataSO customItem = null)
        {
            resourceType = type;
            amount = itemAmount;
            _spawnPosition = initialPosition;
            transform.position = initialPosition;
            if (customItem != null)
            {
                itemData = customItem;
            }
        }

        private void Update()
        {
            // 1. Tự xoay quanh trục Y
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);

            // 2. Nếu đã khóa mục tiêu Player -> Bay thẳng về phía người chơi
            if (_targetPlayer != null)
            {
                Vector3 playerCenter = _targetPlayer.transform.position + Vector3.up * 0.8f;
                transform.position = Vector3.MoveTowards(transform.position, playerCenter, flySpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, playerCenter) < 0.4f)
                {
                    var inv = _targetPlayer.GetComponent<InventorySystem>();
                    TryCollect(inv, _targetPlayer);
                }
                return;
            }

            // 3. Hiệu ứng bập bồng hình sin khi đang nằm chờ trên đất
            float verticalOffset = Mathf.Sin((Time.time + _timeOffset) * bobSpeed) * bobHeight;
            transform.position = new Vector3(_spawnPosition.x, _spawnPosition.y + verticalOffset, _spawnPosition.z);

            // 4. Quét tìm Player trong bán kính nhặt (Non-Allocating)
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, pickupRadius, OverlapBuffer);
            for (int i = 0; i < hitCount; i++)
            {
                Collider col = OverlapBuffer[i];
                if (col == null) continue;

                if (col.TryGetComponent<PlayerStatsComponent>(out var stats) ||
                    (col.attachedRigidbody != null && col.attachedRigidbody.TryGetComponent<PlayerStatsComponent>(out stats)))
                {
                    _targetPlayer = stats;
                    break;
                }
            }
        }

        /// <summary>
        /// Chuyển được bao nhiêu item thì chỉ giảm pickup bấy nhiêu. Phần dư luôn ở lại world.
        /// Tách hàm này khỏi Update để kiểm thử giao dịch pickup mà không phụ thuộc vật lý/frame timing.
        /// </summary>
        public int TryCollect(InventorySystem inventory, PlayerStatsComponent stats)
        {
            if (inventory == null || amount <= 0) return 0;

            ItemDataSO toAdd = itemData != null ? itemData : ItemDatabase.GetItemByResourceType(resourceType);
            if (toAdd == null) return 0;

            inventory.TryAddItem(toAdd, amount, out int remainder);
            int accepted = amount - remainder;
            if (accepted <= 0)
            {
                _targetPlayer = null;
                _spawnPosition = transform.position;
                return 0;
            }

            amount = remainder;
            if (stats != null)
            {
                stats.AddResource(resourceType, accepted);
            }

            if (amount <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                _targetPlayer = null;
                _spawnPosition = transform.position;
            }

            return accepted;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.2f, 0.9f, 0.3f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, pickupRadius);
        }
    }
}

