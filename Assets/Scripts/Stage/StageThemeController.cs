using UnityEngine;

namespace BeneathSurface.Stage
{
    public class StageThemeController : MonoBehaviour
    {
        [System.Serializable]
        public struct DepthProfile
        {
            public int minCheckpoint;
            public Color ambientLight;
            public Color fogColor;
            public float fogDensity;
        }

        [SerializeField] private DepthProfile[] profiles;
        [SerializeField] private BeneathSurface.Core.ObbyRunController runController;

        private int _lastAppliedCheckpoint = -1;

        public void Configure(BeneathSurface.Core.ObbyRunController controller, DepthProfile[] depthProfiles)
        {
            runController = controller;
            profiles = depthProfiles;
            _lastAppliedCheckpoint = -1;
        }

        private void Update()
        {
            if (runController == null || profiles == null || profiles.Length == 0)
            {
                return;
            }

            var checkpoint = runController.GetCurrentCheckpointIndex();
            if (checkpoint == _lastAppliedCheckpoint)
            {
                return;
            }

            _lastAppliedCheckpoint = checkpoint;
            ApplyProfileFor(checkpoint);
        }

        private void ApplyProfileFor(int checkpoint)
        {
            DepthProfile selected = profiles[0];
            for (var i = 0; i < profiles.Length; i++)
            {
                if (checkpoint >= profiles[i].minCheckpoint)
                {
                    selected = profiles[i];
                }
            }

            RenderSettings.ambientLight = selected.ambientLight;
            RenderSettings.fog = true;
            RenderSettings.fogColor = selected.fogColor;
            RenderSettings.fogDensity = selected.fogDensity;
        }
    }
}
