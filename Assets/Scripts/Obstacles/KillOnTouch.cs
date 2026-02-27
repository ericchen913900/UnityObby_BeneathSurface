using BeneathSurface.Core;
using UnityEngine;

namespace BeneathSurface.Obstacles
{
    [RequireComponent(typeof(Collider))]
    public class KillOnTouch : MonoBehaviour
    {
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

            runController.KillPlayer();
        }

        public void Configure(ObbyRunController controller)
        {
            runController = controller;
        }
    }
}
