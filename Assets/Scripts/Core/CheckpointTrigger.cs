using UnityEngine;

namespace BeneathSurface.Core
{
    [RequireComponent(typeof(Collider))]
    public class CheckpointTrigger : MonoBehaviour
    {
        [SerializeField] private int checkpointIndex = 1;
        [SerializeField] private ObbyRunController runController;

        private void Reset()
        {
            var col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || runController == null)
            {
                return;
            }

            runController.RegisterCheckpoint(checkpointIndex, transform.position);
        }

        public void Configure(int index, ObbyRunController controller)
        {
            checkpointIndex = index;
            runController = controller;
        }
    }
}
