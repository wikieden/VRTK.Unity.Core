namespace VRTK.Core.Prefabs.Locomotion.BodyRepresentation
{
    using UnityEngine;
    using VRTK.Core.Data.Type;
    using VRTK.Core.Prefabs.Locomotion.Teleporters;
    using VRTK.Core.Tracking;
    using VRTK.Core.Tracking.CharacterController;
    using VRTK.Core.Tracking.Modification;

    /// <summary>
    /// Sets up the BodyRepresentation Prefab based on the provided user settings.
    /// </summary>
    public class BodyRepresentationInternalSetup : MonoBehaviour
    {
        [Header("Facade Settings")]

        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Tooltip("The public interface facade.")]
        public BodyRepresentationFacade facade;

        [Header("Alias Settings")]

        /// <summary>
        /// The <see cref="CharacterControllerHeightSetter"/> to set aliases on.
        /// </summary>
        [Tooltip("The Character Controller Height Setter to set aliases on.")]
        public CharacterControllerHeightSetter heightSetter;
        /// <summary>
        /// The <see cref="CharacterControllerMover"/> to set aliases on.
        /// </summary>
        [Tooltip("The Character Controller Mover to set aliases on.")]
        public CharacterControllerMover mover;
        /// <summary>
        /// The <see cref="TransformDataEmitter"/> to trigger teleportation for.
        /// </summary>
        [Tooltip("The Transform Data Emitter to trigger teleportation for.")]
        public TransformDataEmitter transformDataEmitter;

        protected virtual void OnEnable()
        {
            if (facade.teleporterFacade != null)
            {
                TeleporterInternalSetup teleporterSetup = facade.teleporterFacade.internalSetup;
                TransformPropertyApplier transformPropertyApplier = teleporterSetup.surfaceTeleporter
                    .gameObject.GetComponent<TransformPropertyApplier>();
                if (transformPropertyApplier != null)
                {
                    transformPropertyApplier.applyOffsetOnAxis = Vector3State.True;
                    teleporterSetup.modifyTeleporter = transformPropertyApplier;
                }

                foreach (SurfaceLocator surfaceLocator in teleporterSetup.surfaceLocatorAliases)
                {
                    surfaceLocator.enabled = false;
                }
            }

            heightSetter.source = facade.headsetAlias;
            heightSetter.offset = facade.playAreaAlias;

            mover.target = facade.headsetAlias;

            transformDataEmitter.Called.AddListener(Teleport);
        }

        protected virtual void OnDisable()
        {
            transformDataEmitter.Called.RemoveListener(Teleport);
        }

        /// <summary>
        /// Triggers teleportation.
        /// </summary>
        /// <param name="data"></param>
        protected virtual void Teleport(TransformData data)
        {
            if (facade.teleporterFacade != null)
            {
                facade.teleporterFacade.Teleport(data);
            }
        }
    }
}