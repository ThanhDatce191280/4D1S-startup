using HuyetNguyetDaiNgan.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HuyetNguyetDaiNgan.Player
{
    /// <summary>
    /// Thu thập tín hiệu điều khiển từ New Input System (Keyboard, Mouse, Gamepad).
    /// </summary>
    public class PlayerInputHandler : MonoBehaviour, IPlayerInput
    {
        public Vector2 MoveInput { get; private set; }
        public bool IsSprinting { get; private set; }
        public Vector2 MousePosition { get; private set; }
        public bool AttackTriggered { get; private set; }

        private void Update()
        {
            ReadMovementInput();
            ReadMousePosition();
            ReadActionInputs();
        }

        private void ReadMovementInput()
        {
            Vector2 rawInput = Vector2.zero;

            // 1. Đọc từ Bàn phím
            Keyboard kb = Keyboard.current;
            if (kb != null)
            {
                if (kb.wKey.isPressed || kb.upArrowKey.isPressed) rawInput.y += 1f;
                if (kb.sKey.isPressed || kb.downArrowKey.isPressed) rawInput.y -= 1f;
                if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) rawInput.x -= 1f;
                if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) rawInput.x += 1f;

                IsSprinting = kb.leftShiftKey.isPressed || kb.rightShiftKey.isPressed;
            }

            // 2. Đọc từ Gamepad (nếu có kết nối)
            Gamepad gp = Gamepad.current;
            if (gp != null && rawInput == Vector2.zero)
            {
                Vector2 stick = gp.leftStick.ReadValue();
                if (stick.sqrMagnitude > 0.05f)
                {
                    rawInput = stick;
                }
                if (gp.leftStickButton.isPressed)
                {
                    IsSprinting = true;
                }
            }

            // Chuẩn hóa vector nếu độ dài > 1 để không bị nhanh hơn khi đi chéo
            MoveInput = rawInput.sqrMagnitude > 1f ? rawInput.normalized : rawInput;
        }

        private void ReadMousePosition()
        {
            Mouse mouse = Mouse.current;
            if (mouse != null)
            {
                MousePosition = mouse.position.ReadValue();
            }
        }

        private void ReadActionInputs()
        {
            AttackTriggered = false;

            // Chặn kích hoạt tấn công khi đang mở Túi đồ hoặc chuột đang trỏ vào vùng UI
            if (InventoryCraftingUI.IsOpenGlobal || InventoryCraftingUI.IsMouseOverAnyUI())
            {
                return;
            }

            Mouse mouse = Mouse.current;
            if (mouse != null && mouse.leftButton.wasPressedThisFrame)
            {
                AttackTriggered = true;
                return;
            }

            Keyboard kb = Keyboard.current;
            if (kb != null && kb.spaceKey.wasPressedThisFrame)
            {
                AttackTriggered = true;
                return;
            }

            Gamepad gp = Gamepad.current;
            if (gp != null && gp.buttonWest.wasPressedThisFrame) // Nút X (Xbox) / Vuông (PS)
            {
                AttackTriggered = true;
            }
        }
    }
}

