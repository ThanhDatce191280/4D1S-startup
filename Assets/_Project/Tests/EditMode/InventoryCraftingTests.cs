using System.Collections.Generic;
using HuyetNguyetDaiNgan.Combat;
using HuyetNguyetDaiNgan.Core;
using HuyetNguyetDaiNgan.Inventory;
using HuyetNguyetDaiNgan.Player;
using HuyetNguyetDaiNgan.World;
using NUnit.Framework;
using UnityEditor;
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
        public void Inventory_TotalSlots_Is20()
        {
            Assert.AreEqual(20, _inventory.TotalSlots, "Túi M5 phải có đúng 20 ô; 6 ô đầu chỉ là hotbar tham chiếu.");
            Assert.AreEqual(20, _inventory.Slots.Count);
        }

        [Test]
        public void Inventory_SwapSlots_SwapsBetweenHotbarAndBag()
        {
            // Đặt Gỗ vào ô 6 (ngoài 6 ô dùng nhanh)
            _inventory.GetSlot(6).SetItem(_woodItem, 4);
            // Ô Hotbar 0 đang trống
            Assert.IsTrue(_inventory.GetSlot(0).IsEmpty);

            // Chuyển Gỗ từ Ba Lô (Ô 6) ra Hotbar (Ô 0)
            _inventory.SwapSlots(6, 0);

            Assert.IsFalse(_inventory.GetSlot(0).IsEmpty, "Ô Hotbar 0 giờ phải có đồ.");
            Assert.AreEqual("resource_wood", _inventory.GetSlot(0).Item.itemId);
            Assert.AreEqual(4, _inventory.GetSlot(0).Quantity);
            Assert.IsTrue(_inventory.GetSlot(6).IsEmpty, "Ô 6 giờ phải trống.");

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

        [Test]
        public void Inventory_AddItem_WhenOnlyPartFits_ReturnsExactRemainder()
        {
            var filler = CreateItem("filler", ItemType.Resource, 1);
            try
            {
                _inventory.GetSlot(0).SetItem(_woodItem, 4);
                for (int i = 1; i < _inventory.TotalSlots; i++)
                {
                    _inventory.GetSlot(i).SetItem(filler, 1);
                }

                bool acceptedAny = _inventory.TryAddItem(_woodItem, 3, out int remainder);

                Assert.IsTrue(acceptedAny);
                Assert.AreEqual(2, remainder);
                Assert.AreEqual(5, _inventory.GetItemCount(_woodItem.itemId));
            }
            finally
            {
                Object.DestroyImmediate(filler);
            }
        }

        [Test]
        public void Inventory_SplitStack_IsAtomicAndDoesNotDuplicate()
        {
            _inventory.GetSlot(0).SetItem(_woodItem, 5);

            Assert.IsTrue(_inventory.TrySplitStack(0, 1, 2));
            Assert.AreEqual(3, _inventory.GetSlot(0).Quantity);
            Assert.AreEqual(2, _inventory.GetSlot(1).Quantity);
            Assert.AreEqual(5, _inventory.GetItemCount(_woodItem.itemId));

            Assert.IsFalse(_inventory.TrySplitStack(0, 1, 1), "Không được tách vào ô đang có đồ.");
            Assert.IsFalse(_inventory.TrySplitStack(0, 2, 3), "Không được tách toàn bộ stack bằng thao tác split.");
            Assert.AreEqual(5, _inventory.GetItemCount(_woodItem.itemId));
        }

        [Test]
        public void InventorySlot_SetItem_ClampsToConfiguredMaxAndClearsInvalidQuantity()
        {
            var slot = new InventorySlot();
            slot.SetItem(_woodItem, 999);
            Assert.AreEqual(_woodItem.maxStackSize, slot.Quantity);

            slot.SetItem(_woodItem, -1);
            Assert.IsTrue(slot.IsEmpty);
            Assert.AreEqual(0, slot.Quantity);
        }

        [Test]
        public void Inventory_InvalidConsumable_DoesNotDisappear()
        {
            var invalidFood = CreateItem("consumable_invalid", ItemType.Consumable, 10);
            try
            {
                _inventory.TryAddItem(invalidFood, 1);

                Assert.IsFalse(_inventory.UseItem(0));
                Assert.AreEqual(1, _inventory.GetItemCount(invalidFood.itemId));
            }
            finally
            {
                Object.DestroyImmediate(invalidFood);
            }
        }

        [Test]
        public void Inventory_Consumable_WhenDead_DoesNotConsumeOrRevive()
        {
            _inventory.TryAddItem(_riceItem, 1);
            _health.TakeDamage(new DamageInfo(1000f));

            Assert.IsFalse(_inventory.UseItem(0));
            Assert.IsTrue(_health.IsDead);
            Assert.AreEqual(1, _inventory.GetItemCount(_riceItem.itemId));
        }

        [Test]
        public void Inventory_EquippedWeapon_IsClearedWhenLastOwnedItemIsRemoved()
        {
            var weaponData = ScriptableObject.CreateInstance<WeaponDataSO>();
            var weaponItem = CreateItem("weapon_test", ItemType.Weapon, 1);
            weaponItem.weaponData = weaponData;
            var combat = _playerObj.AddComponent<PlayerCombatController>();
            try
            {
                _inventory.TryAddItem(weaponItem, 1);
                Assert.IsTrue(_inventory.UseItem(0));
                Assert.AreSame(weaponData, combat.EquippedWeapon);

                Assert.IsTrue(_inventory.TryRemoveItem(weaponItem.itemId, 1));
                Assert.IsNull(combat.EquippedWeapon);
            }
            finally
            {
                Object.DestroyImmediate(weaponItem);
                Object.DestroyImmediate(weaponData);
            }
        }

        [Test]
        public void Crafting_DuplicateIngredientRows_AreSummedBeforeValidation()
        {
            var duplicateRecipe = ScriptableObject.CreateInstance<CraftingRecipeSO>();
            duplicateRecipe.recipeId = "recipe_duplicate_rows";
            duplicateRecipe.resultItem = _axeItem;
            duplicateRecipe.resultAmount = 1;
            duplicateRecipe.ingredients = new List<RecipeIngredient>
            {
                new RecipeIngredient { item = _woodItem, amount = 2 },
                new RecipeIngredient { item = _woodItem, amount = 2 }
            };
            try
            {
                _inventory.TryAddItem(_woodItem, 3);
                Assert.IsFalse(_crafting.CanCraft(duplicateRecipe));
                Assert.IsFalse(_crafting.TryCraft(duplicateRecipe));
                Assert.AreEqual(3, _inventory.GetItemCount(_woodItem.itemId));

                _inventory.TryAddItem(_woodItem, 1);
                Assert.IsTrue(_crafting.TryCraft(duplicateRecipe));
                Assert.AreEqual(0, _inventory.GetItemCount(_woodItem.itemId));
                Assert.AreEqual(1, _inventory.GetItemCount(_axeItem.itemId));
            }
            finally
            {
                Object.DestroyImmediate(duplicateRecipe);
            }
        }

        [Test]
        public void Crafting_WhenResultCannotFit_PreservesAllIngredients()
        {
            var filler = CreateItem("filler", ItemType.Resource, 1);
            try
            {
                _inventory.GetSlot(0).SetItem(_woodItem, 4);
                _inventory.GetSlot(1).SetItem(_stoneItem, 3);
                for (int i = 2; i < _inventory.TotalSlots; i++)
                {
                    _inventory.GetSlot(i).SetItem(filler, 1);
                }

                Assert.IsFalse(_crafting.CanCraft(_axeRecipe));
                Assert.IsFalse(_crafting.TryCraft(_axeRecipe));
                Assert.AreEqual(4, _inventory.GetItemCount(_woodItem.itemId));
                Assert.AreEqual(3, _inventory.GetItemCount(_stoneItem.itemId));
                Assert.AreEqual(0, _inventory.GetItemCount(_axeItem.itemId));
            }
            finally
            {
                Object.DestroyImmediate(filler);
            }
        }

        [Test]
        public void Crafting_WhenIngredientsFreeSlots_CommitsAtomically()
        {
            var filler = CreateItem("filler", ItemType.Resource, 1);
            try
            {
                _inventory.GetSlot(0).SetItem(_woodItem, 3);
                _inventory.GetSlot(1).SetItem(_stoneItem, 2);
                for (int i = 2; i < _inventory.TotalSlots; i++)
                {
                    _inventory.GetSlot(i).SetItem(filler, 1);
                }

                Assert.IsTrue(_crafting.CanCraft(_axeRecipe));
                Assert.IsTrue(_crafting.TryCraft(_axeRecipe));
                Assert.AreEqual(0, _inventory.GetItemCount(_woodItem.itemId));
                Assert.AreEqual(0, _inventory.GetItemCount(_stoneItem.itemId));
                Assert.AreEqual(1, _inventory.GetItemCount(_axeItem.itemId));
            }
            finally
            {
                Object.DestroyImmediate(filler);
            }
        }

        [Test]
        public void Crafting_InvalidRecipeReference_DoesNotMutateInventory()
        {
            var invalidRecipe = ScriptableObject.CreateInstance<CraftingRecipeSO>();
            invalidRecipe.recipeId = "recipe_invalid";
            invalidRecipe.resultItem = _axeItem;
            invalidRecipe.resultAmount = 1;
            invalidRecipe.ingredients = new List<RecipeIngredient>
            {
                new RecipeIngredient { item = null, amount = 1 }
            };
            _inventory.TryAddItem(_woodItem, 3);
            try
            {
                Assert.IsFalse(_crafting.TryCraft(invalidRecipe));
                Assert.AreEqual(3, _inventory.GetItemCount(_woodItem.itemId));
            }
            finally
            {
                Object.DestroyImmediate(invalidRecipe);
            }
        }

        [Test]
        public void Pickup_PartialCapacity_LeavesExactRemainderInWorld()
        {
            var filler = CreateItem("filler", ItemType.Resource, 1);
            var pickupObject = new GameObject("Pickup");
            var pickup = pickupObject.AddComponent<WorldItemPickup>();
            try
            {
                _inventory.GetSlot(0).SetItem(_woodItem, 4);
                for (int i = 1; i < _inventory.TotalSlots; i++)
                {
                    _inventory.GetSlot(i).SetItem(filler, 1);
                }
                pickup.Initialize(ResourceType.Wood, 3, Vector3.zero, _woodItem);

                Assert.AreEqual(1, pickup.TryCollect(_inventory, _stats));
                Assert.AreEqual(2, pickup.Amount);
                Assert.AreEqual(5, _inventory.GetItemCount(_woodItem.itemId));
                Assert.AreEqual(1, _stats.WoodCount);

                Assert.AreEqual(0, pickup.TryCollect(_inventory, _stats));
                Assert.AreEqual(2, pickup.Amount);
                Assert.AreEqual(1, _stats.WoodCount);
            }
            finally
            {
                Object.DestroyImmediate(pickupObject);
                Object.DestroyImmediate(filler);
            }
        }

        [Test]
        public void ProjectItemAssets_HaveStableUniqueNonEmptyIdsAndValidStacks()
        {
            string[] guids = AssetDatabase.FindAssets("t:ItemDataSO", new[] { "Assets/_Project/Data/Items" });
            var ids = new HashSet<string>();
            Assert.GreaterOrEqual(guids.Length, 7, "Thiếu asset item M5.");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var item = AssetDatabase.LoadAssetAtPath<ItemDataSO>(path);
                Assert.IsNotNull(item, path);
                Assert.IsFalse(string.IsNullOrWhiteSpace(item.itemId), path);
                Assert.Greater(item.maxStackSize, 0, path);
                Assert.IsTrue(ids.Add(item.itemId), $"Item ID trùng: '{item.itemId}' tại {path}");
            }
        }

        [Test]
        public void ProjectRecipes_HaveFourValidUniqueAssets()
        {
            string[] guids = AssetDatabase.FindAssets("t:CraftingRecipeSO", new[] { "Assets/_Project/Data/Recipes" });
            var ids = new HashSet<string>();
            Assert.AreEqual(4, guids.Length, "M5 phải có đúng 4 recipe asset.");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var recipe = AssetDatabase.LoadAssetAtPath<CraftingRecipeSO>(path);
                Assert.IsNotNull(recipe, path);
                Assert.IsTrue(ids.Add(recipe.recipeId), $"Recipe ID trùng: '{recipe.recipeId}' tại {path}");
                Assert.IsNotNull(recipe.resultItem, path);
                Assert.Greater(recipe.resultAmount, 0, path);
                Assert.IsNotNull(recipe.ingredients, path);
                Assert.IsNotEmpty(recipe.ingredients, path);
                foreach (RecipeIngredient ingredient in recipe.ingredients)
                {
                    Assert.IsNotNull(ingredient.item, path);
                    Assert.Greater(ingredient.amount, 0, path);
                }
            }
        }

        [Test]
        public void SwordItemAsset_HasCombatConfiguration()
        {
            var sword = AssetDatabase.LoadAssetAtPath<ItemDataSO>("Assets/_Project/Data/Items/Item_weapon_sword_iron_t1.asset");
            Assert.IsNotNull(sword);
            Assert.AreEqual(ItemType.Weapon, sword.itemType);
            Assert.IsNotNull(sword.weaponData);
            Assert.AreEqual(sword.itemId, sword.weaponData.weaponId);
        }

        [Test]
        public void ItemDatabase_FirstAccess_InitializesWithoutRecursionAndReturnsFourRecipes()
        {
            Assert.IsNotNull(ItemDatabase.GetItem("resource_wood"));
            Assert.AreEqual(4, ItemDatabase.GetDefaultRecipes().Count);
        }

        private static ItemDataSO CreateItem(string id, ItemType type, int maxStack)
        {
            var item = ScriptableObject.CreateInstance<ItemDataSO>();
            item.itemId = id;
            item.itemName = id;
            item.itemType = type;
            item.maxStackSize = maxStack;
            return item;
        }
    }
}

