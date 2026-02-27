namespace BeneathSurface.Core
{
    public sealed class RunProgressService
    {
        private readonly int _targetCheckpoint;
        private int _maxReached;

        public RunProgressService(int targetCheckpoint)
        {
            _targetCheckpoint = targetCheckpoint < 1 ? 1 : targetCheckpoint;
            _maxReached = 0;
        }

        public int TargetCheckpoint => _targetCheckpoint;
        public int MaxReached => _maxReached;
        public bool IsFinished => _maxReached >= _targetCheckpoint;

        public bool RegisterCheckpoint(int checkpointIndex)
        {
            if (checkpointIndex <= _maxReached)
            {
                return false;
            }

            _maxReached = checkpointIndex;
            return true;
        }

        public float Progress01()
        {
            var ratio = (float)_maxReached / _targetCheckpoint;
            if (ratio < 0f)
            {
                return 0f;
            }

            if (ratio > 1f)
            {
                return 1f;
            }

            return ratio;
        }
    }
}
