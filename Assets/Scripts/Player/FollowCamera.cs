using UnityEngine;

namespace BeneathSurface.Player
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector2 offset = new Vector2(3f, 2.4f);
        [SerializeField] private float fixedZ = -10f;
        [SerializeField] private bool forceOrthographic = true;
        [SerializeField] private float orthographicSize = 5.6f;
        [SerializeField] private float smoothTime = 0.15f;

        private Vector3 _velocity;

        private void Awake()
        {
            if (!forceOrthographic)
            {
                return;
            }

            var cam = GetComponent<Camera>();
            if (cam != null)
            {
                cam.orthographic = true;
                cam.orthographicSize = orthographicSize;
            }
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            var desired = new Vector3(target.position.x + offset.x, target.position.y + offset.y, fixedZ);
            transform.position = Vector3.SmoothDamp(transform.position, desired, ref _velocity, smoothTime);
        }

        public void Configure(Transform followTarget)
        {
            target = followTarget;
        }
    }
}
