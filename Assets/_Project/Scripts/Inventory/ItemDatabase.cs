using System.Collections.Generic;
using HuyetNguyetDaiNgan.Combat;
using HuyetNguyetDaiNgan.Player;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Inventory
{
    /// <summary>
    /// Kho dữ liệu vật phẩm và công thức toàn cục (Runtime Database & Fallback Generator).
    /// Đảm bảo game luôn có đủ vật phẩm và công thức sinh tồn ở mọi scene, kể cả khi chưa kéo thả trong Inspector.
    /// </summary>
    public static class ItemDatabase
    {
        private static readonly Dictionary<string, ItemDataSO> ItemCache = new Dictionary<string, ItemDataSO>();
        private static readonly List<CraftingRecipeSO> RecipeCache = new List<CraftingRecipeSO>();
        private static bool _initialized = false;

        public static void EnsureInitialized()
        {
            if (_initialized) return;

            // Load tất cả ItemDataSO và CraftingRecipeSO trong Resources nếu có
            ItemDataSO[] loadedItems = Resources.LoadAll<ItemDataSO>("");
            foreach (var it in loadedItems)
            {
                if (it != null && !ItemCache.ContainsKey(it.itemId))
                {
                    ItemCache[it.itemId] = it;
                }
            }

            // Tạo các vật phẩm cốt lõi mặc định nếu chưa có
            GetOrCreateDefaultItems();

            // Tạo các công thức chế tạo cốt lõi
            GetOrCreateDefaultRecipes();

            _initialized = true;
        }

        public static ItemDataSO GetItem(string itemId)
        {
            EnsureInitialized();
            if (ItemCache.TryGetValue(itemId, out var item))
            {
                return item;
            }
            return null;
        }

        public static ItemDataSO GetItemByResourceType(ResourceType type)
        {
            EnsureInitialized();
            switch (type)
            {
                case ResourceType.Wood:
                    return GetItem("resource_wood");
                case ResourceType.Stone:
                    return GetItem("resource_stone");
                case ResourceType.Food:
                default:
                    return GetItem("resource_berry");
            }
        }

        public static List<CraftingRecipeSO> GetDefaultRecipes()
        {
            EnsureInitialized();
            return new List<CraftingRecipeSO>(RecipeCache);
        }

        private static void GetOrCreateDefaultItems()
        {
            // 1. Gỗ Rừng
            if (!ItemCache.ContainsKey("resource_wood"))
            {
                var wood = ScriptableObject.CreateInstance<ItemDataSO>();
                wood.itemId = "resource_wood";
                wood.itemName = "Gỗ Khúc Rừng Già";
                wood.description = "Gỗ đốn từ thân cây cổ thụ, nguyên liệu thô để nhóm lửa và chế tác công cụ.";
                wood.itemType = ItemType.Resource;
                wood.maxStackSize = 99;
                wood.iconGlyph = "🪵";
                ItemCache["resource_wood"] = wood;
            }

            // 2. Đá Cuội
            if (!ItemCache.ContainsKey("resource_stone"))
            {
                var stone = ScriptableObject.CreateInstance<ItemDataSO>();
                stone.itemId = "resource_stone";
                stone.itemName = "Đá Cuội Sông Suối";
                stone.description = "Tảng đá cứng cáp nhặt từ lòng suối, dùng ghè đẽo thành rìu cuốc.";
                stone.itemType = ItemType.Resource;
                stone.maxStackSize = 99;
                stone.iconGlyph = "🪨";
                ItemCache["resource_stone"] = stone;
            }

            // 3. Quả Mọng
            if (!ItemCache.ContainsKey("resource_berry"))
            {
                var berry = ScriptableObject.CreateInstance<ItemDataSO>();
                berry.itemId = "resource_berry";
                berry.itemName = "Quả Mọng Rừng Sâu";
                berry.description = "Quả dại hái từ bụi rậm, ăn vào giúp hồi phục 15 Độ No và 5 Máu.";
                berry.itemType = ItemType.Consumable;
                berry.maxStackSize = 50;
                berry.iconGlyph = "🍎";
                berry.hungerRestore = 15f;
                berry.healthRestore = 5f;
                ItemCache["resource_berry"] = berry;
            }

            // 4. Cơm Lam Ống Nứa
            if (!ItemCache.ContainsKey("consumable_bamboo_rice"))
            {
                var rice = ScriptableObject.CreateInstance<ItemDataSO>();
                rice.itemId = "consumable_bamboo_rice";
                rice.itemName = "Cơm Lam Ống Nứa";
                rice.description = "Món cơm lam nướng than hồng thơm lừng, ăn vào hồi phục 45 Độ No và 30 Máu.";
                rice.itemType = ItemType.Consumable;
                rice.maxStackSize = 20;
                rice.iconGlyph = "🍙";
                rice.hungerRestore = 45f;
                rice.healthRestore = 30f;
                ItemCache["consumable_bamboo_rice"] = rice;
            }

            // 5. Rìu Đá Thô Sơ
            if (!ItemCache.ContainsKey("tool_stone_axe_t1"))
            {
                var axe = ScriptableObject.CreateInstance<ItemDataSO>();
                axe.itemId = "tool_stone_axe_t1";
                axe.itemName = "Rìu Đá Thô Sơ";
                axe.description = "Lưỡi đá ghè buộc chặt vào cán gỗ, chặt cây nhanh gấp đôi tay không.";
                axe.itemType = ItemType.Tool;
                axe.maxStackSize = 1;
                axe.iconGlyph = "🪓";
                ItemCache["tool_stone_axe_t1"] = axe;
            }

            // 6. Đuốc Sáng Đi Rừng
            if (!ItemCache.ContainsKey("tool_torch_t1"))
            {
                var torch = ScriptableObject.CreateInstance<ItemDataSO>();
                torch.itemId = "tool_torch_t1";
                torch.itemName = "Đuốc Sáng Đi Rừng";
                torch.description = "Đuốc tẩm nhựa cây thắp sáng màn đêm và xua đuổi thú dữ ban đêm.";
                torch.itemType = ItemType.Tool;
                torch.maxStackSize = 1;
                torch.iconGlyph = "🔥";
                ItemCache["tool_torch_t1"] = torch;
            }

            // 7. Trường Kiếm Sắt Ấp Làng
            if (!ItemCache.ContainsKey("weapon_sword_iron_t1"))
            {
                var sword = ScriptableObject.CreateInstance<ItemDataSO>();
                sword.itemId = "weapon_sword_iron_t1";
                sword.itemName = "Trường Kiếm Sắt Ấp Làng";
                sword.description = "Vũ khí rèn từ sắt non, sát thương 25 đòn đánh quét hình quạt.";
                sword.itemType = ItemType.Weapon;
                sword.maxStackSize = 1;
                sword.iconGlyph = "⚔";

                var wData = ScriptableObject.CreateInstance<WeaponDataSO>();
                wData.weaponId = "weapon_sword_iron_t1";
                wData.weaponName = "Trường Kiếm Sắt Ấp Làng";
                wData.category = WeaponCategory.Sword;
                wData.damageType = DamageType.Physical;
                wData.baseDamage = 25f;
                wData.attackRange = 2.5f;
                wData.attackAngle = 110f;
                wData.attackCooldown = 0.45f;
                sword.weaponData = wData;

                ItemCache["weapon_sword_iron_t1"] = sword;
            }
        }

        private static void GetOrCreateDefaultRecipes()
        {
            RecipeCache.Clear();

            var wood = GetItem("resource_wood");
            var stone = GetItem("resource_stone");
            var berry = GetItem("resource_berry");
            var rice = GetItem("consumable_bamboo_rice");
            var axe = GetItem("tool_stone_axe_t1");
            var torch = GetItem("tool_torch_t1");
            var sword = GetItem("weapon_sword_iron_t1");

            // 1. Rìu Đá (3 Gỗ + 2 Đá)
            var rAxe = ScriptableObject.CreateInstance<CraftingRecipeSO>();
            rAxe.recipeId = "recipe_stone_axe";
            rAxe.recipeName = "Rìu Đá Thô Sơ";
            rAxe.description = "Ghè lưỡi đá buộc cán gỗ để tăng tốc độ khai thác cây rừng.";
            rAxe.resultItem = axe;
            rAxe.resultAmount = 1;
            rAxe.ingredients = new List<RecipeIngredient>
            {
                new RecipeIngredient { item = wood, amount = 3 },
                new RecipeIngredient { item = stone, amount = 2 }
            };
            RecipeCache.Add(rAxe);

            // 2. Đuốc Sáng (2 Gỗ + 1 Quả)
            var rTorch = ScriptableObject.CreateInstance<CraftingRecipeSO>();
            rTorch.recipeId = "recipe_torch";
            rTorch.recipeName = "Đuốc Sáng Đi Rừng";
            rTorch.description = "Bó cành gỗ bọc nhựa thắp sáng đường mòn ban đêm.";
            rTorch.resultItem = torch;
            rTorch.resultAmount = 1;
            rTorch.ingredients = new List<RecipeIngredient>
            {
                new RecipeIngredient { item = wood, amount = 2 },
                new RecipeIngredient { item = berry, amount = 1 }
            };
            RecipeCache.Add(rTorch);

            // 3. Cơm Lam (2 Gỗ + 3 Quả Mọng)
            var rRice = ScriptableObject.CreateInstance<CraftingRecipeSO>();
            rRice.recipeId = "recipe_bamboo_rice";
            rRice.recipeName = "Cơm Lam Ống Nứa";
            rRice.description = "Đun nướng thức ăn giúp hồi phục sinh lực và độ no dồi dào.";
            rRice.resultItem = rice;
            rRice.resultAmount = 1;
            rRice.ingredients = new List<RecipeIngredient>
            {
                new RecipeIngredient { item = wood, amount = 2 },
                new RecipeIngredient { item = berry, amount = 3 }
            };
            RecipeCache.Add(rRice);

            // 4. Trường Kiếm Sắt (5 Gỗ + 5 Đá)
            var rSword = ScriptableObject.CreateInstance<CraftingRecipeSO>();
            rSword.recipeId = "recipe_iron_sword";
            rSword.recipeName = "Trường Kiếm Sắt";
            rSword.description = "Rèn luyện vũ khí tự vệ trước bầy sói khi đêm Huyết Nguyệt buông xuống.";
            rSword.resultItem = sword;
            rSword.resultAmount = 1;
            rSword.ingredients = new List<RecipeIngredient>
            {
                new RecipeIngredient { item = wood, amount = 5 },
                new RecipeIngredient { item = stone, amount = 5 }
            };
            RecipeCache.Add(rSword);
        }
    }
}

