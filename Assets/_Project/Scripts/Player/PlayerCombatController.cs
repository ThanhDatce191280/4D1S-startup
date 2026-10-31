using System;
using HuyetNguyetDaiNgan.Combat;
using HuyetNguyetDaiNgan.UI;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Player
{
    /// <summary>
    /// Xử lý vung kiếm, kiểm tra trúng đòn hình quạt và gây sát thương.
    /// Dùng OverlapSphereNonAlloc để không phát sinh Garbage Collection trong chiến đấu.
    /// </summary>
    public class PlayerCombatController : MonoBehaviour
    {
        [Header("Weapon Config")]
        [Tooltip("Cấu hình vũ khí hiện tại")]
        [SerializeField] private WeaponDataSO equippedWeapon;

        [Header("Visual References")]
        [Tooltip("Transform xoay mặt của nhân vật để xác định hướng chém")]
        [SerializeField] private Transform visualTransform;

        [Header("Layer Mask")]
        [Tooltip("Các layer đối tượng có thể bị chém trúng")]
        [SerializeField] private LayerMask attackableLayers = ~0;

        // Dependencies & State
        private IPlayerInput _playerInput;
        private PlayerStatsComponent _statsComponent;
        private float _lastAttackTime = -99f;
        private static readonly Collider[] HitBuffer = new Collider[16];

        public WeaponDataSO EquippedWeapon => equippedWeapon;
        public bool CanAttack => !InventoryCraftingUI.IsOpenGlobal
            && !InventoryCraftingUI.IsMouseOverAnyUI()
            && Time.time >= _lastAttackTime + (equippedWeapon != null ? equippedWeapon.attackCooldown : 0.5f)
            && (_statsComponent == null || _statsComponent.CurrentStamina >= 15f);

        public event Action OnAttackStarted;
        public event Action<int> OnAttackLanded; // Trả về số lượng mục tiêu trúng đòn

        private void Awake()
        {
            _playerInput = GetComponent<IPlayerInput>();
            _statsComponent = GetComponent<PlayerStatsComponent>();

            if (visualTransform == null)
            {
                Transform foundVisual = transform.Find("PlayerVisual");
                visualTransform = foundVisual != null ? foundVisual : transform;
            }
        }

        public void EquipWeapon(WeaponDataSO newWeapon)
        {
            equippedWeapon = newWeapon;
        }

        private void Update()
        {
            if (_playerInput == null)
            {
                _playerInput = GetComponent<IPlayerInput>();
                if (_playerInput == null) return;
            }

            if (_statsComponent == null)
            {
                _statsComponent = GetComponent<PlayerStatsComponent>();
            }

            if (_playerInput.AttackTriggered && CanAttack)
            {
                PerformAttack();
            }
        }

        private void PerformAttack()
        {
            if (equippedWeapon == null) return;

            // Tiêu hao thể lực khi vung kiếm
            if (_statsComponent != null)
            {
                if (!_statsComponent.TryConsumeStamina(15f))
                {
                    return;
                }
            }

            _lastAttackTime = Time.time;
            OnAttackStarted?.Invoke();

            Vector3 attackOrigin = transform.position + Vector3.up * 0.8f;
            Vector3 facingDir = visualTransform != null ? visualTransform.forward : transform.forward;
            facingDir.y = 0f;
            facingDir.Normalize();

            float range = equippedWeapon.attackRange;
            float halfAngle = equippedWeapon.attackAngle * 0.5f;

            // Quét hình cầu trước mặt bằng bộ đệm không cấp phát rác
            int hitCount = Physics.OverlapSphereNonAlloc(attackOrigin, range, HitBuffer, attackableLayers, QueryTriggerInteraction.Ignore);
            int validHits = 0;

            for (int i = 0; i < hitCount; i++)
            {
                Collider col = HitBuffer[i];
                if (col == null) continue;

                // Bỏ qua chính bản thân người chơi
                if (col.gameObject == gameObject || col.transform.IsChildOf(transform))
                {
                    continue;
                }

                // Kiểm tra mục tiêu có nằm trong góc quét hình quạt trước mặt không
                Vector3 toTarget = col.bounds.center - attackOrigin;
                toTarget.y = 0f;

                if (toTarget.sqrMagnitude < 0.01f || Vector3.Angle(facingDir, toTarget.normalized) <= halfAngle)
                {
                    if (col.TryGetComponent<IDamageable>(out var damageable) || col.GetComponentInParent<IDamageable>() is { } parentDamageable && (damageable = parentDamageable) != null)
                    {
                        if (!damageable.IsDead)
                        {
                            DamageInfo info = new DamageInfo(
                                equippedWeapon.baseDamage,
                                equippedWeapon.damageType,
                                gameObject,
                                col.bounds.center,
                                equippedWeapon.knockbackForce
                            );

                            damageable.TakeDamage(info);
                            validHits++;
                        }
                    }
                }
            }

            OnAttackLanded?.Invoke(validHits);
        }

        private void OnDrawGizmosSelected()
        {
            if (equippedWeapon == null) return;

            Transform vis = visualTransform != null ? visualTransform : transform;
            Vector3 origin = transform.position + Vector3.up * 0.8f;
            float range = equippedWeapon.attackRange;
            float halfAngle = equippedWeapon.attackAngle * 0.5f;

            Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.35f);
            Gizmos.DrawWireSphere(origin, range);

            Vector3 leftRay = Quaternion.Euler(0f, -halfAngle, 0f) * vis.forward;
            Vector3 rightRay = Quaternion.Euler(0f, halfAngle, 0f) * vis.forward;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, leftRay * range);
            Gizmos.DrawRay(origin, rightRay * range);
            Gizmos.DrawRay(origin, vis.forward * range);
        }
    }
}

