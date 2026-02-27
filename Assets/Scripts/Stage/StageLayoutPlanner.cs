using System.Collections.Generic;
using UnityEngine;

namespace BeneathSurface.Stage
{
    public static class StageLayoutPlanner
    {
        public readonly struct SegmentSeed
        {
            public SegmentSeed(string name, float length, int depthLevel)
            {
                Name = name;
                Length = Mathf.Max(1f, length);
                DepthLevel = Mathf.Max(0, depthLevel);
            }

            public string Name { get; }
            public float Length { get; }
            public int DepthLevel { get; }
        }

        public readonly struct SegmentLayout
        {
            public SegmentLayout(int index, SegmentSeed seed, Vector3 position)
            {
                Index = index;
                Seed = seed;
                Position = position;
            }

            public int Index { get; }
            public SegmentSeed Seed { get; }
            public Vector3 Position { get; }
        }

        public static List<SegmentLayout> BuildLayout(List<SegmentSeed> seeds, int stageCount, Vector3 startPosition)
        {
            var result = new List<SegmentLayout>();
            if (seeds == null || seeds.Count == 0 || stageCount <= 0)
            {
                return result;
            }

            var cursor = startPosition;
            for (var i = 0; i < stageCount; i++)
            {
                var seed = seeds[i % seeds.Count];
                result.Add(new SegmentLayout(i, seed, cursor));
                cursor += new Vector3(0f, -seed.DepthLevel * 0.15f, seed.Length);
            }

            return result;
        }
    }
}
