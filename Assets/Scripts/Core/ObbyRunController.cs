using BeneathSurface.Player;
using UnityEngine;

namespace BeneathSurface.Core
{
    public class ObbyRunController : MonoBehaviour
    {
        [SerializeField] private Transform initialSpawn;
        [SerializeField] private RespawnablePlayer player;

        private CheckpointService _checkpointService;

        private void Awake()
        {
            var spawn = initialSpawn != null ? initialSpawn.position : Vector3.zero;
            _checkpointService = new CheckpointService(spawn);
        }

        public void Configure(Transform spawnPoint, RespawnablePlayer targetPlayer)
        {
            initialSpawn = spawnPoint;
            player = targetPlayer;
            _checkpointService = new CheckpointService(initialSpawn != null ? initialSpawn.position : Vector3.zero);
        }

        public void RegisterCheckpoint(int checkpointIndex, Vector3 checkpointPosition)
        {
            _checkpointService.TrySetCheckpoint(checkpointIndex, checkpointPosition);
        }

        public void KillPlayer()
        {
            if (player == null)
            {
                return;
            }

            player.RespawnAt(_checkpointService.CurrentSpawnPosition);
        }

        public int GetCurrentCheckpointIndex()
        {
            return _checkpointService.CheckpointIndex;
        }
    }
}
