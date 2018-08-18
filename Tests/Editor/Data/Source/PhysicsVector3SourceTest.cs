using VRTK.Core.Data.Source;

namespace Test.VRTK.Core.Data.Source
{
    using NUnit.Framework;
    using UnityEngine;

    public class PhysicsVector3SourceTest
    {
        private PhysicsVector3Source subject;

        [SetUp]
        public void SetUp()
        {
            subject = new GameObject().AddComponent<PhysicsVector3Source>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject.gameObject);
        }

        [Test]
        public void ReturnsGravityVector()
        {
            Vector3 expected = new Vector3(1f, 2f, 3f);

            Vector3 previousGravity = Physics.gravity;
            Physics.gravity = expected;

            try
            {
                Assert.AreEqual(expected, subject.Vector);
            }
            finally
            {
                Physics.gravity = previousGravity;
            }
        }
    }
}