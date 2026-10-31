using UnityEngine;

namespace HuyetNguyetDaiNgan.Combat
{
    public enum DamageType
    {
        Physical,
        Fire,
        Water,
        Earth,
        Wood,
        Ice
    }

    /// <summary>
    /// Gói dữ liệu mô tả đòn đánh trong hệ thống chiến đấu.
    /// </summary>
    public struct DamageInfo
    {
        public float Amount;
        public DamageType Type;
        public GameObject Source;
        public Vector3 HitPoint;
        public Vector3 HitNormal;
        public float KnockbackForce;

        public DamageInfo(float amount, DamageType type = DamageType.Physical, GameObject source = null, Vector3 hitPoint = default, float knockback = 0f)
        {
            Amount = Mathf.Max(0f, amount);
            Type = type;
            Source = source;
            HitPoint = hitPoint;
            HitNormal = Vector3.up;
            KnockbackForce = knockback;
        }
    }
}

