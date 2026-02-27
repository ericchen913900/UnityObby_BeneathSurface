using BeneathSurface.Core;
using BeneathSurface.Player;
using UnityEngine;

namespace BeneathSurface.Obstacles
{
    [RequireComponent(typeof(Collider2D))]
    public class TimedLaserHazard : MonoBehaviour
    {
        [SerializeField] private ObbyRunController runController;
        [SerializeField] private float onDuration = 1.25f;
        [SerializeField] private float offDuration = 1.0f;
        [SerializeField] private SpriteRenderer visualRenderer;
        [SerializeField] private Color activeColor = new Color(1f, 0.15f, 0.15f, 1f);
        [SerializeField] private Color inactiveColor = new Color(0.35f, 0.35f, 0.35f, 1f);

        private Collider2D _trigger;
        private float _timer;
        private bool _isActive = true;

        private void Awake()
        {
            _trigger = GetComponent<Collider2D>();
            _trigger.isTrigger = true;
            if (visualRenderer == null)
            {
                visualRenderer = GetComponent<SpriteRenderer>();
            }

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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isActive || runController == null || ResolvePlayer(other) == null)
            {
                return;
            }

            runController.KillPlayer();
        }

        private void ApplyVisual()
        {
            if (visualRenderer != null)
            {
                visualRenderer.color = _isActive ? activeColor : inactiveColor;
            }

            if (_trigger != null)
            {
                _trigger.enabled = _isActive;
            }
        }

        public void Configure(ObbyRunController controller, SpriteRenderer targetRenderer)
        {
            runController = controller;
            visualRenderer = targetRenderer;
            ApplyVisual();
        }

        private static RespawnablePlayer ResolvePlayer(Collider2D other)
        {
            if (other == null)
            {
                return null;
            }

            var player = other.GetComponent<RespawnablePlayer>();
            if (player != null)
            {
                return player;
            }

            return other.attachedRigidbody != null
                ? other.attachedRigidbody.GetComponent<RespawnablePlayer>()
                : null;
        }
    }
}
