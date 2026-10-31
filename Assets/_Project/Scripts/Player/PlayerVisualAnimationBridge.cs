using UnityEngine;

namespace HuyetNguyetDaiNgan.Player
{
    /// <summary>
    /// Cầu nối truyền thông số từ PlayerMovementController sang Animator của VisualRoot.
    /// Hoạt động an toàn ngay cả khi chưa gắn Animator hoặc chưa có animation hoàn chỉnh.
    /// </summary>
    public class PlayerVisualAnimationBridge : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private Animator animator;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int IsSprintingHash = Animator.StringToHash("IsSprinting");

        private void Awake()
        {
            if (movementController == null)
            {
                movementController = GetComponentInParent<PlayerMovementController>();
            }

            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        private void Update()
        {
            if (animator == null || movementController == null) return;

            float speed = movementController.CurrentSpeed;
            bool isGrounded = movementController.IsGrounded;
            bool isSprinting = speed > 5.5f;

            animator.SetFloat(SpeedHash, speed);
            animator.SetBool(IsGroundedHash, isGrounded);
            animator.SetBool(IsSprintingHash, isSprinting);
        }
    }
}

