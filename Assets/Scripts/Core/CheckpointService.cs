using UnityEngine;

namespace BeneathSurface.Core
{
    public sealed class CheckpointService
    {
        private Vector3 _spawnPosition;
        private int _checkpointIndex;

        public CheckpointService(Vector3 initialSpawn)
        {
            _spawnPosition = initialSpawn;
            _checkpointIndex = 0;
        }

        public Vector3 CurrentSpawnPosition => _spawnPosition;
        public int CheckpointIndex => _checkpointIndex;

        public bool TrySetCheckpoint(int index, Vector3 position)
        {
            if (index <= _checkpointIndex)
            {
                return false;
            }

            _checkpointIndex = index;
            _spawnPosition = position;
            return true;
        }
    }
}
