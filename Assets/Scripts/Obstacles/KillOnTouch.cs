using BeneathSurface.Core;
using BeneathSurface.Player;
using UnityEngine;

namespace BeneathSurface.Obstacles
{
    [RequireComponent(typeof(Collider2D))]
    public class KillOnTouch : MonoBehaviour
    {
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

            runController.KillPlayer();
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

        public void Configure(ObbyRunController controller)
        {
            runController = controller;
        }
    }
}
