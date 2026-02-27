using UnityEngine;
using BeneathSurface.Player;

namespace BeneathSurface.Core
{
    [RequireComponent(typeof(Collider2D))]
    public class CheckpointTrigger : MonoBehaviour
    {
        [SerializeField] private int checkpointIndex = 1;
        [SerializeField] private ObbyRunController runController;

        private void Reset()
        {
            var col = GetComponent<Collider2D>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (runController == null || ResolvePlayer(other) == null)
            {
                return;
            }

            runController.RegisterCheckpoint(checkpointIndex, transform.position);
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

        public void Configure(int index, ObbyRunController controller)
        {
            checkpointIndex = index;
            runController = controller;
        }
    }
}
