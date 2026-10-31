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
            if (recipe == null || inventory == null || recipe.resultItem == null) return false;

            for (int i = 0; i < recipe.ingredients.Count; i++)
            {
                var ingredient = recipe.ingredients[i];
                if (ingredient.item == null) continue;

                if (!inventory.HasItem(ingredient.item.itemId, ingredient.amount))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Thực hiện chế tạo: trừ nguyên liệu và nạp thành phẩm vào túi đồ.
        /// </summary>
        public bool TryCraft(CraftingRecipeSO recipe)
        {
            if (!CanCraft(recipe)) return false;

            // 1. Trừ toàn bộ nguyên liệu
            for (int i = 0; i < recipe.ingredients.Count; i++)
            {
                var ingredient = recipe.ingredients[i];
                if (ingredient.item == null) continue;

                inventory.TryRemoveItem(ingredient.item.itemId, ingredient.amount);
            }

            // 2. Thêm thành phẩm vào túi
            inventory.TryAddItem(recipe.resultItem, recipe.resultAmount, out int remainder);

            OnItemCrafted?.Invoke(recipe);
            return true;
        }
    }
}

