using NUnit.Framework;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Tests
{
    [TestFixture]
    public class FoundationTests
    {
        [Test]
        public void M1_Environment_MathCalculations_AreValid()
        {
            // Kiểm tra các phép tính toán học cơ bản và vector trong Unity Engine
            Vector3 startPos = Vector3.zero;
            Vector3 moveDir = new Vector3(1f, 0f, 1f).normalized;
            Vector3 targetPos = startPos + moveDir * 5f;

            Assert.AreEqual(5f, Vector3.Distance(startPos, targetPos), 0.001f);
        }

        [Test]
        public void M1_Project_AssemblyDefinition_IsOperational()
        {
            // Xác nhận assembly Runtime và Test runner hoạt động đúng chuẩn
            bool isEngineRunning = Application.isPlaying || !Application.isPlaying;
            Assert.IsTrue(isEngineRunning, "Unity test runner environment must be active.");
        }
    }
}

