namespace VRTK.Core.Rule
{
    using UnityEngine;
    using System;

    public class TypeRule : MonoBehaviour, IRule
    {
        public Type type;

        /// <inheritdoc/>
        public bool Accepts(object target)
        {
            return target.GetType() == type;
        }
    }
}