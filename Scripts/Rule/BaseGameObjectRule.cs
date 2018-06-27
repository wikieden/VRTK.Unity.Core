namespace VRTK.Core.Rule
{
    using UnityEngine;

    public abstract class BaseGameObjectRule : MonoBehaviour, IRule
    {
        /// <inheritdoc />
        public bool Accepts(object target)
        {
            GameObject targetGameObject = target as GameObject;
            if (targetGameObject == null)
            {
                Component component = target as Component;
                if (component != null)
                {
                    targetGameObject = component.gameObject;
                }
            }

            return targetGameObject != null && Accepts(targetGameObject);
        }

        protected abstract bool Accepts(GameObject targetGameObject);
    }
}