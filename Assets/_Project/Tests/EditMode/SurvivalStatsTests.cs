using HuyetNguyetDaiNgan.Combat;
using HuyetNguyetDaiNgan.Core;
using HuyetNguyetDaiNgan.Player;
using HuyetNguyetDaiNgan.World;
using NUnit.Framework;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Tests
{
    [TestFixture]
    public class SurvivalStatsTests
    {
        private GameObject _playerObj;
        private PlayerStatsComponent _stats;
        private HealthComponent _health;
        private SurvivalConfigSO _config;

        [SetUp]
        public void SetUp()
        {
            _playerObj = new GameObject("TestPlayerStats");
            _health = _playerObj.AddComponent<HealthComponent>();
            _health.SetMaxHealth(100f, true);

            _stats = _playerObj.AddComponent<PlayerStatsComponent>();

            _config = ScriptableObject.CreateInstance<SurvivalConfigSO>();
            _config.maxHunger = 100f;
            _config.maxStamina = 100f;
            _config.maxMana = 100f;
            _config.attackStaminaCost = 15f;
            _config.sprintStaminaDrainRate = 14f;

            _stats.SetConfig(_config);
        }

        [TearDown]
        public void TearDown()
        {
            if (_playerObj != null)
            {
                Object.DestroyImmediate(_playerObj);
            }
            if (_config != null)
            {
                Object.DestroyImmediate(_config);
            }
        }

        [Test]
        public void Stats_InitializeWithMaxValues()
        {
            Assert.AreEqual(100f, _stats.CurrentHunger);
            Assert.AreEqual(100f, _stats.CurrentStamina);
            Assert.AreEqual(100f, _stats.CurrentMana);
            Assert.AreEqual(1.0f, _stats.HungerPercent);
            Assert.AreEqual(1.0f, _stats.StaminaPercent);
            Assert.AreEqual(1.0f, _stats.ManaPercent);
        }

        [Test]
        public void Stamina_TryConsume_SucceedsWhenSufficient()
        {
            bool success = _stats.TryConsumeStamina(25f);

            Assert.IsTrue(success);
            Assert.AreEqual(75f, _stats.CurrentStamina);
        }

        [Test]
        public void Stamina_TryConsume_FailsWhenInsufficient()
        {
            _stats.TryConsumeStamina(95f);
            Assert.AreEqual(5f, _stats.CurrentStamina);

            bool success = _stats.TryConsumeStamina(15f);
            Assert.IsFalse(success, "Không được phép tiêu hao thể lực khi lượng còn lại nhỏ hơn chi phí.");
            Assert.AreEqual(5f, _stats.CurrentStamina);
        }

        [Test]
        public void Mana_TryConsume_ReducesManaCorrectly()
        {
            bool success = _stats.TryConsumeMana(40f);

            Assert.IsTrue(success);
            Assert.AreEqual(60f, _stats.CurrentMana);
        }

        [Test]
        public void ResourceCollection_IncreasesCountsAccurately()
        {
            _stats.AddResource(ResourceType.Wood, 5);
            _stats.AddResource(ResourceType.Stone, 3);
            _stats.AddResource(ResourceType.Food, 2);

            Assert.AreEqual(5, _stats.WoodCount);
            Assert.AreEqual(3, _stats.StoneCount);
            Assert.AreEqual(2, _stats.FoodCount);
        }

        [Test]
        public void ResourceNode_TakeDamage_DepletesWhenHitsExhausted()
        {
            GameObject nodeObj = new GameObject("TestTreeNode");
            BoxCollider box = nodeObj.AddComponent<BoxCollider>();
            ResourceNode node = nodeObj.AddComponent<ResourceNode>();

            Assert.AreEqual(3f, node.CurrentHealth);
            Assert.IsFalse(node.IsDead);

            // Nhát chém 1
            node.TakeDamage(new DamageInfo(10f));
            Assert.AreEqual(2f, node.CurrentHealth);
            Assert.IsFalse(node.IsDead);

            // Nhát chém 2
            node.TakeDamage(new DamageInfo(10f));
            Assert.AreEqual(1f, node.CurrentHealth);
            Assert.IsFalse(node.IsDead);

            // Nhát chém 3 -> Cạn kiệt
            node.TakeDamage(new DamageInfo(10f));
            Assert.AreEqual(0f, node.CurrentHealth);
            Assert.IsTrue(node.IsDead);

            Object.DestroyImmediate(nodeObj);
        }
    }
}

