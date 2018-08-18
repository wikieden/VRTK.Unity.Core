namespace VRTK.Core.Tracking.CharacterController
{
    using UnityEngine;
    using VRTK.Core.Process;

    /// <summary>
    /// Sets both the height and center of a <see cref="CharacterController"/> to match a source <see cref="GameObject"/>.
    /// </summary>
    public class CharacterControllerHeightSetter : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Defines the height to apply.
        /// </summary>
        [Tooltip("Defines the height to apply.")]
        public GameObject source;
        /// <summary>
        /// The controller to adjust.
        /// </summary>
        [Tooltip("The controller to adjust.")]
        public CharacterController target;
        /// <summary>
        /// An optional offset for the <see cref="source"/> to use when determining the height to apply.
        /// </summary>
        [Tooltip("An optional offset for the source to use when determining the height to apply.")]
        public GameObject offset;

        /// <summary>
        /// Sets the height and center of <see cref="target"/> based on the position of <see cref="source"/>.
        /// </summary>
        public virtual void Process()
        {
            Vector3 sourcePosition = source.transform.position;
            Vector3 relativeSourcePosition =
                offset == null ? sourcePosition : offset.transform.InverseTransformPoint(sourcePosition);
            target.height = Mathf.Max(0f, relativeSourcePosition.y + target.radius - target.skinWidth);

            Vector3 center = target.center;
            center.y = target.height / 2f;
            target.center = center;
        }
    }
}