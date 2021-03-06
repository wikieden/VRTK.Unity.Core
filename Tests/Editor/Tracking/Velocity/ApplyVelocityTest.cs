﻿using VRTK.Core.Tracking.Velocity;

namespace Test.VRTK.Core.Tracking.Velocity
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;

    public class ApplyVelocityTest
    {
        private GameObject containingObject;
        private ApplyVelocity subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ApplyVelocity>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Apply()
        {
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.source = tracker;
            subject.target = containingObject.AddComponent<Rigidbody>();
            subject.Apply();
            Assert.AreEqual(tracker.GetVelocity(), subject.target.velocity);
            Assert.AreEqual(tracker.GetAngularVelocity(), subject.target.angularVelocity);

            Object.DestroyImmediate(tracker.gameObject);
        }

        [Test]
        public void ApplyNoSource()
        {
            subject.target = containingObject.AddComponent<Rigidbody>();

            Vector3 originalVelocity = subject.target.velocity;
            Vector3 originalAngularVelocity = subject.target.angularVelocity;

            Assert.AreEqual(Vector3.zero, originalVelocity);
            Assert.AreEqual(Vector3.zero, originalAngularVelocity);

            subject.Apply();
            Assert.AreEqual(originalVelocity, subject.target.velocity);
            Assert.AreEqual(originalAngularVelocity, subject.target.angularVelocity);
        }
    }
}