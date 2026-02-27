using UnityEngine;

namespace BeneathSurface.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private float jumpHeight = 1.8f;
        [SerializeField] private float gravity = -20f;

        private CharacterController _controller;
        private Vector3 _velocity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            var direction = transform.right * input.x + transform.forward * input.y;
            _controller.Move(direction.normalized * (moveSpeed * Time.deltaTime));

            if (_controller.isGrounded && _velocity.y < 0f)
            {
                _velocity.y = -2f;
            }

            if (_controller.isGrounded && Input.GetButtonDown("Jump"))
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }

        public void SnapTo(Vector3 worldPosition)
        {
            _controller.enabled = false;
            transform.position = worldPosition;
            _velocity = Vector3.zero;
            _controller.enabled = true;
        }
    }
}
