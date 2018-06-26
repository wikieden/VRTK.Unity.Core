namespace VRTK.Core.Rule
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class AnyTagRule : MonoBehaviour, IRule
    {
        public List<string> tags = new List<string>();

        /// <inheritdoc />
        public bool Accepts(object target)
        {
            GameObject targetGameObject = target as GameObject;
            return targetGameObject != null && tags.Any(targetGameObject.CompareTag);
        }
    }
}