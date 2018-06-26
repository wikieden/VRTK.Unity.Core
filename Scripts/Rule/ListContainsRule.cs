namespace VRTK.Core.Rule
{
    using UnityEngine;
    using System.Collections.Generic;

    public class ListContainsRule : MonoBehaviour, IRule
    {
        public List<Object> list = new List<Object>();

        /// <inheritdoc/>
        public bool Accepts(object target)
        {
            Object targetObject = target as Object;
            return targetObject != null && list.Contains(targetObject);
        }
    }
}