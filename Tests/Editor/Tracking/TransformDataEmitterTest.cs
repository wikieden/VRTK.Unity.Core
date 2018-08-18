using VRTK.Core.Data.Type;
using VRTK.Core.Tracking;

namespace Test.VRTK.Core.Tracking
{
    using NUnit.Framework;
    using UnityEngine;

    public class TransformDataEmitterTest
    {
        private TransformDataEmitter subject;

        [SetUp]
        public void SetUp()
        {
            subject = new GameObject().AddComponent<TransformDataEmitter>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject.gameObject);
        }

        [Test]
        public void SetSourceValid()
        {
            GameObject source = new GameObject();

            Assert.IsNull(subject.source);
            subject.SetSource(source);
            Assert.AreEqual(source, subject.source);

            Object.DestroyImmediate(source);
        }

        [Test]
        public void SetSourceInvalid()
        {
            Assert.IsNull(subject.source);
            subject.SetSource(null);
            Assert.IsNull(subject.source);
        }

        [Test]
        public void CalledEmittedWithInvalidSource()
        {
            CalledEventListenerMock calledListenerMock = new CalledEventListenerMock();
            subject.Called.AddListener(calledListenerMock.Listen);

            Assert.IsNull(calledListenerMock.ReceivedData);
            subject.EmitEvent();
            Assert.IsNull(calledListenerMock.ReceivedData.transform);
        }

        [Test]
        public void CalledEmittedWithValidSource()
        {
            CalledEventListenerMock calledListenerMock = new CalledEventListenerMock();
            subject.Called.AddListener(calledListenerMock.Listen);

            GameObject source = new GameObject();
            subject.SetSource(source);

            Assert.IsNull(calledListenerMock.ReceivedData);
            subject.EmitEvent();
            Assert.AreEqual(source.transform, calledListenerMock.ReceivedData.transform);

            Object.DestroyImmediate(source);
        }

        private sealed class CalledEventListenerMock
        {
            public TransformData ReceivedData;

            public void Listen(TransformData data)
            {
                ReceivedData = data;
            }
        }
    }
}