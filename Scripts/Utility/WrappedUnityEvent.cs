namespace VRTK.Core.Utility
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;
    using VRTK.Core.Rule;

    public abstract class WrappedUnityEvent
    {
    }

    public abstract class WrappedUnityEvent<TValue, TEvent> : WrappedUnityEvent where TEvent : UnityEvent<TValue>, new()
    {
        private sealed class TargetActionPair
        {
            public readonly object target;
            public readonly UnityAction<TValue> action;

            public TargetActionPair(object target, UnityAction<TValue> action)
            {
                this.target = target;
                this.action = action;
            }
        }

        [SerializeField]
        private TEvent wrappedEvent = new TEvent();

        private readonly List<TargetActionPair> listenersWithTargets = new List<TargetActionPair>();

        public virtual void AddListener(object target, UnityAction<TValue> action)
        {
            listenersWithTargets.Add(new TargetActionPair(target, action));
        }

        public virtual void RemoveListener(UnityAction<TValue> action)
        {
            listenersWithTargets.RemoveAll(pair => pair.action == action);
        }

        protected internal virtual void Invoke(TValue argument, IRule rule = null)
        {
            List<int> bla = new List<int>();

            for (int index = 0; index < wrappedEvent.GetPersistentEventCount(); index++)
            {
                Object target = wrappedEvent.GetPersistentTarget(index);
                if (target != null && rule?.Accepts(target) != false)
                {
                    continue;
                }

                bla.Add(index);
                wrappedEvent.SetPersistentListenerState(index, UnityEventCallState.Off);
            }

            wrappedEvent.Invoke(argument);

            foreach (int index in bla)
            {
                wrappedEvent.SetPersistentListenerState(index, UnityEventCallState.RuntimeOnly);
            }

            foreach (TargetActionPair pair in listenersWithTargets)
            {
                if (rule?.Accepts(pair.target) != false)
                {
                    pair.action(argument);
                }
            }
        }
    }
}