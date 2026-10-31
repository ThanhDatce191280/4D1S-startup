using HuyetNguyetDaiNgan.Combat;
using NUnit.Framework;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Tests
{
    [TestFixture]
    public class HealthComponentTests
    {
        private GameObject _targetObj;
        private HealthComponent _health;

        [SetUp]
        public void SetUp()
        {
            _targetObj = new GameObject("TestDamageable");
            _health = _targetObj.AddComponent<HealthComponent>();
            _health.SetMaxHealth(100f, true);
        }

        [TearDown]
        public void TearDown()
        {
            if (_targetObj != null)
            {
                Object.DestroyImmediate(_targetObj);
            }
        }

        [Test]
        public void Health_StartsAtMaxHealth()
        {
            Assert.AreEqual(100f, _health.MaxHealth);
            Assert.AreEqual(100f, _health.CurrentHealth);
            Assert.IsFalse(_health.IsDead);
        }

        [Test]
        public void TakeDamage_ReducesHealthCorrectly()
        {
            _health.TakeDamage(new DamageInfo(35f));

            Assert.AreEqual(65f, _health.CurrentHealth);
            Assert.IsFalse(_health.IsDead);
        }

        [Test]
        public void TakeDamage_ExcessiveDamage_ClampsToZeroAndDies()
        {
            bool diedEventFired = false;
            _health.OnDied += () => diedEventFired = true;

            _health.TakeDamage(new DamageInfo(150f));

            Assert.AreEqual(0f, _health.CurrentHealth);
            Assert.IsTrue(_health.IsDead);
            Assert.IsTrue(diedEventFired, "Sự kiện OnDied phải được kích hoạt khi máu về 0.");
        }

        [Test]
        public void Heal_IncreasesHealth_ButNeverExceedsMax()
        {
            _health.TakeDamage(new DamageInfo(50f));
            Assert.AreEqual(50f, _health.CurrentHealth);

            _health.Heal(80f);
            Assert.AreEqual(100f, _health.CurrentHealth, "Máu sau khi hồi không được vượt quá MaxHealth.");
        }

        [Test]
        public void InvulnerableObject_NeverTakesDamage()
        {
            _health.SetInvulnerable(true);
            _health.TakeDamage(new DamageInfo(50f));

            Assert.AreEqual(100f, _health.CurrentHealth);
            Assert.IsFalse(_health.IsDead);
        }

        [Test]
        public void DeadObject_IgnoresFurtherDamage()
        {
            _health.TakeDamage(new DamageInfo(100f));
            Assert.IsTrue(_health.IsDead);

            int damageEventCount = 0;
            _health.OnDamaged += _ => damageEventCount++;

            _health.TakeDamage(new DamageInfo(50f));
            Assert.AreEqual(0, damageEventCount, "Đối tượng đã chết không được tiếp tục nhận sát thương.");
        }
    }
}

