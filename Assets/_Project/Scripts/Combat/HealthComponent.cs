using System;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Combat
{
    /// <summary>
    /// Quản lý chỉ số Máu, nhận sát thương và hồi phục.
    /// Hoạt động độc lập và phát sự kiện C# Action cho UI và VFX.
    /// </summary>
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")]
        [Tooltip("Lượng máu tối đa ban đầu")]
        [SerializeField] private float maxHealth = 100f;

        [Tooltip("Cho phép đối tượng này bất tử (dùng cho bia tập đánh)")]
        [SerializeField] private bool isInvulnerable = false;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => maxHealth;
        public bool IsDead => CurrentHealth <= 0f && !isInvulnerable;
        public float HealthPercent => Mathf.Clamp01(CurrentHealth / Mathf.Max(maxHealth, 0.001f));

        // Events
        public event Action<DamageInfo> OnDamaged;
        public event Action<float> OnHealed;
        public event Action OnDied;

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(DamageInfo info)
        {
            if (IsDead) return;

            if (isInvulnerable)
            {
                OnDamaged?.Invoke(info);
                return;
            }

            float actualDamage = Mathf.Min(CurrentHealth, info.Amount);
            CurrentHealth -= actualDamage;

            OnDamaged?.Invoke(info);

            if (CurrentHealth <= 0f)
            {
                CurrentHealth = 0f;
                OnDied?.Invoke();
            }
        }

        public void Heal(float amount)
        {
            if (IsDead || amount <= 0f) return;

            float prevHealth = CurrentHealth;
            CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
            float actualHealed = CurrentHealth - prevHealth;

            if (actualHealed > 0f)
            {
                OnHealed?.Invoke(actualHealed);
            }
        }

        public void SetMaxHealth(float newMax, bool healToFull = false)
        {
            maxHealth = Mathf.Max(1f, newMax);
            if (healToFull)
            {
                CurrentHealth = maxHealth;
            }
            else
            {
                CurrentHealth = Mathf.Min(CurrentHealth, maxHealth);
            }
        }

        public void SetInvulnerable(bool invulnerable)
        {
            isInvulnerable = invulnerable;
        }
    }
}

