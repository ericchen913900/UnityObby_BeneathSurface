using UnityEngine;

namespace BeneathSurface.Player
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 4f, -6f);
        [SerializeField] private float smoothTime = 0.15f;

        private Vector3 _velocity;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            var desired = target.position + target.TransformDirection(offset);
            transform.position = Vector3.SmoothDamp(transform.position, desired, ref _velocity, smoothTime);
            transform.LookAt(target.position + Vector3.up * 1.2f);
        }

        public void Configure(Transform followTarget)
        {
            target = followTarget;
        }
    }
}
