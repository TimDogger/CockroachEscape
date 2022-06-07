using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

namespace CockroachCore
{
    [RequireComponent(typeof(Collider2D))]
    public class Flashlight : MonoBehaviour
    {
        private float _minRadius = 0.3f;
        private float _maxRadius = 10.0f;

        private CircleCollider2D _flashlightCollider;

        [SerializeField] private Light2D _light;

        [Range(0f, 100f)] [SerializeField] private float _speed = 100f;

        [Range(.1f, .5f)] [SerializeField] private float _radiusStep = .1f;

        [Range(1f, 10f)] [SerializeField] private float _radius = 5f;

        private Vector3 _targetPos = Vector3.zero;

        private void Awake()
        {
            if (!_flashlightCollider)
            {
                _flashlightCollider = GetComponent<CircleCollider2D>();
            }
            SetRadius(_radius);
        }

        private void FixedUpdate()
        {
            float step = _speed * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, step);
        }

        private void SetRadius(float radius)
        {
            _radius = Mathf.Clamp(radius, _minRadius, _maxRadius);
            _flashlightCollider.radius = radius;
            _light.pointLightOuterRadius = _radius / 3f;
            _light.pointLightInnerRadius = _light.pointLightOuterRadius / 2f;
        }

        public void MoveFlashlight(InputAction.CallbackContext context)
        {
            Vector3 pointerPos = context.ReadValue<Vector2>();
            _targetPos = Camera.main.ScreenPointToRay(pointerPos).origin;
            _targetPos.z = 0;
        }

        public void OnScroll(InputAction.CallbackContext context)
        {
            float radiusDelta = context.ReadValue<Vector2>().y * -_radiusStep;
            if (radiusDelta != 0)
            {
                SetRadius(_radius + radiusDelta);
            }
        }
    }
}