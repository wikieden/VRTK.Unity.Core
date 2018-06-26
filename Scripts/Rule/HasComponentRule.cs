namespace VRTK.Core.Rule
{
    using UnityEngine;
    using System;

    public class HasComponentRule : MonoBehaviour, IRule
    {
        public Type type;

        /// <inheritdoc/>
        public bool Accepts(object target)
        {
            GameObject targetGameObject = target as GameObject;
            return targetGameObject != null && targetGameObject.GetComponent(type) != null;
        }
    }
}