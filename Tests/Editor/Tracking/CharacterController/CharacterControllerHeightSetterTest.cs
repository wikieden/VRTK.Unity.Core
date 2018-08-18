using VRTK.Core.Tracking.CharacterController;

namespace Test.VRTK.Core.Tracking.CharacterController
{
    using NUnit.Framework;
    using UnityEngine;

    public class CharacterControllerHeightSetterTest
    {
        private CharacterControllerHeightSetter subject;
        private GameObject source;

        [SetUp]
        public void SetUp()
        {
            subject = new GameObject().AddComponent<CharacterControllerHeightSetter>();
            subject.target = subject.gameObject.AddComponent<CharacterController>();

            source = new GameObject();
            subject.source = source;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject.gameObject);
            Object.DestroyImmediate(source);
        }

        [Test]
        public void SetsHeightAndCenterWithoutOffset()
        {
            source.transform.position = new Vector3(1f, 2f, 3f);

            const float expected = 2f;
            Assert.AreNotEqual(expected, subject.target.height - subject.target.radius + subject.target.skinWidth);

            subject.Process();

            Assert.AreEqual(expected, subject.target.height - subject.target.radius + subject.target.skinWidth);
            Assert.AreEqual(subject.target.height / 2f, subject.target.center.y);
        }

        [Test]
        public void SetsHeightAndCenterWithOffset()
        {
            subject.offset = new GameObject();
            subject.offset.transform.position = new Vector3(-1f, -2f, -3f);

            source.transform.position = new Vector3(1f, 2f, 3f);

            const float expected = 4f;
            Assert.AreNotEqual(expected, subject.target.height - subject.target.radius + subject.target.skinWidth);

            subject.Process();

            Assert.AreEqual(expected, subject.target.height - subject.target.radius + subject.target.skinWidth);
            Assert.AreEqual(subject.target.height / 2f, subject.target.center.y);
        }
    }
}