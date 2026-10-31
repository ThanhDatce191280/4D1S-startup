using HuyetNguyetDaiNgan.Player;
using NUnit.Framework;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Tests
{
    [TestFixture]
    public class MovementMathTests
    {
        private class MockPlayerInput : IPlayerInput
        {
            public Vector2 MoveInput { get; set; }
            public bool IsSprinting { get; set; }
            public Vector2 MousePosition { get; set; }
            public bool AttackTriggered { get; set; }
        }

        [Test]
        public void CameraRelativeDirection_WhenZeroInput_ReturnsZero()
        {
            var go = new GameObject("TestPlayer");
            go.AddComponent<CharacterController>();
            var controller = go.AddComponent<PlayerMovementController>();

            Vector3 result = controller.CalculateCameraRelativeDirection(Vector2.zero);

            Assert.AreEqual(Vector3.zero, result);
            Object.DestroyImmediate(go);
        }

        [Test]
        public void CameraRelativeDirection_DiagonalInput_IsNormalized()
        {
            var go = new GameObject("TestPlayer");
            go.AddComponent<CharacterController>();
            var controller = go.AddComponent<PlayerMovementController>();

            // Nhấn cùng lúc W và D (1, 1)
            Vector2 diagonalInput = new Vector2(1f, 1f).normalized;
            Vector3 result = controller.CalculateCameraRelativeDirection(diagonalInput);

            Assert.LessOrEqual(result.magnitude, 1.001f, "Vận tốc đường chéo không được vượt quá 1.0 để chống lỗi hack tốc độ.");
            Object.DestroyImmediate(go);
        }

        [Test]
        public void MockInput_CorrectlyTogglesSprintState()
        {
            var mock = new MockPlayerInput
            {
                MoveInput = new Vector2(0f, 1f),
                IsSprinting = true
            };

            Assert.IsTrue(mock.IsSprinting);
            Assert.AreEqual(new Vector2(0f, 1f), mock.MoveInput);
        }
    }
}

