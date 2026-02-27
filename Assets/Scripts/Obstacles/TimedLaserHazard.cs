using BeneathSurface.Core;
using UnityEngine;

namespace BeneathSurface.Obstacles
{
    [RequireComponent(typeof(Collider))]
    public class TimedLaserHazard : MonoBehaviour
    {
        [SerializeField] private ObbyRunController runController;
        [SerializeField] private float onDuration = 1.25f;
        [SerializeField] private float offDuration = 1.0f;
        [SerializeField] private Renderer visualRenderer;
        [SerializeField] private Color activeColor = new Color(1f, 0.15f, 0.15f, 1f);
        [SerializeField] private Color inactiveColor = new Color(0.35f, 0.35f, 0.35f, 1f);

        private Collider _trigger;
        private float _timer;
        private bool _isActive = true;

        private void Awake()
        {
            _trigger = GetComponent<Collider>();
            _trigger.isTrigger = true;
            _timer = onDuration;
            ApplyVisual();
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer > 0f)
            {
                return;
            }

            _isActive = !_isActive;
            _timer = _isActive ? onDuration : offDuration;
            ApplyVisual();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive || runController == null || !other.CompareTag("Player"))
            {
                return;
            }

            runController.KillPlayer();
        }

        private void ApplyVisual()
        {
            if (visualRenderer != null && visualRenderer.material != null)
            {
                visualRenderer.material.color = _isActive ? activeColor : inactiveColor;
            }

            if (_trigger != null)
            {
                _trigger.enabled = _isActive;
            }
        }

        public void Configure(ObbyRunController controller, Renderer targetRenderer)
        {
            runController = controller;
            visualRenderer = targetRenderer;
            ApplyVisual();
        }
    }
}
