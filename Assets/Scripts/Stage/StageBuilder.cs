using System.Collections.Generic;
using UnityEngine;

namespace BeneathSurface.Stage
{
    public class StageBuilder : MonoBehaviour
    {
        [SerializeField] private List<StageSegmentDefinition> segments = new List<StageSegmentDefinition>();
        [SerializeField] private int stageCount = 10;
        [SerializeField] private Vector3 startPosition = Vector3.zero;
        [SerializeField] private bool buildOnStart = true;

        private readonly List<GameObject> _spawned = new List<GameObject>();

        private void Start()
        {
            if (buildOnStart)
            {
                Build();
            }
        }

        [ContextMenu("Build Stage")]
        public void Build()
        {
            Clear();

            if (segments.Count == 0)
            {
                return;
            }

            var seeds = new List<StageLayoutPlanner.SegmentSeed>();
            var prefabs = new List<GameObject>();
            for (var i = 0; i < segments.Count; i++)
            {
                var def = segments[i];
                if (def == null || def.SegmentPrefab == null)
                {
                    continue;
                }

                seeds.Add(new StageLayoutPlanner.SegmentSeed(def.SegmentName, def.SegmentLength, def.DepthLevel));
                prefabs.Add(def.SegmentPrefab);
            }

            var layout = StageLayoutPlanner.BuildLayout(seeds, stageCount, startPosition);
            for (var i = 0; i < layout.Count; i++)
            {
                var prefab = prefabs[i % prefabs.Count];
                var segment = layout[i];

                var go = Instantiate(prefab, segment.Position, Quaternion.identity, transform);
                go.name = $"Stage_{segment.Index + 1:00}_{segment.Seed.Name}";
                _spawned.Add(go);
            }
        }

        [ContextMenu("Clear Stage")]
        public void Clear()
        {
            for (var i = _spawned.Count - 1; i >= 0; i--)
            {
                if (_spawned[i] != null)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(_spawned[i]);
                    }
                    else
                    {
                        DestroyImmediate(_spawned[i]);
                    }
                }
            }

            _spawned.Clear();
        }
    }
}
