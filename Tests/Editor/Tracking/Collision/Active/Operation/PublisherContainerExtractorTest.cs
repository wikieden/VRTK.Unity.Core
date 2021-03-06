﻿using VRTK.Core.Tracking.Collision.Active;
using VRTK.Core.Tracking.Collision.Active.Operation;

namespace Test.VRTK.Core.Tracking.Collision.Active.Operation
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;

    public class PublisherContainerExtractorTest
    {
        private GameObject containingObject;
        private PublisherContainerExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<PublisherContainerExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ExtractFromPublisher()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            GameObject publisherSource = new GameObject();
            GameObject publisherChild = new GameObject();
            publisherChild.transform.SetParent(publisherSource.transform);
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();

            publisher.sourceContainer = publisherSource;

            Assert.IsNull(subject.SourceContainer);
            Assert.IsFalse(extractedMock.Received);

            subject.Extract(publisher);

            Assert.AreEqual(publisherSource, subject.SourceContainer);
            Assert.IsTrue(extractedMock.Received);

            Object.DestroyImmediate(publisherSource);
            Object.DestroyImmediate(publisherChild);
        }

        [Test]
        public void ExtractFromConsumerEvent()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            GameObject publisherSource = new GameObject();
            GameObject publisherChild = new GameObject();
            publisherChild.transform.SetParent(publisherSource.transform);
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();

            publisher.sourceContainer = publisherSource;

            ActiveCollisionConsumer.EventData eventData = new ActiveCollisionConsumer.EventData();
            eventData.Set(publisher, null);

            Assert.IsNull(subject.SourceContainer);
            Assert.IsFalse(extractedMock.Received);

            subject.Extract(eventData);

            Assert.AreEqual(publisherSource, subject.SourceContainer);
            Assert.IsTrue(extractedMock.Received);

            Object.DestroyImmediate(publisherSource);
            Object.DestroyImmediate(publisherChild);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            GameObject publisherSource = new GameObject();
            GameObject publisherChild = new GameObject();
            publisherChild.transform.SetParent(publisherSource.transform);
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();

            publisher.sourceContainer = publisherSource;

            Assert.IsNull(subject.SourceContainer);
            Assert.IsFalse(extractedMock.Received);

            subject.gameObject.SetActive(false);
            subject.Extract(publisher);

            Assert.IsNull(subject.SourceContainer);
            Assert.IsFalse(extractedMock.Received);

            Object.DestroyImmediate(publisherSource);
            Object.DestroyImmediate(publisherChild);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            GameObject publisherSource = new GameObject();
            GameObject publisherChild = new GameObject();
            publisherChild.transform.SetParent(publisherSource.transform);
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();

            publisher.sourceContainer = publisherSource;

            Assert.IsNull(subject.SourceContainer);
            Assert.IsFalse(extractedMock.Received);

            subject.enabled = false;
            subject.Extract(publisher);

            Assert.IsNull(subject.SourceContainer);
            Assert.IsFalse(extractedMock.Received);

            Object.DestroyImmediate(publisherSource);
            Object.DestroyImmediate(publisherChild);
        }

        [Test]
        public void ExtractInvalidPublisher()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();

            Assert.IsNull(subject.SourceContainer);
            Assert.IsFalse(extractedMock.Received);

            subject.Extract(publisher);

            Assert.IsNull(subject.SourceContainer);
            Assert.IsFalse(extractedMock.Received);
        }
    }
}