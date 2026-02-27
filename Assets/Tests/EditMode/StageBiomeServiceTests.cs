#if UNITY_INCLUDE_TESTS && BENEATH_SURFACE_ENABLE_TESTS
using BeneathSurface.Core;
using NUnit.Framework;

namespace BeneathSurface.Tests.EditMode
{
    public class StageBiomeServiceTests
    {
        [Test]
        public void GetBiomeLabel_UsesGlowCavesInEarlyRun()
        {
            Assert.AreEqual("Glow Caves", StageBiomeService.GetBiomeLabel(0, 10));
            Assert.AreEqual("Glow Caves", StageBiomeService.GetBiomeLabel(3, 10));
        }

        [Test]
        public void GetBiomeLabel_UsesVerticalRuinsInMidRun()
        {
            Assert.AreEqual("Vertical Ruins", StageBiomeService.GetBiomeLabel(4, 10));
            Assert.AreEqual("Vertical Ruins", StageBiomeService.GetBiomeLabel(7, 10));
        }

        [Test]
        public void GetBiomeLabel_UsesCrustFaultNearSurface()
        {
            Assert.AreEqual("Crust Fault", StageBiomeService.GetBiomeLabel(8, 10));
            Assert.AreEqual("Crust Fault", StageBiomeService.GetBiomeLabel(9, 10));
        }

        [Test]
        public void GetBiomeLabel_UsesSurfaceBreakWhenComplete()
        {
            Assert.AreEqual("Surface Break", StageBiomeService.GetBiomeLabel(10, 10));
            Assert.AreEqual("Surface Break", StageBiomeService.GetBiomeLabel(14, 10));
        }

        [Test]
        public void GetBiomeKind_ReturnsExpectedBuckets()
        {
            Assert.AreEqual(StageBiomeService.BiomeKind.GlowCaves, StageBiomeService.GetBiomeKind(1, 10));
            Assert.AreEqual(StageBiomeService.BiomeKind.VerticalRuins, StageBiomeService.GetBiomeKind(5, 10));
            Assert.AreEqual(StageBiomeService.BiomeKind.CrustFault, StageBiomeService.GetBiomeKind(9, 10));
            Assert.AreEqual(StageBiomeService.BiomeKind.SurfaceBreak, StageBiomeService.GetBiomeKind(10, 10));
        }
    }
}
#endif
