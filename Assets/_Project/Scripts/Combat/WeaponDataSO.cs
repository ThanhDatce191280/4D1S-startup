using UnityEngine;

namespace HuyetNguyetDaiNgan.Combat
{
    public enum WeaponCategory
    {
        Sword,
        Bow,
        Crossbow,
        Axe,
        Hammer,
        Staff
    }

    /// <summary>
    /// Định nghĩa thông số vũ khí qua ScriptableObject (Data-driven design).
    /// </summary>
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Huyết Nguyệt/Combat/Weapon Data")]
    public class WeaponDataSO : ScriptableObject
    {
        [Header("General Info")]
        public string weaponId = "weapon_sword_iron_t1";
        public string weaponName = "Trường Kiếm Sắt Ấp Làng";
        public WeaponCategory category = WeaponCategory.Sword;
        public DamageType damageType = DamageType.Physical;

        [Header("Combat Stats")]
        [Tooltip("Lượng sát thương cơ bản mỗi nhát chém")]
        public float baseDamage = 25f;

        [Tooltip("Tầm quét của vũ khí (bán kính tính từ tâm nhân vật)")]
        public float attackRange = 2.4f;

        [Tooltip("Góc quét hình quạt trước mặt nhân vật (Độ: 0 -> 360)")]
        [Range(30f, 360f)]
        public float attackAngle = 110f;

        [Tooltip("Thời gian hồi chiêu giữa hai lần tấn công (giây)")]
        public float attackCooldown = 0.5f;

        [Tooltip("Lực đẩy lùi mục tiêu trúng đòn")]
        public float knockbackForce = 2.5f;
    }
}

