using UnityEngine;

namespace HuyetNguyetDaiNgan.Player
{
    /// <summary>
    /// Điều khiển chuyển động 3D theo góc nhìn Camera Isometric (Camera-relative movement).
    /// Áp dụng CharacterController, trọng lực và xoay Visual độc lập.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("Tốc độ đi bộ cơ bản (m/s)")]
        [SerializeField] private float walkSpeed = 5.0f;

        [Tooltip("Tốc độ chạy nước rút (m/s)")]
        [SerializeField] private float sprintSpeed = 8.5f;

        [Tooltip("Tốc độ xoay mặt nhân vật theo hướng di chuyển (rad/s)")]
        [SerializeField] private float rotationSpeed = 14.0f;

        [Tooltip("Gia tốc trọng lực (m/s^2)")]
        [SerializeField] private float gravity = -20.0f;

        [Header("Visual & Camera References")]
        [Tooltip("Transform của GameObject con chứa Model 3D để xoay độc lập")]
        [SerializeField] private Transform visualTransform;

        [Tooltip("Camera chính dùng để tính hướng di chuyển tương đối")]
        [SerializeField] private Camera targetCamera;

        // Dependencies & State
        private CharacterController _characterController;
        private IPlayerInput _playerInput;
        private PlayerStatsComponent _statsComponent;
        private float _verticalVelocity;
        private float _currentSpeed;

        public float CurrentSpeed => _currentSpeed;
        public float NormalizedSpeed => _characterController.velocity.magnitude / Mathf.Max(sprintSpeed, 0.001f);
        public bool IsGrounded => _characterController != null && _characterController.isGrounded;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerInput = GetComponent<IPlayerInput>();
            _statsComponent = GetComponent<PlayerStatsComponent>();

            // Cấu hình chuẩn cho CharacterController
            _characterController.slopeLimit = 45f;
            _characterController.stepOffset = 0.35f;

            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }

            if (visualTransform == null)
            {
                Transform foundVisual = transform.Find("PlayerVisual");
                visualTransform = foundVisual != null ? foundVisual : transform;
            }
        }

        public void SetInputProvider(IPlayerInput inputProvider)
        {
            _playerInput = inputProvider;
        }

        public void SetTargetCamera(Camera cam)
        {
            targetCamera = cam;
        }

        private void Update()
        {
            if (_playerInput == null)
            {
                _playerInput = GetComponent<IPlayerInput>();
                if (_playerInput == null) return;
            }

            HandleMovement();
        }

        private void HandleMovement()
        {
            Vector2 moveInput = _playerInput.MoveInput;
            bool isMoving = moveInput.sqrMagnitude > 0.01f;

            // 1. Tính toán vector hướng di chuyển tương đối theo Camera
            Vector3 moveDirection = CalculateCameraRelativeDirection(moveInput);

            // 2. Xác định tốc độ hiện tại (Tích hợp Stamina)
            float targetSpeed = 0f;
            if (isMoving)
            {
                bool wantSprint = _playerInput.IsSprinting;
                if (wantSprint && _statsComponent != null)
                {
                    if (_statsComponent.CurrentStamina > 1f)
                    {
                        targetSpeed = sprintSpeed;
                        _statsComponent.ConsumeStaminaContinuous(14f);
                    }
                    else
                    {
                        targetSpeed = walkSpeed; // Hết thể lực thì ép về đi bộ
                    }
                }
                else
                {
                    targetSpeed = wantSprint ? sprintSpeed : walkSpeed;
                }
            }

            _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, 15f * Time.deltaTime);

            // 3. Xoay mặt nhân vật theo hướng di chuyển mượt mà
            if (isMoving && moveDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                if (visualTransform != null)
                {
                    visualTransform.rotation = Quaternion.Slerp(visualTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }

            // 4. Xử lý Trọng lực (Gravity & Grounding)
            if (_characterController.isGrounded)
            {
                if (_verticalVelocity < 0f)
                {
                    // Lực nén nhẹ xuống sàn để bám dốc, không bị nảy khi đi xuống dốc
                    _verticalVelocity = -2.0f;
                }
            }
            else
            {
                _verticalVelocity += gravity * Time.deltaTime;
            }

            // 5. Di chuyển nhân vật qua CharacterController
            Vector3 finalMovement = (moveDirection * _currentSpeed) + (Vector3.up * _verticalVelocity);
            _characterController.Move(finalMovement * Time.deltaTime);
        }

        /// <summary>
        /// Quy đổi vector 2D từ Input sang vector 3D không gian thế giới dựa theo góc nhìn Camera.
        /// </summary>
        public Vector3 CalculateCameraRelativeDirection(Vector2 input)
        {
            if (input.sqrMagnitude < 0.001f) return Vector3.zero;

            Transform camTransform = targetCamera != null ? targetCamera.transform : null;
            if (camTransform == null && Camera.main != null)
            {
                camTransform = Camera.main.transform;
            }

            if (camTransform == null)
            {
                // Fallback nếu chưa có camera: dùng hướng thế giới (X ngang, Z dọc)
                return new Vector3(input.x, 0f, input.y).normalized;
            }

            Vector3 camForward = camTransform.forward;
            Vector3 camRight = camTransform.right;

            // Triệt tiêu độ cao Y để giữ vector di chuyển song song mặt đất
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 worldDirection = (camRight * input.x + camForward * input.y);
            return worldDirection.sqrMagnitude > 1f ? worldDirection.normalized : worldDirection;
        }
    }
}

