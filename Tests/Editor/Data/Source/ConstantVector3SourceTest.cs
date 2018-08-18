using VRTK.Core.Data.Source;

namespace Test.VRTK.Core.Data.Source
{
    using NUnit.Framework;
    using UnityEngine;

    public class ConstantVector3SourceTest
    {
        private ConstantVector3Source subject;

        [SetUp]
        public void SetUp()
        {
            subject = new GameObject().AddComponent<ConstantVector3Source>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject.gameObject);
        }

        [Test]
        public void ReturnsSetVector()
        {
            Vector3 expected = new Vector3(1f, 2f, 3f);
            subject.vector = expected;

            Assert.AreEqual(expected, subject.Vector);
        }
    }
}