namespace VRTK.Core.Rule
{
    using UnityEngine;

    public class AnyLayerRule : BaseGameObjectRule
    {
        public LayerMask layerMask;

        /// <inheritdoc />
        protected override bool Accepts(GameObject targetGameObject)
        {
            return (targetGameObject.layer & layerMask.value) != 0;
        }
    }
}