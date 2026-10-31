using HuyetNguyetDaiNgan.Combat;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Inventory
{
    public enum ItemType
    {
        Resource,    // Tài nguyên thô (Gỗ, Đá, Quả dại, v.v.)
        Consumable,  // Tiêu hao (Cơm Lam, Thuốc hồi máu, Nước)
        Weapon,      // Binh khí cận chiến / tầm xa
        Tool,        // Công cụ khai thác (Rìu, Cuốc, Đuốc)
        Armor        // Giáp bảo hộ
    }

    /// <summary>
    /// File định nghĩa thông tin vật phẩm trong toàn bộ hệ thống (ScriptableObject).
    /// </summary>
    [CreateAssetMenu(fileName = "Item_NewItem", menuName = "Huyết Nguyệt/Inventory/Item Data")]
    public class ItemDataSO : ScriptableObject
    {
        [Header("Định Danh & Phân Loại")]
        [Tooltip("Mã định danh duy nhất (VD: resource_wood, consumable_bamboo_rice)")]
        public string itemId = "item_new";

        [Tooltip("Tên vật phẩm tiếng Việt hiển thị trên giao diện")]
        public string itemName = "Vật Phẩm Mới";

        [Tooltip("Mô tả công dụng và cốt truyện dân gian")]
        [TextArea(2, 4)]
        public string description = "Mô tả vật phẩm...";

        [Tooltip("Phân loại vật phẩm")]
        public ItemType itemType = ItemType.Resource;

        [Tooltip("Số lượng cộng dồn tối đa trên 1 ô túi đồ")]
        [Range(1, 999)]
        public int maxStackSize = 99;

        [Header("Hình Ảnh & Đại Diện")]
        [Tooltip("Sprite icon hiển thị trong túi đồ")]
        public Sprite icon;

        [Tooltip("Màu đại diện placeholder khi chưa có hình vẽ 2D")]
        public Color placeholderColor = Color.white;

        [Tooltip("Ký tự biểu tượng tóm tắt (VD: 🪵, 🪨, 🍎, ⚔, 🪓)")]
        public string iconGlyph = "📦";

        [Header("Chỉ Số Tiêu Hao (Áp dụng nếu là Consumable)")]
        [Tooltip("Lượng máu hồi phục khi sử dụng")]
        public float healthRestore = 0f;

        [Tooltip("Lượng thức ăn (Độ No) hồi phục khi sử dụng")]
        public float hungerRestore = 0f;

        [Tooltip("Lượng thể lực hồi phục khi sử dụng")]
        public float staminaRestore = 0f;

        [Tooltip("Lượng năng lượng (Mana) hồi phục khi sử dụng")]
        public float manaRestore = 0f;

        [Header("Dữ Liệu Chiến Đấu (Áp dụng nếu là Weapon / Tool)")]
        [Tooltip("Cấu hình vũ khí chiến đấu gắn kèm")]
        public WeaponDataSO weaponData;
    }
}

