using System;
using System.Collections.Generic;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Inventory
{
    [Serializable]
    public class RecipeIngredient
    {
        [Tooltip("Loại nguyên liệu yêu cầu")]
        public ItemDataSO item;

        [Tooltip("Số lượng nguyên liệu cần thiết")]
        [Range(1, 99)]
        public int amount = 1;
    }

    /// <summary>
    /// File định nghĩa công thức chế tạo (ScriptableObject).
    /// </summary>
    [CreateAssetMenu(fileName = "Recipe_NewRecipe", menuName = "Huyết Nguyệt/Inventory/Crafting Recipe")]
    public class CraftingRecipeSO : ScriptableObject
    {
        [Header("Thông Tin Công Thức")]
        [Tooltip("Mã định danh công thức (VD: recipe_stone_axe)")]
        public string recipeId = "recipe_new";

        [Tooltip("Tên thành phẩm tiếng Việt hiển thị trên menu chế tạo")]
        public string recipeName = "Công Thức Mới";

        [Tooltip("Mô tả công thức hoặc hướng dẫn sinh tồn")]
        [TextArea(2, 3)]
        public string description = "Mô tả công thức...";

        [Header("Nguyên Liệu Yêu Cầu")]
        [Tooltip("Danh sách các nguyên liệu cần có trong túi")]
        public List<RecipeIngredient> ingredients = new List<RecipeIngredient>();

        [Header("Thành Phẩm Đầu Ra")]
        [Tooltip("Vật phẩm nhận được sau khi chế tạo")]
        public ItemDataSO resultItem;

        [Tooltip("Số lượng thành phẩm nhận được")]
        [Range(1, 99)]
        public int resultAmount = 1;
    }
}

