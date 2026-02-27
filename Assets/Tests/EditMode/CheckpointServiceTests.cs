using BeneathSurface.Core;
using NUnit.Framework;
using UnityEngine;

namespace BeneathSurface.Tests.EditMode
{
    public class CheckpointServiceTests
    {
        [Test]
        public void InitialSpawnAndIndex_AreSetFromConstructor()
        {
            var spawn = new Vector3(0f, 2f, 0f);
            var service = new CheckpointService(spawn);

            Assert.AreEqual(spawn, service.CurrentSpawnPosition);
            Assert.AreEqual(0, service.CheckpointIndex);
        }

        [Test]
        public void TrySetCheckpoint_RejectsSameOrLowerIndex()
        {
            var service = new CheckpointService(Vector3.zero);

            var accepted = service.TrySetCheckpoint(1, new Vector3(1f, 1f, 1f));
            var rejectedSame = service.TrySetCheckpoint(1, new Vector3(2f, 2f, 2f));
            var rejectedLower = service.TrySetCheckpoint(0, new Vector3(3f, 3f, 3f));

            Assert.IsTrue(accepted);
            Assert.IsFalse(rejectedSame);
            Assert.IsFalse(rejectedLower);
            Assert.AreEqual(new Vector3(1f, 1f, 1f), service.CurrentSpawnPosition);
            Assert.AreEqual(1, service.CheckpointIndex);
        }

        [Test]
        public void TrySetCheckpoint_AcceptsHigherIndex()
        {
            var service = new CheckpointService(Vector3.zero);

            var accepted1 = service.TrySetCheckpoint(1, new Vector3(1f, 0f, 0f));
            var accepted2 = service.TrySetCheckpoint(2, new Vector3(2f, 0f, 0f));

            Assert.IsTrue(accepted1);
            Assert.IsTrue(accepted2);
            Assert.AreEqual(2, service.CheckpointIndex);
            Assert.AreEqual(new Vector3(2f, 0f, 0f), service.CurrentSpawnPosition);
        }
    }
}
