using UnityEngine;

namespace BeneathSurface.Player
{
    [RequireComponent(typeof(PlayerMotor))]
    public class RespawnablePlayer : MonoBehaviour
    {
        [SerializeField] private float respawnHeightOffset = 0.8f;

        private PlayerMotor _motor;

        private void Awake()
        {
            _motor = GetComponent<PlayerMotor>();
        }

        public void RespawnAt(Vector3 spawnPosition)
        {
            var target = spawnPosition + Vector3.up * respawnHeightOffset;
            _motor.SnapTo(target);
        }
    }
}
