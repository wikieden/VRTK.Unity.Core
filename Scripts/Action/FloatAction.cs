namespace VRTK.Core.Action
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Extension;
    using VRTK.Core.Utility;

    /// <summary>
    /// Emits a <see cref="float"/> value.
    /// </summary>
    public class FloatAction : BaseAction<FloatAction, float, FloatAction.WrappedEvent, FloatAction.Event>
    {
        /// <summary>
        /// Defines the wrapped event with the <see cref="float"/> state.
        /// </summary>
        [Serializable]
        public class WrappedEvent : WrappedUnityEvent<float, Event>
        {
        }

        /// <summary>
        /// Defines the event with the <see cref="float"/> state.
        /// </summary>
        [Serializable]
        public class Event : UnityEvent<float>
        {
        }

        /// <summary>
        /// The tolerance of equality between two <see cref="float"/> values.
        /// </summary>
        [Tooltip("The tolerance of equality between two float values.")]
        public float equalityTolerance = float.Epsilon;

        /// <inheritdoc />
        protected override bool IsValueEqual(float value)
        {
            return Value.ApproxEquals(value, equalityTolerance);
        }

        /// <inheritdoc />
        protected override bool ShouldActivate(float value)
        {
            return !defaultValue.ApproxEquals(value, equalityTolerance);
        }
    }
}