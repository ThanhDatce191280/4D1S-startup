using System;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Inventory
{
    /// <summary>
    /// Một ô chứa đồ trong túi (Slot).
    /// Quản lý vật phẩm và số lượng hiện tại, xử lý cộng dồn và tràn stack.
    /// </summary>
    [Serializable]
    public class InventorySlot
    {
        [SerializeField] private ItemDataSO item;
        [SerializeField] private int quantity;

        public ItemDataSO Item => item;
        public int Quantity => quantity;
        public bool IsEmpty => item == null || quantity <= 0;

        public InventorySlot()
        {
            item = null;
            quantity = 0;
        }

        public InventorySlot(ItemDataSO newItem, int initialQuantity)
        {
            item = newItem;
            quantity = Mathf.Max(0, initialQuantity);
        }

        /// <summary>
        /// Kiểm tra xem ô này có thể cộng dồn thêm loại vật phẩm này không.
        /// </summary>
        public bool CanStackWith(ItemDataSO newItem)
        {
            if (IsEmpty || newItem == null) return false;
            return item.itemId == newItem.itemId && quantity < item.maxStackSize;
        }

        /// <summary>
        /// Thêm số lượng vào ô hiện tại. Trả về số lượng dư thừa không nhét vừa stack.
        /// </summary>
        public int AddQuantity(int amount)
        {
            if (item == null || amount <= 0) return amount;

            int spaceLeft = item.maxStackSize - quantity;
            int addable = Mathf.Min(spaceLeft, amount);

            quantity += addable;
            return amount - addable; // Số lượng còn thừa
        }

        /// <summary>
        /// Trừ bớt số lượng trong ô. Trả về số lượng thực tế đã trừ thành công.
        /// </summary>
        public int RemoveQuantity(int amount)
        {
            if (IsEmpty || amount <= 0) return 0;

            int removable = Mathf.Min(quantity, amount);
            quantity -= removable;

            if (quantity <= 0)
            {
                Clear();
            }

            return removable;
        }

        public void SetItem(ItemDataSO newItem, int newQuantity)
        {
            item = newItem;
            quantity = Mathf.Max(0, newQuantity);

            if (item == null)
            {
                Clear();
            }
        }

        public void Clear()
        {
            item = null;
            quantity = 0;
        }
    }
}

