namespace VRTK.Core.Tracking.Follow
{
    using UnityEngine;
    using UnityEngine.Events;
    using VRTK.Core.Data.Source;
    using VRTK.Core.Process;

    /// <summary>
    /// Tracks movement of a <see cref="GameObject"/> along a direction.
    /// </summary>
    public class DirectionalMovementTracker : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The source of the movement to track.
        /// </summary>
        [Tooltip("The source of the movement to track.")]
        public GameObject source;
        /// <summary>
        /// Defines the direction to check the movement against.
        /// </summary>
        [Tooltip("Defines the direction to check the movement against.")]
        public Vector3SourceContainer directionSource;
        /// <summary>
        /// A threshold in the range of 0f to 1f to apply when checking for movement along the direction. 1f means the movement has to match the direction exactly, 0f accepts any movement.
        /// </summary>
        [Tooltip("A threshold in the range of 0f to 1f to apply when checking for movement along the direction. 1f means the movement has to match the direction exactly, 0f accepts any movement.")]
        public float directionalSimilarityThreshold = 0.1f;

        /// <summary>
        /// Emitted when <see cref="source"/> starts moving along the direction of <see cref="directionSource"/>.
        /// </summary>
        public UnityEvent StartedMovingAlongDirection = new UnityEvent();
        /// <summary>
        /// Emitted when <see cref="source"/> stops moving along the direction of <see cref="directionSource"/>.
        /// </summary>
        public UnityEvent StoppedMovingAlongDirection = new UnityEvent();

        public bool IsMovingAlongDirection
        {
            get;
            protected set;
        }

        protected Vector3? previousPosition;

        /// <summary>
        /// Checks for movement along the direction and emits events when a change occurs.
        /// </summary>
        public virtual void Process()
        {
            if (CheckForMovementAlongDirection())
            {
                EmitMovementAlongDirectionEvent();
            }

            previousPosition = source.transform.position;
        }

        /// <summary>
        /// Checks for movement along the direction.
        /// </summary>
        /// <returns>Whether the state of moving along the direction changed.</returns>
        protected virtual bool CheckForMovementAlongDirection()
        {
            if (previousPosition == null)
            {
                return false;
            }

            Vector3 movement = source.transform.position - previousPosition.Value;
            Vector3? direction = directionSource?.Interface?.Vector;
            bool didMoveAlongDirection = direction != null
                && Mathf.Abs(Vector3.Dot(direction.Value.normalized, movement.normalized))
                >= directionalSimilarityThreshold;
            if (didMoveAlongDirection == IsMovingAlongDirection)
            {
                return false;
            }

            IsMovingAlongDirection = didMoveAlongDirection;
            return true;
        }

        /// <summary>
        /// Emits the respective event based on the current state of moving along the direction.
        /// </summary>
        protected virtual void EmitMovementAlongDirectionEvent()
        {
            if (IsMovingAlongDirection)
            {
                StartedMovingAlongDirection?.Invoke();
            }
            else
            {
                StoppedMovingAlongDirection?.Invoke();
            }
        }
    }
}