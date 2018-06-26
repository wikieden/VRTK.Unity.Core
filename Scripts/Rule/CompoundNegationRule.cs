namespace VRTK.Core.Rule
{
    using UnityEngine;

    public class CompoundNegationRule : MonoBehaviour, IRule
    {
        public RuleContainer rule;

        /// <inheritdoc />
        public bool Accepts(object target)
        {
            return !rule.Interface.Accepts(target);
        }
    }
}