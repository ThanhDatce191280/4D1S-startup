using UnityEngine;

namespace HuyetNguyetDaiNgan.Player
{
    /// <summary>
    /// Giao diện trừu tượng hóa tín hiệu điều khiển người chơi.
    /// Giúp tách rời logic di chuyển khỏi phần cứng bàn phím/chuột và hỗ trợ Unit Test.
    /// </summary>
    public interface IPlayerInput
    {
        /// <summary>
        /// Vector di chuyển 2D (X: Ngang [-1, 1], Y: Dọc [-1, 1]). Đã chuẩn hóa.
        /// </summary>
        Vector2 MoveInput { get; }

        /// <summary>
        /// Trạng thái đang giữ phím chạy nhanh (Left Shift / Gamepad L3).
        /// </summary>
        bool IsSprinting { get; }

        /// <summary>
        /// Tọa độ con trỏ chuột trên màn hình (Pixels).
        /// </summary>
        Vector2 MousePosition { get; }

        /// <summary>
        /// Trạng thái kích hoạt đòn đánh / tương tác chính (Chuột trái / Phím Space).
        /// </summary>
        bool AttackTriggered { get; }
    }
}

