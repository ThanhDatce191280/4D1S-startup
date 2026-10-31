using UnityEngine;
using UnityEngine.InputSystem;

namespace HuyetNguyetDaiNgan.Core
{
    /// <summary>
    /// Hiển thị FPS và thông tin phiên bản kiểm thử độc lập ở góc màn hình.
    /// Không cấp phát rác (zero-allocation) trong Update.
    /// </summary>
    public class RuntimeInfoDisplay : MonoBehaviour
    {
        private float _deltaTime;
        private GUIStyle _style;
        private Rect _rect;

        private void Awake()
        {
            _rect = new Rect(20, 20, 400, 80);
            _style = new GUIStyle
            {
                alignment = TextAnchor.UpperLeft,
                fontSize = 18,
                normal = { textColor = new Color(0.9f, 0.95f, 0.9f, 1f) }
            };
        }

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;

            // Cho phép thoát nhanh game độc lập bằng phím Escape qua New Input System
            Keyboard kb = Keyboard.current;
            if (kb != null && kb.escapeKey.wasPressedThisFrame)
            {
                Application.Quit();
            }
        }

        private void OnGUI()
        {
            float msec = _deltaTime * 1000.0f;
            float fps = 1.0f / Mathf.Max(_deltaTime, 0.0001f);
            string text = $"Huyết Nguyệt Đại Ngàn — M1 Foundation Test\nFPS: {fps:0.} ({msec:0.0} ms) | Phím ESC: Thoát";
            GUI.Label(_rect, text, _style);
        }
    }
}

