#if UNITY_INCLUDE_TESTS && BENEATH_SURFACE_ENABLE_TESTS
using System.Collections.Generic;
using BeneathSurface.Stage;
using NUnit.Framework;
using UnityEngine;

namespace BeneathSurface.Tests.EditMode
{
    public class StageLayoutPlannerTests
    {
        [Test]
        public void BuildLayout_ReturnsRequestedCount()
        {
            var defs = new List<StageLayoutPlanner.SegmentSeed>
            {
                new StageLayoutPlanner.SegmentSeed("A", 10f, 0),
                new StageLayoutPlanner.SegmentSeed("B", 10f, 2)
            };

            var layout = StageLayoutPlanner.BuildLayout(defs, 5, Vector3.zero);

            Assert.AreEqual(5, layout.Count);
        }

        [Test]
        public void BuildLayout_AdvancesXBySegmentLength()
        {
            var defs = new List<StageLayoutPlanner.SegmentSeed>
            {
                new StageLayoutPlanner.SegmentSeed("A", 10f, 0),
                new StageLayoutPlanner.SegmentSeed("B", 20f, 0)
            };

            var layout = StageLayoutPlanner.BuildLayout(defs, 3, Vector3.zero);

            Assert.AreEqual(new Vector3(0f, 0f, 0f), layout[0].Position);
            Assert.AreEqual(new Vector3(10f, 0f, 0f), layout[1].Position);
            Assert.AreEqual(new Vector3(30f, 0f, 0f), layout[2].Position);
        }

        [Test]
        public void BuildLayout_AppliesDepthDropPerSegment()
        {
            var defs = new List<StageLayoutPlanner.SegmentSeed>
            {
                new StageLayoutPlanner.SegmentSeed("A", 10f, 2),
                new StageLayoutPlanner.SegmentSeed("B", 10f, 4)
            };

            var layout = StageLayoutPlanner.BuildLayout(defs, 3, Vector3.zero);

            Assert.AreEqual(new Vector3(0f, 0f, 0f), layout[0].Position);
            Assert.AreEqual(new Vector3(10f, -0.3f, 0f), layout[1].Position);
            Assert.AreEqual(new Vector3(20f, -0.9f, 0f), layout[2].Position);
        }
    }
}
#endif
