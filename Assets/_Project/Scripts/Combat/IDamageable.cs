namespace HuyetNguyetDaiNgan.Combat
{
    /// <summary>
    /// Giao diện chuẩn cho mọi thực thể có thể nhận sát thương (Player, Quái vật, Công trình, Cây cối).
    /// </summary>
    public interface IDamageable
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }
        bool IsDead { get; }

        void TakeDamage(DamageInfo info);
        void Heal(float amount);
    }
}

