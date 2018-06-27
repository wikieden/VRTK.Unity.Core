namespace VRTK.Core.Rule
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;

    public class AnyTagRule : BaseGameObjectRule
    {
        public List<string> tags = new List<string>();

        /// <inheritdoc />
        protected override bool Accepts(GameObject targetGameObject)
        {
            return tags.Any(targetGameObject.CompareTag);
        }
    }
}