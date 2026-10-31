using HuyetNguyetDaiNgan.Combat;
using UnityEngine;

namespace HuyetNguyetDaiNgan.UI
{
    /// <summary>
    /// Thanh máu World Space lơ lửng trên đầu đối tượng (Billboard luôn quay về Camera).
    /// </summary>
    public class WorldSpaceHealthBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private HealthComponent targetHealth;
        [SerializeField] private Transform fillTransform;
        [SerializeField] private Vector3 offset = new Vector3(0f, 2.2f, 0f);

        private Camera _mainCamera;
        private Transform _targetTransform;

        private void Awake()
        {
            if (targetHealth == null)
            {
                targetHealth = GetComponentInParent<HealthComponent>();
            }

            if (targetHealth != null)
            {
                _targetTransform = targetHealth.transform;
            }

            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            if (targetHealth != null)
            {
                targetHealth.OnDamaged += OnHealthChanged;
                targetHealth.OnHealed += _ => UpdateBar();
                targetHealth.OnDied += HandleDeath;
            }
        }

        private void OnDisable()
        {
            if (targetHealth != null)
            {
                targetHealth.OnDamaged -= OnHealthChanged;
                targetHealth.OnDied -= HandleDeath;
            }
        }

        private void Start()
        {
            UpdateBar();
        }

        private void LateUpdate()
        {
            if (_targetTransform == null) return;

            // Đặt vị trí trên đầu mục tiêu
            transform.position = _targetTransform.position + offset;

            // Billboard quay mặt về phía Camera
            if (_mainCamera != null)
            {
                transform.rotation = _mainCamera.transform.rotation;
            }
            else
            {
                _mainCamera = Camera.main;
            }
        }

        private void OnHealthChanged(DamageInfo info)
        {
            UpdateBar();
        }

        private void UpdateBar()
        {
            if (fillTransform == null || targetHealth == null) return;

            float percent = targetHealth.HealthPercent;
            Vector3 scale = fillTransform.localScale;
            scale.x = Mathf.Clamp01(percent);
            fillTransform.localScale = scale;
        }

        private void HandleDeath()
        {
            gameObject.SetActive(false);
        }
    }
}

