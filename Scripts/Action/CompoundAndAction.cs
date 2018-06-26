namespace VRTK.Core.Action
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;
    using VRTK.Core.Extension;

    /// <summary>
    /// Emits a <see cref="bool"/> value when all given actions are in their active state.
    /// </summary>
    public class CompoundAndAction : BooleanAction
    {
        /// <summary>
        /// BaseActions to check the active state on.
        /// </summary>
        [Tooltip("BaseActions to check the active state on.")]
        public List<BaseAction> actions = new List<BaseAction>();

        private void Awake()
        {
            Activated.AddListener(
                this,
                arg0 =>
                {
                });

            //Debug.Log(string.Join("\n", Activated.ListenersWithTargets.Select(pair => pair.target)));
        }

        protected virtual void Update()
        {
            bool areAllActionsActivated = actions.EmptyIfNull().All(action => action.IsActivated);
            if (areAllActionsActivated != IsActivated)
            {
                Receive(areAllActionsActivated);
            }
        }
    }
}