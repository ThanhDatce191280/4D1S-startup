using UnityEngine;

namespace HuyetNguyetDaiNgan.Core
{
    /// <summary>
    /// File cấu hình thông số cân bằng sinh tồn (ScriptableObject).
    /// </summary>
    [CreateAssetMenu(fileName = "SurvivalConfig", menuName = "Huyết Nguyệt/Core/Survival Config")]
    public class SurvivalConfigSO : ScriptableObject
    {
        [Header("Hunger (Thức Ăn)")]
        [Tooltip("Lượng thức ăn tối đa")]
        public float maxHunger = 100f;

        [Tooltip("Tốc độ tiêu hao thức ăn mỗi giây")]
        public float hungerDrainRate = 0.4f;

        [Tooltip("Ngưỡng no để tự động hồi máu (> 80%)")]
        public float wellFedThreshold = 80f;

        [Tooltip("Lượng máu hồi phục mỗi giây khi no nê")]
        public float wellFedHealthRegenRate = 1.5f;

        [Tooltip("Lượng máu bị rút mỗi giây khi chết đói (= 0%)")]
        public float starvationDamageRate = 3.0f;

        [Header("Stamina (Thể Lực)")]
        [Tooltip("Lượng thể lực tối đa")]
        public float maxStamina = 100f;

        [Tooltip("Thể lực tiêu hao mỗi giây khi chạy nước rút")]
        public float sprintStaminaDrainRate = 14f;

        [Tooltip("Thể lực tiêu hao mỗi nhát chém kiếm")]
        public float attackStaminaCost = 15f;

        [Tooltip("Tốc độ tự hồi phục thể lực mỗi giây khi đi bộ hoặc đứng yên")]
        public float staminaRegenRate = 22f;

        [Tooltip("Thời gian trễ (giây) sau hành động trước khi thể lực bắt đầu hồi")]
        public float staminaRegenDelay = 0.8f;

        [Header("Mana (Năng Lượng)")]
        [Tooltip("Lượng Mana tối đa")]
        public float maxMana = 100f;

        [Tooltip("Tốc độ tự hồi phục Mana mỗi giây")]
        public float manaRegenRate = 5f;
    }
}

