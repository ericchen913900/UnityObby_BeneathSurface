using UnityEngine;

namespace BeneathSurface.Stage
{
    [CreateAssetMenu(menuName = "BeneathSurface/Stage Segment Definition")]
    public class StageSegmentDefinition : ScriptableObject
    {
        [SerializeField] private string segmentName = "Segment";
        [SerializeField] private GameObject segmentPrefab;
        [SerializeField] private float segmentLength = 24f;
        [SerializeField] private int depthLevel = 0;

        public string SegmentName => segmentName;
        public GameObject SegmentPrefab => segmentPrefab;
        public float SegmentLength => segmentLength;
        public int DepthLevel => depthLevel;
    }
}
