using System;
using System.Collections.Generic;
using HuyetNguyetDaiNgan.Combat;
using HuyetNguyetDaiNgan.Player;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Inventory
{
    /// <summary>
    /// Hệ thống Quản Lý Túi Đồ của người chơi:
    /// - 6 Ô Hotbar bên ngoài (Index 0 đến 5, gán phím số 1 đến 6).
    /// - 20 Ô Ba Lô bên trong (Index 6 đến 25).
    /// Xử lý lưu trữ, cộng dồn, hoán đổi 2 chiều và sử dụng trực tiếp.
    /// </summary>
    public class InventorySystem : MonoBehaviour
    {
        public const int HotbarSlots = 6;
        public const int BagSlots = 20;

        [Header("Inventory Settings")]
        [Tooltip("Tổng số ô (6 Hotbar + 20 Ba Lô = 26 ô)")]
        [SerializeField] private int totalSlots = 26;

        [Tooltip("Danh sách các ô trong túi đồ")]
        [SerializeField] private InventorySlot[] slots;

        // Dependencies
        private HealthComponent _healthComponent;
        private PlayerStatsComponent _statsComponent;
        private PlayerCombatController _combatController;

        // Events
        public event Action OnInventoryChanged;
        public event Action<int, ItemDataSO> OnItemUsed;

        public int TotalSlots => totalSlots;
        public IReadOnlyList<InventorySlot> Slots => slots;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _statsComponent = GetComponent<PlayerStatsComponent>();
            _combatController = GetComponent<PlayerCombatController>();

            InitializeSlots();
        }

        private void Start()
        {
            if (_healthComponent == null) _healthComponent = GetComponent<HealthComponent>() ?? FindFirstObjectByType<HealthComponent>();
            if (_statsComponent == null) _statsComponent = GetComponent<PlayerStatsComponent>() ?? FindFirstObjectByType<PlayerStatsComponent>();
            if (_combatController == null) _combatController = GetComponent<PlayerCombatController>() ?? FindFirstObjectByType<PlayerCombatController>();

            // Luôn đảm bảo 26 ô (6 Hotbar + 20 Ba Lô)
            InitializeSlots();

            // Tự động cấp vật phẩm khởi đầu nếu túi còn trống
            if (IsEmpty())
            {
                var rice = ItemDatabase.GetItem("consumable_bamboo_rice");
                var sword = ItemDatabase.GetItem("weapon_sword_iron_t1");
                var wood = ItemDatabase.GetItem("resource_wood");
                var stone = ItemDatabase.GetItem("resource_stone");
                var berry = ItemDatabase.GetItem("resource_berry");

                // Đặt Cơm Lam vào Hotbar Ô 1 (Index 0), Kiếm vào Hotbar Ô 2 (Index 1), Quả Táo vào Hotbar Ô 3 (Index 2)
                if (rice != null) slots[0].SetItem(rice, 2);
                if (sword != null) slots[1].SetItem(sword, 1);
                if (berry != null) slots[2].SetItem(berry, 5);

                // Đặt nguyên liệu vào Ba lô
                if (wood != null) slots[6].SetItem(wood, 5);
                if (stone != null) slots[7].SetItem(stone, 4);
                if (rice != null) slots[8].SetItem(rice, 2);

                OnInventoryChanged?.Invoke();
            }
        }

        public bool IsEmpty()
        {
            if (slots == null) return true;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && !slots[i].IsEmpty) return false;
            }
            return true;
        }

        public void InitializeSlots()
        {
            totalSlots = 26; // Bắt buộc cố định 26 ô: 6 Hotbar (0..5) + 20 Ba Lô (6..25)
            if (slots == null || slots.Length != totalSlots)
            {
                var oldSlots = slots;
                slots = new InventorySlot[totalSlots];
                for (int i = 0; i < totalSlots; i++)
                {
                    slots[i] = (oldSlots != null && i < oldSlots.Length && oldSlots[i] != null) ? oldSlots[i] : new InventorySlot();
                }
            }
        }

        public InventorySlot GetSlot(int index)
        {
            if (index < 0 || index >= totalSlots) return null;
            return slots[index];
        }

        /// <summary>
        /// Thêm vật phẩm vào túi đồ. Tự động cộng dồn vào các ô cùng loại trước,
        /// sau đó mới tràn sang các ô trống tiếp theo.
        /// </summary>
        public bool TryAddItem(ItemDataSO item, int amount, out int remainder)
        {
            remainder = amount;
            if (item == null || amount <= 0) return false;

            // 1. Cộng dồn vào các ô hiện có cùng loại chưa đầy stack
            for (int i = 0; i < totalSlots; i++)
            {
                if (slots[i].CanStackWith(item))
                {
                    remainder = slots[i].AddQuantity(remainder);
                    if (remainder <= 0)
                    {
                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                }
            }

            // 2. Điền vào các ô trống còn lại
            for (int i = 0; i < totalSlots; i++)
            {
                if (slots[i].IsEmpty)
                {
                    int addable = Mathf.Min(item.maxStackSize, remainder);
                    slots[i].SetItem(item, addable);
                    remainder -= addable;
                    if (remainder <= 0)
                    {
                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                }
            }

            // Nếu còn thừa đồ không nhét vừa
            OnInventoryChanged?.Invoke();
            return remainder < amount;
        }

        public bool TryAddItem(ItemDataSO item, int amount = 1)
        {
            return TryAddItem(item, amount, out _);
        }

        /// <summary>
        /// Đếm tổng số lượng của một loại vật phẩm có trong toàn bộ túi.
        /// </summary>
        public int GetItemCount(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return 0;

            int count = 0;
            for (int i = 0; i < totalSlots; i++)
            {
                if (!slots[i].IsEmpty && slots[i].Item.itemId == itemId)
                {
                    count += slots[i].Quantity;
                }
            }
            return count;
        }

        public bool HasItem(string itemId, int requiredAmount = 1)
        {
            return GetItemCount(itemId) >= requiredAmount;
        }

        /// <summary>
        /// Trừ bớt vật phẩm khỏi túi đồ theo số lượng yêu cầu (Dùng cho Chế Tạo hoặc Tiêu Hao).
        /// </summary>
        public bool TryRemoveItem(string itemId, int amount = 1)
        {
            if (!HasItem(itemId, amount)) return false;

            int remainingToRemove = amount;

            for (int i = totalSlots - 1; i >= 0; i--)
            {
                if (!slots[i].IsEmpty && slots[i].Item.itemId == itemId)
                {
                    int removed = slots[i].RemoveQuantity(remainingToRemove);
                    remainingToRemove -= removed;

                    if (remainingToRemove <= 0)
                    {
                        break;
                    }
                }
            }

            OnInventoryChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Đổi chỗ 2 ô trong túi đồ (Drag & Drop hoặc sắp xếp).
        /// </summary>
        public void SwapSlots(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= totalSlots || toIndex < 0 || toIndex >= totalSlots) return;
            if (fromIndex == toIndex) return;

            var fromSlot = slots[fromIndex];
            var toSlot = slots[toIndex];

            // Nếu cùng loại vật phẩm -> Gộp stack
            if (!fromSlot.IsEmpty && !toSlot.IsEmpty && fromSlot.Item.itemId == toSlot.Item.itemId)
            {
                int remainder = toSlot.AddQuantity(fromSlot.Quantity);
                if (remainder > 0)
                {
                    fromSlot.SetItem(fromSlot.Item, remainder);
                }
                else
                {
                    fromSlot.Clear();
                }
            }
            else
            {
                // Hoán đổi thông thường
                ItemDataSO tempItem = fromSlot.Item;
                int tempQty = fromSlot.Quantity;

                fromSlot.SetItem(toSlot.Item, toSlot.Quantity);
                toSlot.SetItem(tempItem, tempQty);
            }

            OnInventoryChanged?.Invoke();
        }

        /// <summary>
        /// Sử dụng hoặc trang bị vật phẩm tại ô chỉ định.
        /// </summary>
        public void UseItem(int slotIndex)
        {
            InventorySlot slot = GetSlot(slotIndex);
            if (slot == null || slot.IsEmpty) return;

            ItemDataSO item = slot.Item;

            if (_healthComponent == null) _healthComponent = GetComponent<HealthComponent>() ?? FindFirstObjectByType<HealthComponent>();
            if (_statsComponent == null) _statsComponent = GetComponent<PlayerStatsComponent>() ?? FindFirstObjectByType<PlayerStatsComponent>();
            if (_combatController == null) _combatController = GetComponent<PlayerCombatController>() ?? FindFirstObjectByType<PlayerCombatController>();

            switch (item.itemType)
            {
                case ItemType.Consumable:
                    float healAmount = item.healthRestore;
                    float hungerAmount = item.hungerRestore;

                    // Fallback bảo hiểm chỉ số chuẩn
                    if (item.itemId == "consumable_bamboo_rice")
                    {
                        if (healAmount <= 0f) healAmount = 30f;
                        if (hungerAmount <= 0f) hungerAmount = 45f;
                    }
                    else if (item.itemId == "resource_berry")
                    {
                        if (healAmount <= 0f) healAmount = 5f;
                        if (hungerAmount <= 0f) hungerAmount = 15f;
                    }
                    else
                    {
                        if (hungerAmount <= 0f) hungerAmount = 25f;
                    }

                    if (_healthComponent != null && healAmount > 0f)
                    {
                        _healthComponent.Heal(healAmount);
                    }
                    if (_statsComponent != null && hungerAmount > 0f)
                    {
                        _statsComponent.AddHunger(hungerAmount);
                    }
                    if (_statsComponent != null && item.staminaRestore > 0f)
                    {
                        _statsComponent.TryConsumeStamina(-item.staminaRestore);
                    }

                    slot.RemoveQuantity(1);
                    Debug.Log($"[Inventory] Đã dùng '{item.itemName}' tại ô #{slotIndex}. Giảm 1 (còn lại {slot.Quantity}). Hồi phục +{hungerAmount} Độ No, +{healAmount} Máu!");

                    OnInventoryChanged?.Invoke();
                    OnItemUsed?.Invoke(slotIndex, item);
                    break;

                case ItemType.Weapon:
                case ItemType.Tool:
                    // Trang bị vũ khí
                    if (_combatController != null)
                    {
                        var weapon = item.weaponData ?? ItemDatabase.GetItem(item.itemId)?.weaponData;
                        if (weapon != null)
                        {
                            _combatController.EquipWeapon(weapon);
                            OnItemUsed?.Invoke(slotIndex, item);
                        }
                    }
                    break;
            }
        }
    }
}

