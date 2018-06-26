namespace VRTK.Core.Rule
{
    using UnityEngine;

    public class AnyLayerRule : MonoBehaviour, IRule
    {
        public LayerMask layerMask;

        /// <inheritdoc />
        public bool Accepts(object target)
        {
            GameObject targetObject = target as GameObject;
            return targetObject != null && (targetObject.layer & layerMask.value) != 0;
        }
    }
}