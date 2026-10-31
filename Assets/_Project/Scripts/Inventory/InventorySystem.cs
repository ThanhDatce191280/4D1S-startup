using System;
using System.Collections.Generic;
using HuyetNguyetDaiNgan.Combat;
using HuyetNguyetDaiNgan.Player;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Inventory
{
    /// <summary>
    /// Hệ thống Quản Lý Túi Đồ của người chơi:
    /// - Tổng cộng đúng 20 ô theo phạm vi M5.
    /// - 6 ô đầu (Index 0 đến 5) đồng thời là ô dùng nhanh, không phải 6 ô chứa thêm.
    /// Xử lý lưu trữ, cộng dồn, hoán đổi 2 chiều và sử dụng trực tiếp.
    /// </summary>
    public class InventorySystem : MonoBehaviour
    {
        public const int HotbarSlots = 6;
        public const int BagSlots = 20;

        [Header("Inventory Settings")]
        [Tooltip("Tổng số ô túi đồ M5 (6 ô đầu là hotbar)")]
        [SerializeField] private int totalSlots = BagSlots;

        [Tooltip("Danh sách các ô trong túi đồ")]
        [SerializeField] private InventorySlot[] slots;

        // Dependencies
        private HealthComponent _healthComponent;
        private PlayerStatsComponent _statsComponent;
        private PlayerCombatController _combatController;
        private string _equippedItemId;

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

            // Luôn đảm bảo đúng 20 ô. Hotbar chỉ tham chiếu 6 ô đầu.
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

                // Đặt nguyên liệu vào các ô còn lại của túi
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
            totalSlots = BagSlots;
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
            if (!IsValidItem(item) || amount <= 0) return false;

            InitializeSlots();

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

            InitializeSlots();

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
            if (string.IsNullOrWhiteSpace(itemId) || amount <= 0) return false;
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
            EnsureEquippedItemStillOwned();
            return true;
        }

        /// <summary>
        /// Tách đúng số lượng từ một stack sang một ô trống. Không thay đổi gì nếu thao tác không hợp lệ.
        /// </summary>
        public bool TrySplitStack(int fromIndex, int toIndex, int amount)
        {
            InventorySlot fromSlot = GetSlot(fromIndex);
            InventorySlot toSlot = GetSlot(toIndex);
            if (fromSlot == null || toSlot == null || fromIndex == toIndex || amount <= 0) return false;
            if (fromSlot.IsEmpty || !toSlot.IsEmpty || amount >= fromSlot.Quantity) return false;

            ItemDataSO item = fromSlot.Item;
            if (!IsValidItem(item) || amount > item.maxStackSize) return false;

            fromSlot.RemoveQuantity(amount);
            toSlot.SetItem(item, amount);
            OnInventoryChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Kiểm tra một giao dịch trừ nhiều item và thêm thành phẩm trên bản sao túi đồ.
        /// Dùng cùng thuật toán với commit để tránh trường hợp kiểm tra và thực thi lệch nhau.
        /// </summary>
        public bool CanApplyTransaction(IReadOnlyDictionary<string, int> removals, ItemDataSO resultItem, int resultAmount)
        {
            return TrySimulateTransaction(removals, resultItem, resultAmount, out _, out _);
        }

        /// <summary>
        /// Commit giao dịch inventory nguyên tử. Nếu không chứa được thành phẩm hoặc dữ liệu sai,
        /// toàn bộ túi giữ nguyên và không phát sự kiện thay đổi.
        /// </summary>
        public bool TryApplyTransaction(IReadOnlyDictionary<string, int> removals, ItemDataSO resultItem, int resultAmount)
        {
            if (!TrySimulateTransaction(removals, resultItem, resultAmount, out ItemDataSO[] simulatedItems, out int[] simulatedQuantities))
            {
                return false;
            }

            for (int i = 0; i < totalSlots; i++)
            {
                slots[i].SetItem(simulatedItems[i], simulatedQuantities[i]);
            }

            EnsureEquippedItemStillOwned();
            OnInventoryChanged?.Invoke();
            return true;
        }

        private bool TrySimulateTransaction(
            IReadOnlyDictionary<string, int> removals,
            ItemDataSO resultItem,
            int resultAmount,
            out ItemDataSO[] simulatedItems,
            out int[] simulatedQuantities)
        {
            simulatedItems = null;
            simulatedQuantities = null;
            if (removals == null || removals.Count == 0 || !IsValidItem(resultItem) || resultAmount <= 0) return false;

            InitializeSlots();
            simulatedItems = new ItemDataSO[totalSlots];
            simulatedQuantities = new int[totalSlots];
            for (int i = 0; i < totalSlots; i++)
            {
                simulatedItems[i] = slots[i].Item;
                simulatedQuantities[i] = slots[i].Quantity;
            }

            foreach (KeyValuePair<string, int> removal in removals)
            {
                if (string.IsNullOrWhiteSpace(removal.Key) || removal.Value <= 0) return false;

                int remaining = removal.Value;
                for (int i = totalSlots - 1; i >= 0 && remaining > 0; i--)
                {
                    if (simulatedItems[i] == null || simulatedQuantities[i] <= 0 || simulatedItems[i].itemId != removal.Key) continue;

                    int removed = Mathf.Min(simulatedQuantities[i], remaining);
                    simulatedQuantities[i] -= removed;
                    remaining -= removed;
                    if (simulatedQuantities[i] == 0)
                    {
                        simulatedItems[i] = null;
                    }
                }

                if (remaining > 0) return false;
            }

            int resultRemaining = resultAmount;
            for (int i = 0; i < totalSlots && resultRemaining > 0; i++)
            {
                ItemDataSO existing = simulatedItems[i];
                if (existing == null || simulatedQuantities[i] <= 0 || existing.itemId != resultItem.itemId) continue;
                if (!AreCompatibleItemDefinitions(existing, resultItem)) return false;

                int space = Mathf.Max(0, existing.maxStackSize - simulatedQuantities[i]);
                int added = Mathf.Min(space, resultRemaining);
                simulatedQuantities[i] += added;
                resultRemaining -= added;
            }

            for (int i = 0; i < totalSlots && resultRemaining > 0; i++)
            {
                if (simulatedItems[i] != null && simulatedQuantities[i] > 0) continue;

                int added = Mathf.Min(resultItem.maxStackSize, resultRemaining);
                simulatedItems[i] = resultItem;
                simulatedQuantities[i] = added;
                resultRemaining -= added;
            }

            return resultRemaining == 0;
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
        public bool UseItem(int slotIndex)
        {
            InventorySlot slot = GetSlot(slotIndex);
            if (slot == null || slot.IsEmpty || !IsValidItem(slot.Item)) return false;

            ItemDataSO item = slot.Item;

            if (_healthComponent == null) _healthComponent = GetComponent<HealthComponent>() ?? FindFirstObjectByType<HealthComponent>();
            if (_statsComponent == null) _statsComponent = GetComponent<PlayerStatsComponent>() ?? FindFirstObjectByType<PlayerStatsComponent>();
            if (_combatController == null) _combatController = GetComponent<PlayerCombatController>() ?? FindFirstObjectByType<PlayerCombatController>();

            switch (item.itemType)
            {
                case ItemType.Consumable:
                    float healAmount = item.healthRestore;
                    float hungerAmount = item.hungerRestore;

                    bool hasConfiguredEffect = healAmount > 0f || hungerAmount > 0f || item.staminaRestore > 0f || item.manaRestore > 0f;
                    bool hasRequiredTarget = (healAmount <= 0f || _healthComponent != null)
                        && ((hungerAmount <= 0f && item.staminaRestore <= 0f && item.manaRestore <= 0f) || _statsComponent != null);
                    if (!hasConfiguredEffect || !hasRequiredTarget || (_healthComponent != null && _healthComponent.IsDead))
                    {
                        return false;
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
                        _statsComponent.RestoreStamina(item.staminaRestore);
                    }
                    if (_statsComponent != null && item.manaRestore > 0f)
                    {
                        _statsComponent.RestoreMana(item.manaRestore);
                    }

                    slot.RemoveQuantity(1);
                    Debug.Log($"[Inventory] Đã dùng '{item.itemName}' tại ô #{slotIndex}. Giảm 1 (còn lại {slot.Quantity}). Hồi phục +{hungerAmount} Độ No, +{healAmount} Máu!");

                    OnInventoryChanged?.Invoke();
                    OnItemUsed?.Invoke(slotIndex, item);
                    return true;

                case ItemType.Weapon:
                case ItemType.Tool:
                    // Trang bị vũ khí
                    if (_combatController != null)
                    {
                        var weapon = item.weaponData ?? ItemDatabase.GetItem(item.itemId)?.weaponData;
                        if (weapon != null)
                        {
                            _combatController.EquipWeapon(weapon);
                            _equippedItemId = item.itemId;
                            OnItemUsed?.Invoke(slotIndex, item);
                            return true;
                        }
                    }
                    return false;
            }

            return false;
        }

        private static bool IsValidItem(ItemDataSO item)
        {
            return item != null && !string.IsNullOrWhiteSpace(item.itemId) && item.maxStackSize > 0;
        }

        private static bool AreCompatibleItemDefinitions(ItemDataSO left, ItemDataSO right)
        {
            return left != null
                && right != null
                && left.itemId == right.itemId
                && left.itemType == right.itemType
                && left.maxStackSize == right.maxStackSize;
        }

        private void EnsureEquippedItemStillOwned()
        {
            if (string.IsNullOrEmpty(_equippedItemId) || HasItem(_equippedItemId)) return;

            if (_combatController != null)
            {
                _combatController.EquipWeapon(null);
            }
            _equippedItemId = null;
        }
    }
}

