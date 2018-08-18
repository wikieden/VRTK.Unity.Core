using VRTK.Core.Data.Source;
using VRTK.Core.Tracking.Follow;

namespace Test.VRTK.Core.Tracking.Follow
{
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;
    using UnityEngine;

    public class DirectionalMovementTrackerTest
    {
        private DirectionalMovementTracker subject;
        private GameObject source;
        private UnityEventListenerMock startedMovingAlongDirectionListenerMock;
        private UnityEventListenerMock stoppedMovingAlongDirectionListenerMock;

        [SetUp]
        public void SetUp()
        {
            subject = new GameObject().AddComponent<DirectionalMovementTracker>();

            source = new GameObject();
            subject.source = source;

            startedMovingAlongDirectionListenerMock = new UnityEventListenerMock();
            subject.StartedMovingAlongDirection.AddListener(startedMovingAlongDirectionListenerMock.Listen);

            stoppedMovingAlongDirectionListenerMock = new UnityEventListenerMock();
            subject.StoppedMovingAlongDirection.AddListener(stoppedMovingAlongDirectionListenerMock.Listen);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject.gameObject);
            Object.DestroyImmediate(source);
        }

        [Test]
        public void EmitsNothingOnFirstProcess()
        {
            Assert.IsFalse(startedMovingAlongDirectionListenerMock.Received);
            Assert.IsFalse(stoppedMovingAlongDirectionListenerMock.Received);

            subject.Process();

            Assert.IsFalse(startedMovingAlongDirectionListenerMock.Received);
            Assert.IsFalse(stoppedMovingAlongDirectionListenerMock.Received);
        }

        [Test]
        public void EmitsNothingWhenStayingStill()
        {
            Assert.IsFalse(startedMovingAlongDirectionListenerMock.Received);
            Assert.IsFalse(stoppedMovingAlongDirectionListenerMock.Received);

            subject.Process();
            subject.Process();

            Assert.IsFalse(startedMovingAlongDirectionListenerMock.Received);
            Assert.IsFalse(stoppedMovingAlongDirectionListenerMock.Received);
        }

        [Test]
        public void EmitsNothingWhenMovingInWrongDirection()
        {
            ConstantVector3Source directionSource = subject.gameObject.AddComponent<ConstantVector3Source>();
            directionSource.vector = new Vector3(0f, 1f, 0f);
            subject.directionSource = new Vector3SourceContainer
            {
                Interface = directionSource
            };

            Assert.IsFalse(startedMovingAlongDirectionListenerMock.Received);
            Assert.IsFalse(stoppedMovingAlongDirectionListenerMock.Received);

            subject.Process();
            source.transform.position += new Vector3(1f, 0f, 0f);
            subject.Process();

            Assert.IsFalse(startedMovingAlongDirectionListenerMock.Received);
            Assert.IsFalse(stoppedMovingAlongDirectionListenerMock.Received);
        }

        [Test]
        public void EmitsStartedWhenMovingAlongDirection()
        {
            ConstantVector3Source directionSource = subject.gameObject.AddComponent<ConstantVector3Source>();
            directionSource.vector = new Vector3(0f, 1f, 0f);
            subject.directionSource = new Vector3SourceContainer
            {
                Interface = directionSource
            };

            Assert.IsFalse(startedMovingAlongDirectionListenerMock.Received);
            Assert.IsFalse(stoppedMovingAlongDirectionListenerMock.Received);

            subject.Process();
            source.transform.position += new Vector3(1f, 2f, 3f);
            subject.Process();

            Assert.IsTrue(startedMovingAlongDirectionListenerMock.Received);
            Assert.IsFalse(stoppedMovingAlongDirectionListenerMock.Received);
        }

        [Test]
        public void EmitsStoppedWhenNoLongerMovingAlongDirection()
        {
            ConstantVector3Source directionSource = subject.gameObject.AddComponent<ConstantVector3Source>();
            directionSource.vector = new Vector3(0f, 1f, 0f);
            subject.directionSource = new Vector3SourceContainer
            {
                Interface = directionSource
            };

            Assert.IsFalse(startedMovingAlongDirectionListenerMock.Received);
            Assert.IsFalse(stoppedMovingAlongDirectionListenerMock.Received);

            subject.Process();
            source.transform.position += new Vector3(-1f, -2f, -3f);
            subject.Process();

            startedMovingAlongDirectionListenerMock.Reset();
            subject.Process();

            Assert.IsFalse(startedMovingAlongDirectionListenerMock.Received);
            Assert.IsTrue(stoppedMovingAlongDirectionListenerMock.Received);
        }
    }
}