namespace VRTK.Core.Tracking
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using VRTK.Core.Data.Type;

    /// <summary>
    /// Allows emitting an event with <see cref="TransformData"/> for a <see cref="GameObject"/>.
    /// </summary>
    public class TransformDataEmitter : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the <see cref="TransformData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<TransformData>
        {
        }

        /// <summary>
        /// The source to create the <see cref="TransformData"/> for.
        /// </summary>
        [Tooltip("The source to create the transform data for.")]
        public GameObject source;

        /// <summary>
        /// Emitted whenever told to do so.
        /// </summary>
        public UnityEvent Called = new UnityEvent();

        /// <summary>
        /// Sets <see cref="source"/>.
        /// </summary>
        /// <param name="source">The new source to use from now on.</param>
        public void SetSource(GameObject source)
        {
            this.source = source;
        }

        /// <summary>
        /// Emits the event with the <see cref="TransformData"/> for <see cref="source"/>.
        /// </summary>
        public void EmitEvent()
        {
            Called?.Invoke(new TransformData(source));
        }
    }
}