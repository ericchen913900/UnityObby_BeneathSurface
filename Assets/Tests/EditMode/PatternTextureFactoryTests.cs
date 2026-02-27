#if UNITY_INCLUDE_TESTS && BENEATH_SURFACE_ENABLE_TESTS
using BeneathSurface.Visual;
using NUnit.Framework;
using UnityEngine;

namespace BeneathSurface.Tests.EditMode
{
    public class PatternTextureFactoryTests
    {
        [Test]
        public void CreateBiomeTexture_ReturnsExpectedDimensions()
        {
            var texture = PatternTextureFactory.CreateBiomeTexture(BiomeVisualType.GlowCaves, 32);
            Assert.AreEqual(32, texture.width);
            Assert.AreEqual(32, texture.height);
        }

        [Test]
        public void CreateBiomeTexture_ProducesVariationAcrossPixels()
        {
            var texture = PatternTextureFactory.CreateBiomeTexture(BiomeVisualType.CrustFault, 32);
            var a = texture.GetPixel(2, 2);
            var b = texture.GetPixel(21, 17);
            Assert.AreNotEqual(a, b);
        }
    }
}
#endif
