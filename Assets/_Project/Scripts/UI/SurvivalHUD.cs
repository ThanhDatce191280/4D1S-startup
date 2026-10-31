using HuyetNguyetDaiNgan.Combat;
using HuyetNguyetDaiNgan.Inventory;
using HuyetNguyetDaiNgan.Player;
using UnityEngine;

namespace HuyetNguyetDaiNgan.UI
{
    /// <summary>
    /// Giao diện HUD sinh tồn hiển thị 4 thanh trạng thái (HP, Hunger, Stamina, Mana)
    /// và bộ đếm tài nguyên thu thập (Gỗ, Đá, Quả).
    /// Thiết kế Smoked Glass / Kính Mờ Trong Suốt, cực kỳ nhỏ gọn, không che chắn tầm nhìn.
    /// Đặt ở góc dưới - phải màn hình, thuận tiện quan sát thế giới xung quanh.
    /// </summary>
    public class SurvivalHUD : MonoBehaviour
    {
        [Header("Target References")]
        [Tooltip("Thực thể người chơi cần theo dõi chỉ số")]
        [SerializeField] private PlayerStatsComponent playerStats;
        [SerializeField] private HealthComponent playerHealth;
        [SerializeField] private InventorySystem playerInventory;

        [Header("Position & Layout")]
        [Tooltip("Chiều rộng bảng chỉ số (pixel)")]
        [SerializeField] private float cardWidth = 224f;

        [Tooltip("Khoảng cách từ mép phải màn hình (pixel)")]
        [SerializeField] private float rightMargin = 20f;

        [Tooltip("Khoảng cách từ đáy màn hình lên (pixel)")]
        [SerializeField] private float bottomOffset = 115f;

        // Bảng màu Kính mờ trong suốt (Smoked Glass / Translucent)
        private readonly Color _panelBg = new Color(0.03f, 0.05f, 0.07f, 0.42f);           // Nền kính trong suốt 42%
        private readonly Color _panelBorderOuter = new Color(0.72f, 0.58f, 0.28f, 0.45f); // Viền vàng kim mỏng nhẹ
        private readonly Color _panelBorderInner = new Color(0.25f, 0.30f, 0.36f, 0.22f);
        private readonly Color _troughBg = new Color(0.01f, 0.02f, 0.04f, 0.40f);         // Lòng thanh rỗng trong suốt
        private readonly Color _troughBorder = new Color(0.35f, 0.40f, 0.45f, 0.30f);

        // Màu thanh trạng thái ngọc sáng (Vibrant Jewel Fills)
        private readonly Color _hpColor = new Color(0.94f, 0.22f, 0.24f, 0.85f);       // Đỏ Ruby
        private readonly Color _hungerColor = new Color(0.98f, 0.56f, 0.12f, 0.85f);   // Cam Mật Ong
        private readonly Color _staminaColor = new Color(0.96f, 0.84f, 0.16f, 0.85f);  // Vàng Ánh Kim
        private readonly Color _manaColor = new Color(0.12f, 0.68f, 0.98f, 0.85f);     // Xanh Lapis

        // Cached Textures & Styles
        private Texture2D _whiteTex;
        private GUIStyle _headerStyle;
        private GUIStyle _barLeftStyle;
        private GUIStyle _barRightStyle;
        private GUIStyle _chipStyle;
        private GUIStyle _hintStyle;
        private GUIStyle _alertStyle;

        private void Awake()
        {
            _whiteTex = new Texture2D(1, 1);
            _whiteTex.SetPixel(0, 0, Color.white);
            _whiteTex.Apply();

            FindTargetsIfNeeded();
        }

        private void Start()
        {
            InitStyles();
        }

        private void OnDestroy()
        {
            if (_whiteTex != null)
            {
                Destroy(_whiteTex);
            }
        }

        private void FindTargetsIfNeeded()
        {
            if (playerStats == null)
            {
                playerStats = FindFirstObjectByType<PlayerStatsComponent>();
            }

            if (playerHealth == null && playerStats != null)
            {
                playerHealth = playerStats.GetComponent<HealthComponent>();
            }

            if (playerHealth == null)
            {
                playerHealth = FindFirstObjectByType<HealthComponent>();
            }

            if (playerInventory == null && playerStats != null)
            {
                playerInventory = playerStats.GetComponent<InventorySystem>();
            }

            if (playerInventory == null)
            {
                playerInventory = FindFirstObjectByType<InventorySystem>();
            }
        }

        private void InitStyles()
        {
            _headerStyle = new GUIStyle
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = new Color(0.96f, 0.88f, 0.68f, 0.95f) }
            };

            _barLeftStyle = new GUIStyle
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = new Color(0.96f, 0.96f, 0.96f, 0.95f) }
            };

            _barRightStyle = new GUIStyle
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleRight,
                normal = { textColor = new Color(1f, 1f, 1f, 0.95f) }
            };

            _chipStyle = new GUIStyle
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };

            _hintStyle = new GUIStyle
            {
                fontSize = 10,
                fontStyle = FontStyle.Normal,
                normal = { textColor = new Color(0.85f, 0.88f, 0.92f, 0.85f) }
            };

            _alertStyle = new GUIStyle
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(1f, 0.35f, 0.35f, 0.95f) }
            };
        }

        private void OnGUI()
        {
            if (_whiteTex == null) return;
            FindTargetsIfNeeded();

            if (playerStats == null) return;

            // 1. Tọa độ Bảng chỉ số: Nhỏ gọn, Trong suốt, Bên Phải, Lửng lơ trên đáy
            float cardHeight = 146f;
            float startX = Screen.width - cardWidth - rightMargin;
            float startY = Mathf.Max(20f, Screen.height - cardHeight - bottomOffset);

            // Nền kính mờ trong suốt (Smoked Glass) + viền kim loại mảnh
            DrawSolidRect(new Rect(startX - 1, startY - 1, cardWidth + 2, cardHeight + 2), _panelBorderOuter);
            DrawSolidRect(new Rect(startX, startY, cardWidth, cardHeight), _panelBg);

            // Vạch điểm nhấn màu vàng kim trên đỉnh
            DrawSolidRect(new Rect(startX + 8, startY, cardWidth - 16, 1.5f), new Color(0.85f, 0.70f, 0.30f, 0.7f));

            float innerX = startX + 10f;
            float innerWidth = cardWidth - 20f;
            float currentY = startY + 6f;

            // Tiêu đề nhỏ tinh tế
            GUI.Label(new Rect(innerX, currentY, innerWidth, 16), "❖  SINH MỆNH", _headerStyle);

            currentY += 19f;
            float barHeight = 13f;
            float spacing = 18f;

            // Bar 1: Sinh lực (HP)
            float hpCurrent = playerHealth != null ? playerHealth.CurrentHealth : 100f;
            float hpMax = playerHealth != null ? playerHealth.MaxHealth : 100f;
            float hpPercent = Mathf.Clamp01(hpCurrent / Mathf.Max(hpMax, 0.001f));
            DrawGlassyBar(innerX, currentY, innerWidth, barHeight, hpPercent, _hpColor, "♥ HP", $"{Mathf.CeilToInt(hpCurrent)}/{Mathf.CeilToInt(hpMax)}");

            // Bar 2: Thức ăn (Hunger)
            currentY += spacing;
            float hungerCurrent = playerStats.CurrentHunger;
            float hungerMax = playerStats.MaxHunger;
            float hungerPercent = playerStats.HungerPercent;
            DrawGlassyBar(innerX, currentY, innerWidth, barHeight, hungerPercent, _hungerColor, "🍖 NO", $"{Mathf.CeilToInt(hungerCurrent)}/{Mathf.CeilToInt(hungerMax)}");

            // Bar 3: Thể lực (Stamina)
            currentY += spacing;
            float stamCurrent = playerStats.CurrentStamina;
            float stamMax = playerStats.MaxStamina;
            float stamPercent = playerStats.StaminaPercent;
            DrawGlassyBar(innerX, currentY, innerWidth, barHeight, stamPercent, _staminaColor, "⚡ LỰC", $"{Mathf.CeilToInt(stamCurrent)}/{Mathf.CeilToInt(stamMax)}");

            // Bar 4: Năng lượng (Mana)
            currentY += spacing;
            float manaCurrent = playerStats.CurrentMana;
            float manaMax = playerStats.MaxMana;
            float manaPercent = playerStats.ManaPercent;
            DrawGlassyBar(innerX, currentY, innerWidth, barHeight, manaPercent, _manaColor, "💧 MANA", $"{Mathf.CeilToInt(manaCurrent)}/{Mathf.CeilToInt(manaMax)}");

            // Báo động trạng thái đói / no nê
            currentY += 17f;
            if (hungerPercent <= 0.001f)
            {
                DrawSolidRect(new Rect(innerX, currentY, innerWidth, 14), new Color(0.5f, 0.05f, 0.05f, 0.5f));
                GUI.Label(new Rect(innerX, currentY, innerWidth, 14), "⚠ ĐÓI: RÚT MÁU (-3/s)", _alertStyle);
                currentY += 16f;
            }
            else if (hungerCurrent >= 80f)
            {
                DrawSolidRect(new Rect(innerX, currentY, innerWidth, 14), new Color(0.08f, 0.4f, 0.12f, 0.45f));
                GUI.color = new Color(0.5f, 1f, 0.6f, 0.95f);
                GUI.Label(new Rect(innerX, currentY, innerWidth, 14), "✨ NO NÊ: HỒI MÁU (+1.5/s)", _alertStyle);
                GUI.color = Color.white;
                currentY += 16f;
            }
            else
            {
                currentY += 3f;
            }

            // Kho tài nguyên: 3 mini pills nhỏ gọn nằm ngang (đồng bộ chính xác 100% với túi đồ)
            float chipWidth = (innerWidth - 8f) / 3f;
            float chipHeight = 18f;

            int woodVal = playerInventory != null ? playerInventory.GetItemCount("resource_wood") : playerStats.WoodCount;
            int stoneVal = playerInventory != null ? playerInventory.GetItemCount("resource_stone") : playerStats.StoneCount;
            int foodVal = playerInventory != null ? playerInventory.GetItemCount("resource_berry") : playerStats.FoodCount;

            DrawMiniResourceChip(innerX, currentY, chipWidth, chipHeight, $"🪵 {woodVal}", new Color(0.65f, 0.42f, 0.22f, 0.8f));
            DrawMiniResourceChip(innerX + chipWidth + 4f, currentY, chipWidth, chipHeight, $"🪨 {stoneVal}", new Color(0.65f, 0.68f, 0.72f, 0.8f));
            DrawMiniResourceChip(innerX + (chipWidth + 4f) * 2f, currentY, chipWidth, chipHeight, $"🍎 {foodVal}", new Color(0.95f, 0.30f, 0.30f, 0.8f));

            // 2. Bảng Hướng dẫn thao tác (Góc dưới bên trái, trong suốt thanh mảnh)
            float hintW = 340f;
            float hintH = 42f;
            float hintX = 16f;
            float hintY = Screen.height - hintH - 16f;

            DrawSolidRect(new Rect(hintX - 1, hintY - 1, hintW + 2, hintH + 2), new Color(0.25f, 0.30f, 0.35f, 0.25f));
            DrawSolidRect(new Rect(hintX, hintY, hintW, hintH), _panelBg);

            string hintText = "• WASD: Di chuyển | Giữ Shift: Chạy nhanh (Tốn thể lực)\n" +
                              "• Chuột Trái / Space: Chặt Cây / Đập Đá / Hái Quả";
            GUI.Label(new Rect(hintX + 8, hintY + 5, hintW - 16, hintH - 10), hintText, _hintStyle);
        }

        private void DrawGlassyBar(float x, float y, float width, float height, float percent, Color fillColor, string leftText, string rightText)
        {
            // Trough trong suốt viền mỏng
            DrawSolidRect(new Rect(x - 1, y - 1, width + 2, height + 2), _troughBorder);
            DrawSolidRect(new Rect(x, y, width, height), _troughBg);

            // Thanh màu bán trong suốt
            float fillWidth = width * Mathf.Clamp01(percent);
            if (fillWidth > 0f)
            {
                DrawSolidRect(new Rect(x, y, fillWidth, height), fillColor);

                // Dải phản quang mỏng ở trên đỉnh (Glass highlight)
                Color highlightColor = Color.Lerp(fillColor, Color.white, 0.4f);
                highlightColor.a = 0.4f;
                DrawSolidRect(new Rect(x, y, fillWidth, 2f), highlightColor);
            }

            // Chữ hiển thị có bóng đổ đen viền nhẹ để đọc rõ trên nền trong suốt
            Rect textRect = new Rect(x + 5, y - 1, width - 10, height);

            Color prevColor = GUI.color;
            GUI.color = new Color(0f, 0f, 0f, 0.9f);
            GUI.Label(new Rect(textRect.x + 1, textRect.y + 1, textRect.width, textRect.height), leftText, _barLeftStyle);
            GUI.Label(new Rect(textRect.x + 1, textRect.y + 1, textRect.width, textRect.height), rightText, _barRightStyle);

            GUI.color = prevColor;
            GUI.Label(textRect, leftText, _barLeftStyle);
            GUI.Label(textRect, rightText, _barRightStyle);
        }

        private void DrawMiniResourceChip(float x, float y, float width, float height, string text, Color accentColor)
        {
            // Viền và nền chip trong suốt
            DrawSolidRect(new Rect(x - 1, y - 1, width + 2, height + 2), new Color(accentColor.r, accentColor.g, accentColor.b, 0.35f));
            DrawSolidRect(new Rect(x, y, width, height), new Color(0.02f, 0.03f, 0.05f, 0.45f));

            // Vạch màu nhỏ nhận diện
            DrawSolidRect(new Rect(x, y, 2.5f, height), accentColor);

            // Bóng đổ + Text
            Color prevColor = GUI.color;
            GUI.color = new Color(0f, 0f, 0f, 0.85f);
            GUI.Label(new Rect(x + 1, y + 1, width, height), text, _chipStyle);

            GUI.color = prevColor;
            GUI.Label(new Rect(x, y, width, height), text, _chipStyle);
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
