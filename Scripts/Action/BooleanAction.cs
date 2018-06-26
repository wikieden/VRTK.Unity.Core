namespace VRTK.Core.Action
{
    using System;
    using UnityEngine.Events;
    using VRTK.Core.Utility;

    /// <summary>
    /// Emits a <see cref="bool"/> value.
    /// </summary>
    public class BooleanAction : BaseAction<BooleanAction, bool, BooleanAction.WrappedEvent, BooleanAction.Event>
    {
        /// <summary>
        /// Defines the wrapped event with the <see cref="bool"/> state.
        /// </summary>
        [Serializable]
        public class WrappedEvent : WrappedUnityEvent<bool, Event>
        {
        }

        /// <summary>
        /// Defines the event with the <see cref="bool"/> state.
        /// </summary>
        [Serializable]
        public class Event : UnityEvent<bool>
        {
        }
    }
}