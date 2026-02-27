#if UNITY_INCLUDE_TESTS && BENEATH_SURFACE_ENABLE_TESTS
using BeneathSurface.Core;
using NUnit.Framework;

namespace BeneathSurface.Tests.EditMode
{
    public class RunProgressServiceTests
    {
        [Test]
        public void Progress_IsClampedBetweenZeroAndOne()
        {
            var service = new RunProgressService(10);
            Assert.AreEqual(0f, service.Progress01());

            service.RegisterCheckpoint(15);
            Assert.AreEqual(1f, service.Progress01());
        }

        [Test]
        public void RegisterCheckpoint_OnlyAcceptsHigherValues()
        {
            var service = new RunProgressService(5);

            Assert.IsTrue(service.RegisterCheckpoint(1));
            Assert.IsFalse(service.RegisterCheckpoint(1));
            Assert.IsFalse(service.RegisterCheckpoint(0));
            Assert.AreEqual(1, service.MaxReached);
        }

        [Test]
        public void IsFinished_BecomesTrueAtTargetCheckpoint()
        {
            var service = new RunProgressService(3);
            service.RegisterCheckpoint(1);
            Assert.IsFalse(service.IsFinished);

            service.RegisterCheckpoint(3);
            Assert.IsTrue(service.IsFinished);
        }
    }
}
#endif
