using VRTK.Core.Tracking.CharacterController;

namespace Test.VRTK.Core.Tracking.CharacterController
{
    using NUnit.Framework;
    using UnityEngine;

    public class CharacterControllerTopFollowerTest
    {
        private CharacterControllerTopFollower subject;
        private GameObject target;

        [SetUp]
        public void SetUp()
        {
            subject = new GameObject().AddComponent<CharacterControllerTopFollower>();
            subject.source = subject.gameObject.AddComponent<CharacterController>();

            target = new GameObject();
            subject.target = target;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject.gameObject);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void FollowsTopWithoutOffset()
        {
            subject.source.transform.position = new Vector3(1f, 2f, 3f);
            Vector3 expected = subject.source.transform.position
                + subject.source.center
                + subject.source.height / 2f * Vector3.up;

            Assert.AreNotEqual(expected, target.transform.position);
            subject.Process();
            Assert.AreEqual(expected, target.transform.position);
        }

        [Test]
        public void FollowsTopWithOffset()
        {
            subject.source.transform.position = new Vector3(1f, 2f, 3f);
            subject.offsetFromTop = new Vector3(3f, 4f, 8f);

            Vector3 expected = subject.source.transform.position
                + subject.source.center
                + subject.source.height / 2f * Vector3.up
                + subject.offsetFromTop;

            Assert.AreNotEqual(expected, target.transform.position);
            subject.Process();
            Assert.AreEqual(expected, target.transform.position);
        }
    }
}