namespace VRTK.Core.Rule
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;
    using VRTK.Core.Extension;

    public class CompoundAndRule : MonoBehaviour, IRule
    {
        public List<RuleContainer> rules = new List<RuleContainer>();

        /// <inheritdoc/>
        public bool Accepts(object target)
        {
            return rules.EmptyIfNull().All(rule => rule.Interface.Accepts(target));
        }
    }
}
