namespace VRTK.Core.Tracking.CharacterController
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Data.Source;
    using VRTK.Core.Process;

    /// <summary>
    /// Moves a <see cref="CharacterController"/> by following the movement of two <see cref="GameObject"/>s.
    /// </summary>
    public class CharacterControllerMover : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The collision happening during a move of a <see cref="CharacterController"/>.
        /// </summary>
        [Flags]
        public enum CollisionFlags
        {
            /// <summary>
            /// No collision occured.
            /// </summary>
            None = 1 << 0,
            /// <summary>
            /// A collision with the sides occured.
            /// </summary>
            Sides = 1 << 1,
            /// <summary>
            /// A collision with the top occured.
            /// </summary>
            Above = 1 << 2,
            /// <summary>
            /// A collision with the bottom occured.
            /// </summary>
            Below = 1 << 3
        }

        /// <summary>
        /// Defines the event with the <see cref="CollisionFlags"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<CollisionFlags>
        {
        }

        /// <summary>
        /// The controller to move.
        /// </summary>
        [Tooltip("The controller to move.")]
        public CharacterController characterController;
        /// <summary>
        /// The source of the movement motion.
        /// </summary>
        [Tooltip("The source of the movement motion.")]
        public GameObject source;
        /// <summary>
        /// The target of the movement motion.
        /// </summary>
        [Tooltip("The target of the movement motion.")]
        public GameObject target;

        /// <summary>
        /// An optional source of gravity to apply when moving <see cref="characterController"/>.
        /// </summary>
        [Tooltip("An optional source of gravity to apply when moving character controller.")]
        public Vector3SourceContainer gravitySource;
        /// <summary>
        /// Whether to apply <see cref="gravitySource"/> to the movement motion.
        /// </summary>
        [Tooltip("Whether to apply gravity source to the movement motion.")]
        public bool applyGravity = true;
        /// <summary>
        /// Whether to apply <see cref="gravitySource"/> to the movement motion.
        /// </summary>
        [UnityFlags]
        [Tooltip("Whether to apply gravity source to the movement motion.")]
        public CollisionFlags collisionsToIgnore = CollisionFlags.None | CollisionFlags.Below;

        /// <summary>
        /// Emitted whenever a move resulted in a collision.
        /// </summary>
        public UnityEvent CollisionStarted = new UnityEvent();
        /// <summary>
        /// Emitted whenever a move no longer results in a collision.
        /// </summary>
        public UnityEvent CollisionStopped = new UnityEvent();

        public bool IsColliding
        {
            get;
            protected set;
        }
        public CollisionFlags LastMovementCollisionFlags
        {
            get;
            protected set;
        } = CollisionFlags.None;

        /// <summary>
        /// Moves <see cref="characterController"/> based on the movement of <see cref="source"/> to <see cref="target"/>.
        /// </summary>
        public virtual void Process()
        {
            Vector3 motion = target.transform.position - source.transform.position;
            if (applyGravity)
            {
                motion += gravitySource?.Interface?.Vector ?? Vector3.zero;
            }

            CheckForCollisions(characterController.Move(motion));
        }

        /// <summary>
        /// Checks whether a collision happened based on the given flags and updates the state.
        /// </summary>
        /// <param name="flags">The flags to check against.</param>
        protected virtual void CheckForCollisions(UnityEngine.CollisionFlags flags)
        {
            LastMovementCollisionFlags = flags == UnityEngine.CollisionFlags.None
                ? CollisionFlags.None
                : (CollisionFlags)((int)flags << 1);
            bool isColliding = (LastMovementCollisionFlags & ~collisionsToIgnore) != 0;
            if (isColliding == IsColliding)
            {
                return;
            }

            IsColliding = isColliding;
            EmitCollisionEvent();
        }

        /// <summary>
        /// Emits the respective collision event for the current state.
        /// </summary>
        protected virtual void EmitCollisionEvent()
        {
            if (IsColliding)
            {
                CollisionStarted?.Invoke(LastMovementCollisionFlags);
            }
            else
            {
                CollisionStopped?.Invoke(LastMovementCollisionFlags);
            }
        }
    }
}