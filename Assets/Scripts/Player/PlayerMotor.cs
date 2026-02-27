using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BeneathSurface.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private float jumpHeight = 1.8f;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private float groundProbeDistance = 0.08f;
        [SerializeField] private LayerMask groundMask = ~0;

        private Rigidbody2D _body;
        private Collider2D _collider;
        private float _moveInput;
        private bool _jumpQueued;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();

            _body.freezeRotation = true;
            _body.gravityScale = Mathf.Abs(gravity) / 9.81f;
        }

        private void Update()
        {
            var input = ReadMoveInput();
            _moveInput = Mathf.Clamp(input.x, -1f, 1f);

            if (ReadJumpPressed())
            {
                _jumpQueued = true;
            }
        }

        private void FixedUpdate()
        {
            var velocity = _body.velocity;
            velocity.x = _moveInput * moveSpeed;

            if (_jumpQueued && IsGrounded())
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            _body.velocity = velocity;
            _jumpQueued = false;
        }

        public void SnapTo(Vector3 worldPosition)
        {
            var target = new Vector2(worldPosition.x, worldPosition.y);
            _body.velocity = Vector2.zero;
            _body.position = target;
            transform.position = new Vector3(target.x, target.y, transform.position.z);
        }

        private bool IsGrounded()
        {
            var bounds = _collider.bounds;
            var probeCenter = new Vector2(bounds.center.x, bounds.min.y - groundProbeDistance * 0.5f);
            var probeSize = new Vector2(bounds.size.x * 0.85f, groundProbeDistance);
            var hit = Physics2D.OverlapBox(probeCenter, probeSize, 0f, groundMask);
            return hit != null && hit != _collider;
        }

        private static Vector2 ReadMoveInput()
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                var x = (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1f : 0f)
                    - (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? 1f : 0f);
                var y = (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1f : 0f)
                    - (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? 1f : 0f);
                return new Vector2(x, y);
            }
#endif
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        private static bool ReadJumpPressed()
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                return keyboard.spaceKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame;
            }
#endif
            return Input.GetButtonDown("Jump");
        }
    }
}
