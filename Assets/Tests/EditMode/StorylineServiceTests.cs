#if UNITY_INCLUDE_TESTS && BENEATH_SURFACE_ENABLE_TESTS
using BeneathSurface.Core;
using NUnit.Framework;

namespace BeneathSurface.Tests.EditMode
{
    public class StorylineServiceTests
    {
        [Test]
        public void GetObjectiveLine_UsesIntroBeatAtStart()
        {
            var line = StorylineService.GetObjectiveLine(0, 10, false);
            StringAssert.Contains("hollow mountain", line);
        }

        [Test]
        public void GetObjectiveLine_UsesBiomeBeatInMidRun()
        {
            var line = StorylineService.GetObjectiveLine(5, 10, false);
            StringAssert.Contains("Vertical Ruins", line);
        }

        [Test]
        public void GetObjectiveLine_UsesEscapeBeatWhenFinished()
        {
            var line = StorylineService.GetObjectiveLine(10, 10, true);
            StringAssert.Contains("surface", line);
        }
    }
}
#endif
