using System;
using HuyetNguyetDaiNgan.Combat;
using HuyetNguyetDaiNgan.Core;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Player
{
    public enum ResourceType
    {
        Wood,
        Stone,
        Food
    }

    /// <summary>
    /// Quản lý 4 thanh trạng thái sinh tồn (HP, Hunger, Stamina, Mana) theo quy tắc GDD Mục 3.1.
    /// </summary>
    public class PlayerStatsComponent : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private SurvivalConfigSO config;

        // Current Stat Values
        public float CurrentHunger { get; private set; }
        public float CurrentStamina { get; private set; }
        public float CurrentMana { get; private set; }

        public float MaxHunger => config != null ? config.maxHunger : 100f;
        public float MaxStamina => config != null ? config.maxStamina : 100f;
        public float MaxMana => config != null ? config.maxMana : 100f;

        public float HungerPercent => Mathf.Clamp01(CurrentHunger / Mathf.Max(MaxHunger, 0.001f));
        public float StaminaPercent => Mathf.Clamp01(CurrentStamina / Mathf.Max(MaxStamina, 0.001f));
        public float ManaPercent => Mathf.Clamp01(CurrentMana / Mathf.Max(MaxMana, 0.001f));

        // Collected Resources Counter
        public int WoodCount { get; private set; }
        public int StoneCount { get; private set; }
        public int FoodCount { get; private set; }

        // Dependencies & Timers
        private HealthComponent _healthComponent;
        private float _staminaRegenTimer;
        private float _starvationTimer;

        // Events
        public event Action OnStatsChanged;
        public event Action<ResourceType, int> OnResourceCollected;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();

            CurrentHunger = MaxHunger;
            CurrentStamina = MaxStamina;
            CurrentMana = MaxMana;
        }

        public void SetConfig(SurvivalConfigSO customConfig)
        {
            config = customConfig;
            CurrentHunger = MaxHunger;
            CurrentStamina = MaxStamina;
            CurrentMana = MaxMana;
        }

        private void Update()
        {
            if (_healthComponent != null && _healthComponent.IsDead) return;

            UpdateHunger();
            UpdateStamina();
            UpdateMana();

            OnStatsChanged?.Invoke();
        }

        private void UpdateHunger()
        {
            if (config == null) return;

            // 1. Tiêu hao thức ăn dần theo thời gian
            CurrentHunger = Mathf.Max(0f, CurrentHunger - config.hungerDrainRate * Time.deltaTime);

            // 2. No nê (> 80%): Tự hồi phục nhẹ HP
            if (CurrentHunger >= config.wellFedThreshold && _healthComponent != null)
            {
                if (_healthComponent.CurrentHealth < _healthComponent.MaxHealth)
                {
                    _healthComponent.Heal(config.wellFedHealthRegenRate * Time.deltaTime);
                }
            }

            // 3. Chết đói (= 0%): Rút máu theo giây
            if (CurrentHunger <= 0f && _healthComponent != null)
            {
                _starvationTimer += Time.deltaTime;
                if (_starvationTimer >= 1.0f)
                {
                    _starvationTimer = 0f;
                    _healthComponent.TakeDamage(new DamageInfo(config.starvationDamageRate, DamageType.Physical));
                }
            }
            else
            {
                _starvationTimer = 0f;
            }
        }

        private void UpdateStamina()
        {
            if (config == null) return;

            _staminaRegenTimer -= Time.deltaTime;

            // Hồi phục thể lực khi hết thời gian trễ
            if (_staminaRegenTimer <= 0f && CurrentStamina < MaxStamina)
            {
                CurrentStamina = Mathf.Min(MaxStamina, CurrentStamina + config.staminaRegenRate * Time.deltaTime);
            }
        }

        private void UpdateMana()
        {
            if (config == null) return;

            if (CurrentMana < MaxMana)
            {
                CurrentMana = Mathf.Min(MaxMana, CurrentMana + config.manaRegenRate * Time.deltaTime);
            }
        }

        public bool TryConsumeStamina(float amount)
        {
            if (amount < 0f) return false;

            if (CurrentStamina >= amount)
            {
                CurrentStamina = Mathf.Max(0f, CurrentStamina - amount);
                if (config != null) _staminaRegenTimer = config.staminaRegenDelay;
                return true;
            }
            return false;
        }

        public void ConsumeStaminaContinuous(float ratePerSecond)
        {
            CurrentStamina = Mathf.Max(0f, CurrentStamina - ratePerSecond * Time.deltaTime);
            if (config != null) _staminaRegenTimer = config.staminaRegenDelay;
        }

        public bool TryConsumeMana(float amount)
        {
            if (amount < 0f) return false;

            if (CurrentMana >= amount)
            {
                CurrentMana = Mathf.Max(0f, CurrentMana - amount);
                return true;
            }
            return false;
        }

        public void AddHunger(float amount)
        {
            CurrentHunger = Mathf.Clamp(CurrentHunger + amount, 0f, MaxHunger);
        }

        public void RestoreStamina(float amount)
        {
            if (amount <= 0f) return;
            CurrentStamina = Mathf.Min(MaxStamina, CurrentStamina + amount);
        }

        public void RestoreMana(float amount)
        {
            if (amount <= 0f) return;
            CurrentMana = Mathf.Min(MaxMana, CurrentMana + amount);
        }

        public void AddResource(ResourceType type, int amount)
        {
            switch (type)
            {
                case ResourceType.Wood:
                    WoodCount += amount;
                    break;
                case ResourceType.Stone:
                    StoneCount += amount;
                    break;
                case ResourceType.Food:
                    FoodCount += amount;
                    // Không tự động ăn khi nhặt. Quả được thu thập vào túi đồ để người chơi chủ động ăn khi đói.
                    break;
            }

            OnResourceCollected?.Invoke(type, amount);
        }
    }
}

