using VRTK.Core.Tracking.CharacterController;
using VRTK.Core.Data.Source;

namespace Test.VRTK.Core.Tracking.CharacterController
{
    using NUnit.Framework;
    using UnityEngine;

    public class CharacterControllerMoverTest
    {
        private CharacterControllerMover subject;
        private GameObject source;
        private GameObject target;

        [SetUp]
        public void SetUp()
        {
            subject = new GameObject().AddComponent<CharacterControllerMover>();
            subject.characterController = subject.gameObject.AddComponent<CharacterController>();

            source = new GameObject();
            subject.source = source;

            target = new GameObject();
            subject.target = target;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject.gameObject);
            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void StaysIfNoMovement()
        {
            Vector3 targetPosition = Vector3.zero;
            source.transform.position = targetPosition;
            target.transform.position = targetPosition;
            subject.characterController.transform.position = targetPosition;

            Assert.AreEqual(targetPosition, subject.characterController.transform.position);
            subject.Process();
            Assert.AreEqual(targetPosition, subject.characterController.transform.position);
        }

        [Test]
        public void MovesForMovementWithoutGravity()
        {
            Vector3 sourcePosition = Vector3.zero;
            Vector3 targetPosition = Vector3.one;

            source.transform.position = sourcePosition;
            target.transform.position = targetPosition;
            subject.characterController.transform.position = sourcePosition;

            Assert.AreEqual(sourcePosition, subject.characterController.transform.position);
            Assert.AreNotEqual(targetPosition, subject.characterController.transform.position);
            subject.Process();
            Assert.AreEqual(targetPosition, subject.characterController.transform.position);
        }

        [Test]
        public void MovesForMovementWithGravity()
        {
            Vector3 sourcePosition = Vector3.zero;
            Vector3 targetPosition = Vector3.one;

            source.transform.position = sourcePosition;
            target.transform.position = targetPosition;
            subject.characterController.transform.position = sourcePosition;

            ConstantVector3Source gravitySource = subject.gameObject.AddComponent<ConstantVector3Source>();
            gravitySource.vector = new Vector3(1f, 2f, 3f);
            subject.gravitySource = new Vector3SourceContainer
            {
                Interface = gravitySource
            };

            Assert.AreEqual(sourcePosition, subject.characterController.transform.position);
            Assert.AreNotEqual(targetPosition, subject.characterController.transform.position);
            subject.Process();
            Assert.AreEqual(targetPosition + gravitySource.vector, subject.characterController.transform.position);
        }

        [Test]
        public void MovesForMovementWithoutGravityButWithGravitySource()
        {
            Vector3 sourcePosition = Vector3.zero;
            Vector3 targetPosition = Vector3.one;

            source.transform.position = sourcePosition;
            target.transform.position = targetPosition;
            subject.characterController.transform.position = sourcePosition;

            ConstantVector3Source gravitySource = subject.gameObject.AddComponent<ConstantVector3Source>();
            gravitySource.vector = new Vector3(1f, 2f, 3f);
            subject.gravitySource = new Vector3SourceContainer
            {
                Interface = gravitySource
            };

            subject.applyGravity = false;

            Assert.AreEqual(sourcePosition, subject.characterController.transform.position);
            Assert.AreNotEqual(targetPosition, subject.characterController.transform.position);
            subject.Process();
            Assert.AreEqual(targetPosition, subject.characterController.transform.position);
        }
    }
}