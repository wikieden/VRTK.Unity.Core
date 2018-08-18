namespace VRTK.Core.Tracking.CharacterController
{
    using UnityEngine;
    using VRTK.Core.Process;

    /// <summary>
    /// Positions a <see cref="GameObject"/> based on the top of a <see cref="CharacterController"/>.
    /// </summary>
    public class CharacterControllerTopFollower : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The controller to get the top position from.
        /// </summary>
        [Tooltip("The controller to get the top position from.")]
        public CharacterController source;
        /// <summary>
        /// The target to position.
        /// </summary>
        [Tooltip("The target to position.")]
        public GameObject target;
        /// <summary>
        /// The offset from the top of <see cref="source"/> to apply.
        /// </summary>
        [Tooltip("The offset from the top of source to apply.")]
        public Vector3 offsetFromTop;

        /// <summary>
        /// Moves <see cref="target"/> to be at the top of <see cref="source"/> offset by <see cref="offsetFromTop"/>.
        /// </summary>
        public virtual void Process()
        {
            target.transform.position = source.transform.position
                + source.center
                + source.height / 2f * Vector3.up
                + offsetFromTop;
        }
    }
}