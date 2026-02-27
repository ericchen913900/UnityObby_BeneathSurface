using UnityEngine;

namespace BeneathSurface.Obstacles
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = new Vector3(0f, 0f, 6f);
        [SerializeField] private float duration = 2f;

        private Vector3 _start;

        private void Awake()
        {
            _start = transform.position;
        }

        private void Update()
        {
            var t = Mathf.PingPong(Time.time / Mathf.Max(0.01f, duration), 1f);
            transform.position = Vector3.Lerp(_start, _start + offset, t);
        }
    }
}
