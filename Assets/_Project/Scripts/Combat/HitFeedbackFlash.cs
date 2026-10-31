using System.Collections;
using UnityEngine;

namespace HuyetNguyetDaiNgan.Combat
{
    /// <summary>
    /// Hiệu ứng thị giác chớp đỏ khi đối tượng nhận sát thương.
    /// Tự động lắng nghe sự kiện OnDamaged từ HealthComponent.
    /// </summary>
    public class HitFeedbackFlash : MonoBehaviour
    {
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private Color flashColor = new Color(1f, 0.2f, 0.2f, 1f);
        [SerializeField] private float flashDuration = 0.15f;

        private HealthComponent _healthComponent;
        private Color _originalColor;
        private Material _instancedMaterial;
        private Coroutine _flashRoutine;
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            if (_healthComponent == null)
            {
                _healthComponent = GetComponentInParent<HealthComponent>();
            }

            if (targetRenderer == null)
            {
                targetRenderer = GetComponentInChildren<Renderer>();
            }

            if (targetRenderer != null)
            {
                _instancedMaterial = targetRenderer.material; // Tạo bản sao runtime để không đổi màu prefab gốc
                if (_instancedMaterial.HasProperty(BaseColorId))
                {
                    _originalColor = _instancedMaterial.GetColor(BaseColorId);
                }
                else if (_instancedMaterial.HasProperty(ColorId))
                {
                    _originalColor = _instancedMaterial.GetColor(ColorId);
                }
            }
        }

        private void OnEnable()
        {
            if (_healthComponent != null)
            {
                _healthComponent.OnDamaged += HandleDamaged;
            }
        }

        private void OnDisable()
        {
            if (_healthComponent != null)
            {
                _healthComponent.OnDamaged -= HandleDamaged;
            }
        }

        private void HandleDamaged(DamageInfo info)
        {
            if (targetRenderer == null || _instancedMaterial == null) return;

            if (_flashRoutine != null)
            {
                StopCoroutine(_flashRoutine);
            }
            _flashRoutine = StartCoroutine(FlashCoroutine());
        }

        private IEnumerator FlashCoroutine()
        {
            SetColor(flashColor);
            yield return new WaitForSeconds(flashDuration);
            SetColor(_originalColor);
            _flashRoutine = null;
        }

        private void SetColor(Color c)
        {
            if (_instancedMaterial.HasProperty(BaseColorId))
            {
                _instancedMaterial.SetColor(BaseColorId, c);
            }
            else if (_instancedMaterial.HasProperty(ColorId))
            {
                _instancedMaterial.SetColor(ColorId, c);
            }
        }

        private void OnDestroy()
        {
            if (_instancedMaterial != null)
            {
                Destroy(_instancedMaterial);
            }
        }
    }
}

