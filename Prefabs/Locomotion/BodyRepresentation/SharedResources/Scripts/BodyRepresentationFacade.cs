namespace VRTK.Core.Prefabs.Locomotion.BodyRepresentation
{
    using UnityEngine;
    using VRTK.Core.Prefabs.Locomotion.Teleporters;

    /// <summary>
    /// The public interface for the BodyRepresentation prefab.
    /// </summary>
    public class BodyRepresentationFacade : MonoBehaviour
    {
        /// <summary>
        /// The alias for the CameraRig Play Area.
        /// </summary>
        [Tooltip("The alias for the CameraRig Play Area.")]
        public GameObject playAreaAlias;
        /// <summary>
        /// The alias for the CameraRig Headset.
        /// </summary>
        [Tooltip("The alias for the CameraRig Headset.")]
        public GameObject headsetAlias;
        /// <summary>
        /// An optional teleporter to use.
        /// </summary>
        [Tooltip("An optional teleporter to use.")]
        public TeleporterFacade teleporterFacade;
    }
}