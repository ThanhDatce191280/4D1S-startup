using System.Collections.Generic;
using HuyetNguyetDaiNgan.Combat;
using HuyetNguyetDaiNgan.Core;
using HuyetNguyetDaiNgan.Inventory;
using HuyetNguyetDaiNgan.Player;
using NUnit.Framework;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Tests
{
    [TestFixture]
    public class InventoryCraftingTests
    {
        private GameObject _playerObj;
        private InventorySystem _inventory;
        private CraftingSystem _crafting;
        private HealthComponent _health;
        private PlayerStatsComponent _stats;

        private ItemDataSO _woodItem;
        private ItemDataSO _stoneItem;
        private ItemDataSO _riceItem;
        private ItemDataSO _axeItem;
        private CraftingRecipeSO _axeRecipe;

        [SetUp]
        public void SetUp()
        {
            _playerObj = new GameObject("TestPlayer_Inventory");
            _health = _playerObj.AddComponent<HealthComponent>();
            _health.SetMaxHealth(100f, true);

            _stats = _playerObj.AddComponent<PlayerStatsComponent>();
            _inventory = _playerObj.AddComponent<InventorySystem>();
            _crafting = _playerObj.AddComponent<CraftingSystem>();
            _crafting.SetInventory(_inventory);

            // Mock Items
            _woodItem = ScriptableObject.CreateInstance<ItemDataSO>();
            _woodItem.itemId = "resource_wood";
            _woodItem.itemName = "Gỗ Rừng";
            _woodItem.maxStackSize = 5;
            _woodItem.itemType = ItemType.Resource;

            _stoneItem = ScriptableObject.CreateInstance<ItemDataSO>();
            _stoneItem.itemId = "resource_stone";
            _stoneItem.itemName = "Đá Cuội";
            _stoneItem.maxStackSize = 5;
            _stoneItem.itemType = ItemType.Resource;

            _riceItem = ScriptableObject.CreateInstance<ItemDataSO>();
            _riceItem.itemId = "consumable_bamboo_rice";
            _riceItem.itemName = "Cơm Lam Ống Nứa";
            _riceItem.itemType = ItemType.Consumable;
            _riceItem.healthRestore = 25f;
            _riceItem.hungerRestore = 40f;
            _riceItem.maxStackSize = 10;

            _axeItem = ScriptableObject.CreateInstance<ItemDataSO>();
            _axeItem.itemId = "tool_stone_axe_t1";
            _axeItem.itemName = "Rìu Đá Thô Sơ";
            _axeItem.itemType = ItemType.Tool;
            _axeItem.maxStackSize = 1;

            // Mock Recipe: 3 Wood + 2 Stone -> 1 Axe
            _axeRecipe = ScriptableObject.CreateInstance<CraftingRecipeSO>();
            _axeRecipe.recipeId = "recipe_stone_axe";
            _axeRecipe.recipeName = "Rìu Đá Thô Sơ";
            _axeRecipe.ingredients = new List<RecipeIngredient>
            {
                new RecipeIngredient { item = _woodItem, amount = 3 },
                new RecipeIngredient { item = _stoneItem, amount = 2 }
            };
            _axeRecipe.resultItem = _axeItem;
            _axeRecipe.resultAmount = 1;

            _crafting.AddRecipe(_axeRecipe);
        }

        [TearDown]
        public void TearDown()
        {
            if (_playerObj != null) Object.DestroyImmediate(_playerObj);
            if (_woodItem != null) Object.DestroyImmediate(_woodItem);
            if (_stoneItem != null) Object.DestroyImmediate(_stoneItem);
            if (_riceItem != null) Object.DestroyImmediate(_riceItem);
            if (_axeItem != null) Object.DestroyImmediate(_axeItem);
            if (_axeRecipe != null) Object.DestroyImmediate(_axeRecipe);
        }

        [Test]
        public void Inventory_AddItem_AddsToFirstAvailableSlot()
        {
            bool added = _inventory.TryAddItem(_woodItem, 3);

            Assert.IsTrue(added);
            Assert.AreEqual(3, _inventory.GetItemCount("resource_wood"));
            Assert.AreEqual("resource_wood", _inventory.GetSlot(0).Item.itemId);
            Assert.AreEqual(3, _inventory.GetSlot(0).Quantity);
        }

        [Test]
        public void Inventory_AddItem_StacksUpToMaxStackSize()
        {
            _inventory.TryAddItem(_woodItem, 3);
            _inventory.TryAddItem(_woodItem, 2);

            Assert.AreEqual(5, _inventory.GetSlot(0).Quantity);
            Assert.AreEqual(5, _inventory.GetItemCount("resource_wood"));
            Assert.IsTrue(_inventory.GetSlot(1).IsEmpty, "Ô thứ 2 vẫn phải trống vì ô đầu vừa khít 5.");
        }

        [Test]
        public void Inventory_AddItem_OverflowsToNextSlotWhenStackFull()
        {
            _inventory.TryAddItem(_woodItem, 3);
            _inventory.TryAddItem(_woodItem, 4); // Tổng 7, maxStack 5 -> Ô 0 có 5, Ô 1 có 2

            Assert.AreEqual(7, _inventory.GetItemCount("resource_wood"));
            Assert.AreEqual(5, _inventory.GetSlot(0).Quantity);
            Assert.AreEqual(2, _inventory.GetSlot(1).Quantity);
        }

        [Test]
        public void Inventory_RemoveItem_DeductsAccurately()
        {
            _inventory.TryAddItem(_woodItem, 5);
            _inventory.TryAddItem(_woodItem, 2); // Tổng 7

            bool removed = _inventory.TryRemoveItem("resource_wood", 4);

            Assert.IsTrue(removed);
            Assert.AreEqual(3, _inventory.GetItemCount("resource_wood"));
        }

        [Test]
        public void Inventory_UseItem_Consumable_RestoresStatsAndDecrements()
        {
            // Trừ máu và thức ăn người chơi
            _health.TakeDamage(new DamageInfo(50f));
            _stats.AddHunger(-50f);
            Assert.AreEqual(50f, _health.CurrentHealth);

            _inventory.TryAddItem(_riceItem, 2);
            _inventory.UseItem(0); // Dùng Cơm Lam (+25 HP, +40 Hunger)

            Assert.AreEqual(75f, _health.CurrentHealth);
            Assert.AreEqual(90f, _stats.CurrentHunger, "Độ No ban đầu 100 - 50 = 50, ăn Cơm Lam (+40) tăng lên 90.");
            Assert.AreEqual(1, _inventory.GetItemCount("consumable_bamboo_rice"));
        }

        [Test]
        public void Inventory_TotalSlots_Is26()
        {
            Assert.AreEqual(26, _inventory.TotalSlots, "Tổng số ô phải là 26 (6 Hotbar + 20 Ba Lô).");
            Assert.AreEqual(26, _inventory.Slots.Count);
        }

        [Test]
        public void Inventory_SwapSlots_SwapsBetweenHotbarAndBag()
        {
            // Đặt Gỗ vào Ba Lô (Ô 6)
            _inventory.GetSlot(6).SetItem(_woodItem, 4);
            // Ô Hotbar 0 đang trống
            Assert.IsTrue(_inventory.GetSlot(0).IsEmpty);

            // Chuyển Gỗ từ Ba Lô (Ô 6) ra Hotbar (Ô 0)
            _inventory.SwapSlots(6, 0);

            Assert.IsFalse(_inventory.GetSlot(0).IsEmpty, "Ô Hotbar 0 giờ phải có đồ.");
            Assert.AreEqual("resource_wood", _inventory.GetSlot(0).Item.itemId);
            Assert.AreEqual(4, _inventory.GetSlot(0).Quantity);
            Assert.IsTrue(_inventory.GetSlot(6).IsEmpty, "Ô Ba Lô 6 giờ phải trống.");

            // Đổi ngược lại từ Hotbar (Ô 0) về Ba Lô (Ô 6)
            _inventory.SwapSlots(0, 6);
            Assert.IsTrue(_inventory.GetSlot(0).IsEmpty);
            Assert.IsFalse(_inventory.GetSlot(6).IsEmpty);
            Assert.AreEqual(4, _inventory.GetSlot(6).Quantity);
        }

        [Test]
        public void Inventory_SwapSlots_MergesMatchingItems()
        {
            // Ô 0 có 2 Gỗ, Ô 1 có 2 Gỗ (maxStack = 5)
            _inventory.GetSlot(0).SetItem(_woodItem, 2);
            _inventory.GetSlot(1).SetItem(_woodItem, 2);

            // Gộp Ô 0 vào Ô 1
            _inventory.SwapSlots(0, 1);

            Assert.AreEqual(4, _inventory.GetSlot(1).Quantity, "Ô 1 được gộp thành 4 Gỗ.");
            Assert.IsTrue(_inventory.GetSlot(0).IsEmpty, "Ô 0 được dọn trống sau khi gộp hết.");
        }

        [Test]
        public void Crafting_CanCraft_ChecksIngredientsAccurately()
        {
            Assert.IsFalse(_crafting.CanCraft(_axeRecipe), "Chưa có nguyên liệu thì không thể chế tạo.");

            _inventory.TryAddItem(_woodItem, 3);
            Assert.IsFalse(_crafting.CanCraft(_axeRecipe), "Mới có gỗ, thiếu đá thì chưa thể chế tạo.");

            _inventory.TryAddItem(_stoneItem, 2);
            Assert.IsTrue(_crafting.CanCraft(_axeRecipe), "Đã đủ 3 gỗ và 2 đá thì phải chế tạo được.");
        }

        [Test]
        public void Crafting_TryCraft_DeductsIngredientsAndGivesResult()
        {
            _inventory.TryAddItem(_woodItem, 5);
            _inventory.TryAddItem(_stoneItem, 4);

            bool success = _crafting.TryCraft(_axeRecipe);

            Assert.IsTrue(success);
            Assert.AreEqual(2, _inventory.GetItemCount("resource_wood"), "5 Gỗ trừ 3 còn 2.");
            Assert.AreEqual(2, _inventory.GetItemCount("resource_stone"), "4 Đá trừ 2 còn 2.");
            Assert.AreEqual(1, _inventory.GetItemCount("tool_stone_axe_t1"), "Nhận được 1 Rìu Đá trong túi đồ.");
        }
    }
}

