using System.Collections.Generic;
using HuyetNguyetDaiNgan.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HuyetNguyetDaiNgan.UI
{
    /// <summary>
    /// Giao diện Túi Đồ & Bàn Chế Tạo Dân Gian:
    /// - 6 Ô Hotbar bên ngoài (Index 0..5, phím tắt 1..6, click dùng trực tiếp ngoài màn hình).
    /// - 20 Ô Ba Lô bên trong (Index 6..25).
    /// - Chọn đồ và xem chi tiết an toàn, không bị nhầm lẫn với thao tác dời đồ.
    /// - Chức năng [⇄ CHUYỂN Ô] chuyển đồ 2 chiều giữa Ba Lô và 6 ô Hotbar.
    /// - Dùng đồ: Nút [SỬ DỤNG / ĂN NGAY], hoặc Click chuột phải, hoặc Nhấp đúp chuột.
    /// - Ăn đồ ăn nào thì trừ chính xác đồ ăn đó và tăng thanh Độ No.
    /// </summary>
    public class InventoryCraftingUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InventorySystem inventory;
        [SerializeField] private CraftingSystem craftingSystem;

        [Header("State")]
        [SerializeField] private bool isOpen = false;

        // Visual Colors & Themes (Smoked Glass & Gold Fantasy)
        private readonly Color _windowBg = new Color(0.04f, 0.06f, 0.08f, 0.90f);
        private readonly Color _panelInnerBg = new Color(0.02f, 0.03f, 0.05f, 0.65f);
        private readonly Color _borderGold = new Color(0.85f, 0.70f, 0.32f, 0.90f);
        private readonly Color _borderInner = new Color(0.28f, 0.33f, 0.40f, 0.65f);
        private readonly Color _slotBg = new Color(0.02f, 0.03f, 0.05f, 0.78f);
        private readonly Color _slotHotbarBg = new Color(0.04f, 0.06f, 0.10f, 0.85f);
        private readonly Color _slotSelectedBg = new Color(0.22f, 0.42f, 0.62f, 0.90f);
        private readonly Color _slotMovingBg = new Color(0.55f, 0.35f, 0.10f, 0.90f);
        private readonly Color _btnActionBg = new Color(0.18f, 0.50f, 0.75f, 0.95f);
        private readonly Color _btnMoveBg = new Color(0.70f, 0.45f, 0.15f, 0.95f);
        private readonly Color _btnCancelBg = new Color(0.35f, 0.38f, 0.42f, 0.80f);
        private readonly Color _btnCraftActive = new Color(0.18f, 0.62f, 0.32f, 0.95f);
        private readonly Color _btnCraftDisabled = new Color(0.25f, 0.28f, 0.30f, 0.50f);

        // State
        private int _selectedSlotIndex = -1;
        private bool _isMovingMode = false;
        private Texture2D _whiteTex;

        // Styles
        private GUIStyle _titleStyle;
        private GUIStyle _columnHeaderStyle;
        private GUIStyle _slotQuantityStyle;
        private GUIStyle _slotGlyphStyle;
        private GUIStyle _hotkeyBadgeStyle;
        private GUIStyle _hintStyle;
        private GUIStyle _hintMovingStyle;
        private GUIStyle _tooltipTitleStyle;
        private GUIStyle _tooltipDescStyle;
        private GUIStyle _statBonusStyle;
        private GUIStyle _recipeNameStyle;
        private GUIStyle _recipeReqStyle;
        private GUIStyle _btnStyle;
        private GUIStyle _closeBtnStyle;

        public static bool IsOpenGlobal { get; private set; }

        public bool IsOpen
        {
            get => isOpen;
            set
            {
                isOpen = value;
                IsOpenGlobal = value;
                if (!isOpen)
                {
                    _selectedSlotIndex = -1;
                    _isMovingMode = false;
                }
            }
        }

        public static bool IsMouseOverAnyUI()
        {
            if (IsOpenGlobal) return true;

            Mouse mouse = Mouse.current;
            if (mouse == null) return false;

            Vector2 screenPos = mouse.position.ReadValue();
            Vector2 guiPos = new Vector2(screenPos.x, Screen.height - screenPos.y);

            // 1. Nút mở túi đồ ở góc trên bên phải
            Rect toggleBtnRect = new Rect(Screen.width - 130f, 10f, 120f, 40f);
            if (toggleBtnRect.Contains(guiPos)) return true;

            // 2. Bảng HUD sinh tồn ở góc dưới bên phải
            Rect hudRect = new Rect(Screen.width - 260f, Screen.height - 280f, 250f, 200f);
            if (hudRect.Contains(guiPos)) return true;

            // 3. Thanh Hotbar 6 ô ở đáy giữa màn hình
            float hotbarSlotSize = 46f;
            float hotbarGap = 6f;
            int count = InventorySystem.HotbarSlots;
            float hotbarW = count * hotbarSlotSize + (count - 1) * hotbarGap + 20f;
            float hotbarH = hotbarSlotSize + 16f;
            float hotbarX = (Screen.width - hotbarW) * 0.5f;
            float hotbarY = Screen.height - hotbarH - 10f;
            Rect hotbarRect = new Rect(hotbarX - 4f, hotbarY - 4f, hotbarW + 8f, hotbarH + 8f);
            if (hotbarRect.Contains(guiPos)) return true;

            return false;
        }

        private void Awake()
        {
            _whiteTex = new Texture2D(1, 1);
            _whiteTex.SetPixel(0, 0, Color.white);
            _whiteTex.Apply();

            FindDependenciesIfNeeded();
        }

        private void Start()
        {
            InitStyles();
        }

        private void OnDisable()
        {
            IsOpenGlobal = false;
        }

        private void OnDestroy()
        {
            IsOpenGlobal = false;
            if (_whiteTex != null)
            {
                Destroy(_whiteTex);
            }
        }

        private void FindDependenciesIfNeeded()
        {
            if (inventory == null)
            {
                inventory = FindFirstObjectByType<InventorySystem>();
                if (inventory == null)
                {
                    var playerObj = GameObject.FindWithTag("Player");
                    if (playerObj != null)
                    {
                        inventory = playerObj.AddComponent<InventorySystem>();
                        inventory.InitializeSlots();
                    }
                }
            }

            if (craftingSystem == null)
            {
                craftingSystem = FindFirstObjectByType<CraftingSystem>();
                if (craftingSystem == null && inventory != null)
                {
                    craftingSystem = inventory.gameObject.AddComponent<CraftingSystem>();
                    craftingSystem.SetInventory(inventory);
                }
            }
        }

        private void Update()
        {
            Keyboard kb = Keyboard.current;
            if (kb == null) return;

            // 1. Phím I, Tab, B để bật/tắt túi đồ
            if (kb.iKey.wasPressedThisFrame || kb.tabKey.wasPressedThisFrame || kb.bKey.wasPressedThisFrame)
            {
                IsOpen = !IsOpen;
            }

            // 2. Phím ESC để đóng túi đồ
            if (isOpen && kb.escapeKey.wasPressedThisFrame)
            {
                IsOpen = false;
            }

            // 3. Phím số 1..6 để sử dụng nhanh 6 ô Hotbar bên ngoài
            if (kb.digit1Key.wasPressedThisFrame || kb.numpad1Key.wasPressedThisFrame) TriggerHotbarSlot(0);
            else if (kb.digit2Key.wasPressedThisFrame || kb.numpad2Key.wasPressedThisFrame) TriggerHotbarSlot(1);
            else if (kb.digit3Key.wasPressedThisFrame || kb.numpad3Key.wasPressedThisFrame) TriggerHotbarSlot(2);
            else if (kb.digit4Key.wasPressedThisFrame || kb.numpad4Key.wasPressedThisFrame) TriggerHotbarSlot(3);
            else if (kb.digit5Key.wasPressedThisFrame || kb.numpad5Key.wasPressedThisFrame) TriggerHotbarSlot(4);
            else if (kb.digit6Key.wasPressedThisFrame || kb.numpad6Key.wasPressedThisFrame) TriggerHotbarSlot(5);
        }

        private void TriggerHotbarSlot(int hotbarIndex)
        {
            if (inventory != null)
            {
                inventory.UseItem(hotbarIndex);
            }
        }

        private void InitStyles()
        {
            _titleStyle = new GUIStyle
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = new Color(0.96f, 0.88f, 0.68f, 1f) }
            };

            _columnHeaderStyle = new GUIStyle
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = new Color(0.85f, 0.90f, 0.96f, 1f) }
            };

            _slotGlyphStyle = new GUIStyle
            {
                fontSize = 18,
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };

            _slotQuantityStyle = new GUIStyle
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.LowerRight,
                normal = { textColor = Color.white }
            };

            _hotkeyBadgeStyle = new GUIStyle
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperLeft,
                normal = { textColor = new Color(0.95f, 0.80f, 0.40f, 1f) }
            };

            _hintStyle = new GUIStyle
            {
                fontSize = 10,
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = new Color(0.75f, 0.82f, 0.90f, 0.95f) }
            };

            _hintMovingStyle = new GUIStyle
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = new Color(1f, 0.80f, 0.20f, 1f) }
            };

            _tooltipTitleStyle = new GUIStyle
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(1f, 0.90f, 0.60f, 1f) }
            };

            _tooltipDescStyle = new GUIStyle
            {
                fontSize = 10,
                fontStyle = FontStyle.Normal,
                wordWrap = true,
                normal = { textColor = new Color(0.82f, 0.86f, 0.90f, 0.95f) }
            };

            _statBonusStyle = new GUIStyle
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.45f, 0.95f, 0.50f, 1f) }
            };

            _recipeNameStyle = new GUIStyle
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = Color.white }
            };

            _recipeReqStyle = new GUIStyle
            {
                fontSize = 10,
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = new Color(0.80f, 0.85f, 0.90f, 0.95f) }
            };

            _btnStyle = new GUIStyle
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };

            _closeBtnStyle = new GUIStyle
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(0.95f, 0.70f, 0.70f, 1f) }
            };
        }

        private void OnGUI()
        {
            if (_whiteTex == null) return;
            FindDependenciesIfNeeded();

            // 1. Luôn hiển thị Thanh Hotbar 6 ô ngoài màn hình (đáy giữa)
            DrawMainHudHotbar();

            // 2. Nút bấm nhỏ mở túi đồ trực tiếp ở góc trên bên phải màn hình
            float toggleBtnW = 105f;
            float toggleBtnH = 26f;
            float toggleBtnX = Screen.width - toggleBtnW - 20f;
            float toggleBtnY = 20f;

            DrawSolidRect(new Rect(toggleBtnX - 1, toggleBtnY - 1, toggleBtnW + 2, toggleBtnH + 2), _borderGold);
            DrawSolidRect(new Rect(toggleBtnX, toggleBtnY, toggleBtnW, toggleBtnH), isOpen ? _slotSelectedBg : _windowBg);

            string toggleLabel = isOpen ? "✕ Đóng [I]" : "🎒 Túi Đồ [I]";
            if (GUI.Button(new Rect(toggleBtnX, toggleBtnY, toggleBtnW, toggleBtnH), toggleLabel, _btnStyle))
            {
                IsOpen = !IsOpen;
            }

            // 3. Nếu đang mở túi đồ -> Vẽ Cửa Sổ Túi Đồ & Bàn Chế Tạo Dân Gian
            if (!isOpen) return;

            float windowWidth = 640f;
            float windowHeight = 465f;
            float startX = (Screen.width - windowWidth) * 0.5f;
            float startY = (Screen.height - windowHeight) * 0.5f;

            // Nền cửa sổ lớn (Smoked Glass + Viền kép Gold)
            DrawSolidRect(new Rect(startX - 2, startY - 2, windowWidth + 4, windowHeight + 4), _borderGold);
            DrawSolidRect(new Rect(startX - 1, startY - 1, windowWidth + 2, windowHeight + 2), _borderInner);
            DrawSolidRect(new Rect(startX, startY, windowWidth, windowHeight), _windowBg);

            // Tiêu đề cửa sổ & Nút Đóng
            GUI.Label(new Rect(startX + 16, startY + 10, 360, 22), "❖  TÚI ĐỒ & BÀN CHẾ TẠO DÂN GIAN", _titleStyle);

            if (GUI.Button(new Rect(startX + windowWidth - 36, startY + 8, 26, 24), "✕", _closeBtnStyle))
            {
                IsOpen = false;
            }

            // Đường kẻ ngăn cách tiêu đề
            DrawSolidRect(new Rect(startX + 16, startY + 34, windowWidth - 32, 1), _borderInner);

            // Cột Trái: Hotbar (6 ô) + Ba Lô (20 ô) + Chi tiết vật phẩm
            float colLeftX = startX + 16f;
            float colLeftW = 290f;
            DrawInventoryColumn(colLeftX, startY + 42f, colLeftW, windowHeight - 78f);

            // Vách ngăn giữa 2 cột
            DrawSolidRect(new Rect(startX + 316, startY + 42, 1, windowHeight - 82), _borderInner);

            // Cột Phải: Bàn Chế Tạo (Crafting Recipes)
            float colRightX = startX + 328f;
            float colRightW = windowWidth - 344f;
            DrawCraftingColumn(colRightX, startY + 42f, colRightW, windowHeight - 78f);

            // Dòng tiện ích ở đáy cửa sổ
            float bottomBarY = startY + windowHeight - 28f;
            DrawSolidRect(new Rect(startX + 16, bottomBarY - 4, windowWidth - 32, 1), _borderInner);

            Rect testBtnRect = new Rect(startX + 16, bottomBarY, 140, 22);
            DrawSolidRect(testBtnRect, new Color(0.2f, 0.45f, 0.25f, 0.9f));
            if (GUI.Button(testBtnRect, "+ Nhận Thêm Đồ Test", _btnStyle))
            {
                if (inventory != null)
                {
                    inventory.TryAddItem(ItemDatabase.GetItem("consumable_bamboo_rice"), 2);
                    inventory.TryAddItem(ItemDatabase.GetItem("resource_wood"), 5);
                    inventory.TryAddItem(ItemDatabase.GetItem("resource_stone"), 5);
                    inventory.TryAddItem(ItemDatabase.GetItem("resource_berry"), 5);
                }
            }

            GUI.Label(new Rect(startX + 166, bottomBarY + 3, windowWidth - 180, 20), "• Chuột phải hoặc bấm [ĂN/SỬ DỤNG] để dùng | Phím 1..6 dùng nhanh ngoài màn hình", _tooltipDescStyle);
        }

        private void DrawMainHudHotbar()
        {
            float slotSize = 46f;
            float gap = 6f;
            int count = InventorySystem.HotbarSlots; // 6
            float totalW = count * slotSize + (count - 1) * gap + 16f;
            float totalH = slotSize + 14f;
            float startX = (Screen.width - totalW) * 0.5f;
            float startY = Screen.height - totalH - 10f;

            // Khung đế dock
            Rect dockRect = new Rect(startX, startY, totalW, totalH);
            DrawSolidRect(new Rect(dockRect.x - 1, dockRect.y - 1, dockRect.width + 2, dockRect.height + 2), _borderInner);
            DrawSolidRect(dockRect, new Color(0.03f, 0.05f, 0.07f, 0.85f));

            for (int i = 0; i < count; i++)
            {
                float sx = startX + 8f + i * (slotSize + gap);
                float sy = startY + 7f;
                Rect slotRect = new Rect(sx, sy, slotSize, slotSize);

                DrawSolidRect(new Rect(slotRect.x - 1, slotRect.y - 1, slotRect.width + 2, slotRect.height + 2), _borderInner);
                DrawSolidRect(slotRect, _slotHotbarBg);

                // Số phím tắt [1..6] ở góc trên
                GUI.Label(new Rect(slotRect.x + 3, slotRect.y + 1, 14, 14), (i + 1).ToString(), _hotkeyBadgeStyle);

                if (inventory != null)
                {
                    var slot = inventory.GetSlot(i);
                    if (slot != null && !slot.IsEmpty)
                    {
                        string glyph = !string.IsNullOrEmpty(slot.Item.iconGlyph) ? slot.Item.iconGlyph : "📦";
                        GUI.Label(slotRect, glyph, _slotGlyphStyle);

                        if (slot.Quantity > 1)
                        {
                            GUI.Label(new Rect(slotRect.x + 2, slotRect.y + slotSize - 16, slotSize - 6, 14), slot.Quantity.ToString(), _slotQuantityStyle);
                        }
                    }
                }

                // Click trực tiếp vào ô hotbar ngoài màn hình để sử dụng/trang bị
                if (GUI.Button(slotRect, GUIContent.none, GUIStyle.none))
                {
                    TriggerHotbarSlot(i);
                }
            }
        }

        private void DrawInventoryColumn(float x, float y, float width, float height)
        {
            // 1. THANH PHÍM TẮT NGOÀI (6 Ô)
            GUI.Label(new Rect(x, y, width, 16), "⚡ THANH PHÍM TẮT NGOÀI (6 Ô - PHÍM 1..6)", _columnHeaderStyle);

            float hotbarSlotSize = 42f;
            float hotbarGap = 6f;
            float hotbarStartY = y + 19f;

            for (int i = 0; i < InventorySystem.HotbarSlots; i++)
            {
                float sx = x + i * (hotbarSlotSize + hotbarGap);
                DrawSingleSlot(i, sx, hotbarStartY, hotbarSlotSize, (i + 1).ToString());
            }

            // 2. BA LÔ TÚI ĐỒ (20 Ô)
            float bagStartY = hotbarStartY + hotbarSlotSize + 10f;
            GUI.Label(new Rect(x, bagStartY, width, 16), "🎒 BA LÔ TÚI ĐỒ (20 Ô)", _columnHeaderStyle);

            float bagGridY = bagStartY + 19f;
            int cols = 5;
            int rows = 4;
            float bagSlotSize = 50f;
            float bagGap = 8f;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int index = InventorySystem.HotbarSlots + (r * cols + c); // Index 6..25
                    float sx = x + c * (bagSlotSize + bagGap);
                    float sy = bagGridY + r * (bagSlotSize + bagGap);
                    DrawSingleSlot(index, sx, sy, bagSlotSize, null);
                }
            }

            // 3. HƯỚNG DẪN THAO TÁC RÕ RÀNG
            float hintY = bagGridY + rows * (bagSlotSize + bagGap) + 4f;
            string moveHint;
            if (_isMovingMode && _selectedSlotIndex >= 0)
            {
                var slot = inventory?.GetSlot(_selectedSlotIndex);
                string itemName = slot != null && !slot.IsEmpty ? slot.Item.itemName : "Vật phẩm";
                moveHint = $"➜ ĐANG CHUYỂN [{itemName}]: Hãy nhấp ô đích để đặt đồ vào!";
                GUI.Label(new Rect(x, hintY, width, 16), moveHint, _hintMovingStyle);
            }
            else if (_selectedSlotIndex >= 0)
            {
                moveHint = $"➜ Đang chọn [{GetSlotDisplayName(_selectedSlotIndex)}]. Bấm [SỬ DỤNG] hoặc [⇄ CHUYỂN Ô]. (Chuột phải để ăn ngay)";
                GUI.Label(new Rect(x, hintY, width, 16), moveHint, _hintStyle);
            }
            else
            {
                moveHint = "💡 Nhấp ô để xem • Chuột phải / Đúp chuột để ăn ngay • Bấm [⇄ CHUYỂN Ô] để dời đồ.";
                GUI.Label(new Rect(x, hintY, width, 16), moveHint, _hintStyle);
            }

            // 4. CHI TIẾT VẬT PHẨM ĐÃ CHỌN
            float detailY = hintY + 18f;
            float detailH = height - (detailY - y);
            DrawItemDetails(x, detailY, width, detailH);
        }

        private void DrawSingleSlot(int index, float x, float y, float size, string badgeText)
        {
            Rect slotRect = new Rect(x, y, size, size);

            bool isSelected = (_selectedSlotIndex == index);
            Color bgColor = _slotBg;
            if (_isMovingMode && isSelected)
            {
                bgColor = _slotMovingBg;
            }
            else if (isSelected)
            {
                bgColor = _slotSelectedBg;
            }
            else if (index < InventorySystem.HotbarSlots)
            {
                bgColor = _slotHotbarBg;
            }

            Color borderColor = isSelected ? _borderGold : _borderInner;

            // Viền & Nền ô
            DrawSolidRect(new Rect(slotRect.x - 1, slotRect.y - 1, slotRect.width + 2, slotRect.height + 2), borderColor);
            DrawSolidRect(slotRect, bgColor);

            // Badge phím tắt [1..6]
            if (!string.IsNullOrEmpty(badgeText))
            {
                GUI.Label(new Rect(slotRect.x + 3, slotRect.y + 1, 14, 14), badgeText, _hotkeyBadgeStyle);
            }

            if (inventory != null)
            {
                var slot = inventory.GetSlot(index);
                if (slot != null && !slot.IsEmpty)
                {
                    // Icon Glyphs (🪵, 🪨, 🍎, ⚔, 🪓, 🍙)
                    string glyph = !string.IsNullOrEmpty(slot.Item.iconGlyph) ? slot.Item.iconGlyph : "📦";
                    GUI.Label(slotRect, glyph, _slotGlyphStyle);

                    // Số lượng
                    if (slot.Quantity > 1)
                    {
                        GUI.Label(new Rect(slotRect.x + 2, slotRect.y + size - 16, size - 6, 14), slot.Quantity.ToString(), _slotQuantityStyle);
                    }
                }
            }

            // Bắt sự kiện Chuột Phải hoặc Nhấp Đúp để ĂN / DÙNG NGAY
            Event e = Event.current;
            if (e != null && e.isMouse && slotRect.Contains(e.mousePosition))
            {
                if (e.type == EventType.MouseDown)
                {
                    if (e.button == 1) // Chuột phải: Dùng ngay
                    {
                        _selectedSlotIndex = index;
                        _isMovingMode = false;
                        if (inventory != null)
                        {
                            inventory.UseItem(index);
                        }
                        e.Use();
                        return;
                    }
                    else if (e.button == 0 && e.clickCount == 2) // Nhấp đúp chuột trái: Dùng ngay
                    {
                        _selectedSlotIndex = index;
                        _isMovingMode = false;
                        if (inventory != null)
                        {
                            inventory.UseItem(index);
                        }
                        e.Use();
                        return;
                    }
                }
            }

            // Click Chuột Trái bình thường: Chọn ô hoặc Đặt đồ khi đang chuyển
            if (GUI.Button(slotRect, GUIContent.none, GUIStyle.none))
            {
                HandleSlotClick(index);
            }
        }

        private string GetSlotDisplayName(int slotIndex)
        {
            if (slotIndex < InventorySystem.HotbarSlots)
            {
                return $"Phím {slotIndex + 1}";
            }
            return $"Ba Lô #{slotIndex - InventorySystem.HotbarSlots + 1}";
        }

        /// <summary>
        /// Xử lý chọn ô và di chuyển đồ:
        /// - Khi bình thường: Nhấp ô nào thì CHỌN ô đó (xem thông tin, chuẩn bị ăn).
        /// - Khi bấm [⇄ CHUYỂN Ô]: Nhấp ô đích để chuyển / hoán đổi / gộp đồ.
        /// </summary>
        private void HandleSlotClick(int clickedIndex)
        {
            if (_isMovingMode)
            {
                // Đang trong chế độ chuyển đồ: Dời sang ô đích
                if (inventory != null && _selectedSlotIndex >= 0 && _selectedSlotIndex != clickedIndex)
                {
                    inventory.SwapSlots(_selectedSlotIndex, clickedIndex);
                    _selectedSlotIndex = clickedIndex;
                }
                _isMovingMode = false;
                return;
            }

            // Bình thường: Chỉ việc chọn ô này
            _selectedSlotIndex = clickedIndex;
        }

        private void DrawItemDetails(float x, float y, float width, float height)
        {
            DrawSolidRect(new Rect(x - 1, y - 1, width + 2, height + 2), _borderInner);
            DrawSolidRect(new Rect(x, y, width, height), _panelInnerBg);

            if (inventory == null || _selectedSlotIndex < 0)
            {
                GUI.Label(new Rect(x + 10, y + 10, width - 20, 36), "Nhấp vào 1 ô để xem thông tin hoặc sử dụng.\nNhấp đúp hoặc chuột phải vào đồ ăn để ăn ngay.", _tooltipDescStyle);
                return;
            }

            var slot = inventory.GetSlot(_selectedSlotIndex);
            if (slot == null || slot.IsEmpty)
            {
                GUI.Label(new Rect(x + 10, y + 10, width - 20, 30), $"[{GetSlotDisplayName(_selectedSlotIndex)}] Hiện đang trống.", _tooltipDescStyle);
                return;
            }

            ItemDataSO item = slot.Item;
            GUI.Label(new Rect(x + 10, y + 4, width - 170, 16), $"{item.iconGlyph} {item.itemName} (x{slot.Quantity})", _tooltipTitleStyle);
            GUI.Label(new Rect(x + 10, y + 20, width - 170, 28), item.description, _tooltipDescStyle);

            // Hiển thị chỉ số hồi phục nếu là Consumable
            if (item.itemType == ItemType.Consumable)
            {
                float hunger = (item.itemId == "consumable_bamboo_rice") ? 45f : (item.itemId == "resource_berry" ? 15f : (item.hungerRestore > 0 ? item.hungerRestore : 20f));
                float hp = (item.itemId == "consumable_bamboo_rice") ? 30f : (item.itemId == "resource_berry" ? 5f : (item.healthRestore > 0 ? item.healthRestore : 5f));
                GUI.Label(new Rect(x + 10, y + 46, width - 170, 16), $"+{hunger} Độ No  •  +{hp} Máu", _statBonusStyle);
            }

            // Nếu đang trong chế độ chuyển ô
            if (_isMovingMode)
            {
                Rect cancelMoveRect = new Rect(x + width - 120, y + 14, 110, 32);
                DrawSolidRect(cancelMoveRect, _btnCancelBg);
                if (GUI.Button(cancelMoveRect, "✕ Hủy Chuyển", _btnStyle))
                {
                    _isMovingMode = false;
                }
                return;
            }

            // Nút "⇄ CHUYỂN Ô"
            Rect moveBtnRect = new Rect(x + width - 165, y + 14, 78, 30);
            DrawSolidRect(moveBtnRect, _btnMoveBg);
            if (GUI.Button(moveBtnRect, "⇄ Chuyển Ô", _btnStyle))
            {
                _isMovingMode = true;
            }

            // Nút "SỬ DỤNG / TRANG BỊ"
            Rect actionBtnRect = new Rect(x + width - 82, y + 14, 76, 30);
            string actionText = item.itemType == ItemType.Weapon || item.itemType == ItemType.Tool ? "TRANG BỊ" : "ĂN NGAY";

            DrawSolidRect(actionBtnRect, _btnActionBg);
            if (GUI.Button(actionBtnRect, actionText, _btnStyle))
            {
                int usedSlot = _selectedSlotIndex;
                inventory.UseItem(usedSlot);

                // Nếu sau khi dùng ô đã hết sạch thì bỏ chọn
                var currentSlot = inventory.GetSlot(usedSlot);
                if (currentSlot == null || currentSlot.IsEmpty)
                {
                    _selectedSlotIndex = -1;
                }
            }
        }

        private void DrawCraftingColumn(float x, float y, float width, float height)
        {
            GUI.Label(new Rect(x, y, width, 16), "🔨 CÔNG THỨC CHẾ TẠO DÂN GIAN", _columnHeaderStyle);

            if (craftingSystem == null || craftingSystem.AvailableRecipes == null || craftingSystem.AvailableRecipes.Count == 0)
            {
                GUI.Label(new Rect(x, y + 30, width, 30), "Chưa có công thức chế tạo nào.", _tooltipDescStyle);
                return;
            }

            float rowStartY = y + 20f;
            float rowHeight = 64f;
            float rowGap = 8f;

            var recipes = craftingSystem.AvailableRecipes;
            for (int i = 0; i < recipes.Count; i++)
            {
                var recipe = recipes[i];
                if (recipe == null) continue;

                float rowY = rowStartY + i * (rowHeight + rowGap);
                if (rowY + rowHeight > y + height) break;

                DrawRecipeRow(x, rowY, width, rowHeight, recipe);
            }
        }

        private void DrawRecipeRow(float x, float y, float width, float height, CraftingRecipeSO recipe)
        {
            bool canCraft = craftingSystem != null && craftingSystem.CanCraft(recipe);
            Color rowBg = canCraft ? new Color(0.04f, 0.12f, 0.08f, 0.65f) : new Color(0.03f, 0.04f, 0.06f, 0.55f);
            Color rowBorder = canCraft ? new Color(0.35f, 0.75f, 0.40f, 0.65f) : _borderInner;

            DrawSolidRect(new Rect(x - 1, y - 1, width + 2, height + 2), rowBorder);
            DrawSolidRect(new Rect(x, y, width, height), rowBg);

            // Icon & Tên thành phẩm
            string resultGlyph = recipe.resultItem != null ? recipe.resultItem.iconGlyph : "📦";
            string title = $"{resultGlyph} {recipe.recipeName}";
            GUI.Label(new Rect(x + 8, y + 4, width - 90, 16), title, _recipeNameStyle);

            // Chuỗi nguyên liệu yêu cầu
            string reqStr = "";
            for (int ing = 0; ing < recipe.ingredients.Count; ing++)
            {
                var req = recipe.ingredients[ing];
                if (req.item == null) continue;

                int currentCount = inventory != null ? inventory.GetItemCount(req.item.itemId) : 0;
                string glyph = req.item.iconGlyph;
                reqStr += $"{glyph} {currentCount}/{req.amount}   ";
            }
            GUI.Label(new Rect(x + 8, y + 24, width - 90, 16), reqStr, _recipeReqStyle);
            GUI.Label(new Rect(x + 8, y + 42, width - 90, 18), recipe.description, _tooltipDescStyle);

            // Nút Chế Tạo
            Rect craftBtnRect = new Rect(x + width - 78, y + 16, 70, 32);
            Color btnColor = canCraft ? _btnCraftActive : _btnCraftDisabled;
            DrawSolidRect(craftBtnRect, btnColor);

            if (GUI.Button(craftBtnRect, "CHẾ TẠO", _btnStyle))
            {
                if (canCraft)
                {
                    craftingSystem.TryCraft(recipe);
                }
            }
        }

        private void DrawSolidRect(Rect rect, Color color)
        {
            Color prevColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, _whiteTex);
            GUI.color = prevColor;
        }
    }
}
