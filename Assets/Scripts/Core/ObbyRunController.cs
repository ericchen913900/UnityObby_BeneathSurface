using BeneathSurface.Player;
using UnityEngine;

namespace BeneathSurface.Core
{
    public class ObbyRunController : MonoBehaviour
    {
        [SerializeField] private Transform initialSpawn;
        [SerializeField] private RespawnablePlayer player;
        [SerializeField] private int finishCheckpoint = 10;

        private CheckpointService _checkpointService;
        private RunProgressService _progressService;

        private void Awake()
        {
            var spawn = initialSpawn != null ? initialSpawn.position : Vector3.zero;
            _checkpointService = new CheckpointService(spawn);
            _progressService = new RunProgressService(finishCheckpoint);
        }

        public void Configure(Transform spawnPoint, RespawnablePlayer targetPlayer)
        {
            initialSpawn = spawnPoint;
            player = targetPlayer;
            _checkpointService = new CheckpointService(initialSpawn != null ? initialSpawn.position : Vector3.zero);
            _progressService = new RunProgressService(finishCheckpoint);
        }

        public void RegisterCheckpoint(int checkpointIndex, Vector3 checkpointPosition)
        {
            if (_checkpointService.TrySetCheckpoint(checkpointIndex, checkpointPosition))
            {
                _progressService.RegisterCheckpoint(checkpointIndex);
            }
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

        public float GetProgress01()
        {
            return _progressService.Progress01();
        }

        public bool HasFinished()
        {
            return _progressService.IsFinished;
        }

        public int GetFinishCheckpoint()
        {
            return _progressService.TargetCheckpoint;
        }
    }
}
