using System;
using System.Collections.Generic;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Inventory
{
    /// <summary>
    /// Hệ thống Chế Tạo của người chơi (Crafting System).
    /// Kiểm tra nguyên liệu trong túi đồ và sản xuất thành phẩm.
    /// </summary>
    public class CraftingSystem : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Hệ thống túi đồ của người chơi")]
        [SerializeField] private InventorySystem inventory;

        [Header("Recipe Database")]
        [Tooltip("Danh sách các công thức đã mở khóa hoặc có sẵn")]
        [SerializeField] private List<CraftingRecipeSO> availableRecipes = new List<CraftingRecipeSO>();

        public IReadOnlyList<CraftingRecipeSO> AvailableRecipes => availableRecipes;

        // Events
        public event Action<CraftingRecipeSO> OnItemCrafted;
        public event Action OnRecipesChanged;

        private void Awake()
        {
            if (inventory == null)
            {
                inventory = GetComponent<InventorySystem>();
            }
        }

        private void Start()
        {
            if (inventory == null)
            {
                inventory = GetComponent<InventorySystem>();
            }

            // Tự động nạp 4 công thức sinh tồn mặc định nếu danh sách đang trống
            if (availableRecipes == null || availableRecipes.Count == 0)
            {
                SetRecipes(ItemDatabase.GetDefaultRecipes());
            }
        }

        public void SetInventory(InventorySystem inv)
        {
            inventory = inv;
        }

        public void SetRecipes(List<CraftingRecipeSO> recipes)
        {
            availableRecipes = recipes ?? new List<CraftingRecipeSO>();
            OnRecipesChanged?.Invoke();
        }

        public void AddRecipe(CraftingRecipeSO recipe)
        {
            if (recipe != null && !availableRecipes.Contains(recipe))
            {
                availableRecipes.Add(recipe);
                OnRecipesChanged?.Invoke();
            }
        }

        /// <summary>
        /// Kiểm tra xem người chơi có đủ tất cả nguyên liệu trong túi để chế tạo không.
        /// </summary>
        public bool CanCraft(CraftingRecipeSO recipe)
        {
            return TryBuildRemovalMap(recipe, out Dictionary<string, int> removals)
                && inventory.CanApplyTransaction(removals, recipe.resultItem, recipe.resultAmount);
        }

        /// <summary>
        /// Thực hiện chế tạo: trừ nguyên liệu và nạp thành phẩm vào túi đồ.
        /// </summary>
        public bool TryCraft(CraftingRecipeSO recipe)
        {
            if (!TryBuildRemovalMap(recipe, out Dictionary<string, int> removals)) return false;
            if (!inventory.TryApplyTransaction(removals, recipe.resultItem, recipe.resultAmount)) return false;

            OnItemCrafted?.Invoke(recipe);
            return true;
        }

        private bool TryBuildRemovalMap(CraftingRecipeSO recipe, out Dictionary<string, int> removals)
        {
            removals = null;
            if (recipe == null || inventory == null || recipe.resultItem == null || recipe.resultAmount <= 0) return false;
            if (string.IsNullOrWhiteSpace(recipe.resultItem.itemId) || recipe.resultItem.maxStackSize <= 0) return false;
            if (recipe.ingredients == null || recipe.ingredients.Count == 0) return false;

            removals = new Dictionary<string, int>(StringComparer.Ordinal);
            for (int i = 0; i < recipe.ingredients.Count; i++)
            {
                RecipeIngredient ingredient = recipe.ingredients[i];
                if (ingredient == null || ingredient.item == null || ingredient.amount <= 0) return false;
                if (string.IsNullOrWhiteSpace(ingredient.item.itemId) || ingredient.item.maxStackSize <= 0) return false;

                if (removals.TryGetValue(ingredient.item.itemId, out int current))
                {
                    if (current > int.MaxValue - ingredient.amount) return false;
                    removals[ingredient.item.itemId] = current + ingredient.amount;
                }
                else
                {
                    removals.Add(ingredient.item.itemId, ingredient.amount);
                }
            }

            return true;
        }
    }
}

