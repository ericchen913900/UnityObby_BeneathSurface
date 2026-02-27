using UnityEngine;

namespace BeneathSurface.Obstacles
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Vector2 offset = new Vector2(3f, 0f);
        [SerializeField] private float duration = 2f;

        private Vector3 _start;

        private void Awake()
        {
            _start = transform.position;
        }

        private void Update()
        {
            var t = Mathf.PingPong(Time.time / Mathf.Max(0.01f, duration), 1f);
            var delta = Vector2.Lerp(Vector2.zero, offset, t);
            transform.position = new Vector3(_start.x + delta.x, _start.y + delta.y, _start.z);
        }
    }
}
