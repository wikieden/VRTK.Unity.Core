namespace VRTK.Core.Utility
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using VRTK.Core.Data.Attribute;

    public class UnityEventForwarder : MonoBehaviour
    {
        [Serializable]
        protected class TargetInfo
        {
            public string name;
            public object value;
            public Type type;
            public MethodInfo methodInfo;
        }

        [ComponentPicker]
        public Component source;

        public ExclusionRule targetValidity;

        public IReadOnlyList<Component> Targets
        {
            get
            {
                return targets;
            }
            set
            {
                targets.Clear();
                targets.AddRange(value);
                CacheTargetInfos();
            }
        }

        [SerializeField]
        [ComponentPicker]
        protected List<Component> targets = new List<Component>();

        protected Dictionary<FieldInfo, Delegate> handlers = new Dictionary<FieldInfo, Delegate>();
        protected List<TargetInfo> targetInfos = new List<TargetInfo>();

        protected virtual void OnEnable()
        {
            CreateAndAddHandlers();
            CacheTargetInfos();
        }

        protected virtual void OnDisable()
        {
            RemoveAndClearHandlers();
            targetInfos.Clear();
        }

        protected virtual void CreateAndAddHandlers()
        {
            RemoveAndClearHandlers();

            FieldInfo[] fieldInfos = source
                .GetType()
                .GetFields()
                .Where(info => typeof(UnityEventBase).IsAssignableFrom(info.FieldType))
                .ToArray();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                ParameterExpression[] parameterExpressions = fieldInfo.FieldType
                    .GetMethod(nameof(UnityEvent.Invoke))
                    .GetParameters()
                    .Select(info => Expression.Parameter(info.ParameterType))
                    .ToArray();
                NewArrayExpression newArrayExpression = Expression.NewArrayInit(
                    typeof(object),
                    parameterExpressions.Select(expression => Expression.TypeAs(expression, typeof(object))));
                MethodCallExpression methodCallExpression = Expression.Call(
                    Expression.Constant(this),
                    nameof(Forward),
                    null,
                    Expression.Constant(fieldInfo),
                    newArrayExpression);
                MethodInfo addListenerMethodInfo = fieldInfo.FieldType.GetMethod(nameof(UnityEvent.AddListener));
                Delegate handler = Expression.Lambda(
                        addListenerMethodInfo.GetParameters().Single().ParameterType,
                        methodCallExpression,
                        parameterExpressions)
                    .Compile();

                addListenerMethodInfo.Invoke(
                    fieldInfo.GetValue(source),
                    new object[]
                    {
                        handler
                    });
                handlers[fieldInfo] = handler;
            }
        }

        protected virtual void RemoveAndClearHandlers()
        {
            foreach (KeyValuePair<FieldInfo, Delegate> pair in handlers)
            {
                MethodInfo removeListenerMethodInfo = pair.Key.FieldType.GetMethod(nameof(UnityEvent.RemoveListener));
                removeListenerMethodInfo?.Invoke(
                    pair.Key.GetValue(source),
                    new object[]
                    {
                        pair.Value
                    });
            }

            handlers.Clear();
        }

        protected virtual void CacheTargetInfos()
        {
            targetInfos.Clear();

            foreach (Component target in targets.Where(component => !ExclusionRule.ShouldExclude(component.gameObject, targetValidity)))
            {
                foreach (FieldInfo fieldInfo in target.GetType().GetFields())
                {
                    if (!typeof(UnityEventBase).IsAssignableFrom(fieldInfo.FieldType))
                    {
                        continue;
                    }

                    const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
                    MethodInfo invokeMethod = fieldInfo.FieldType.GetMethod(nameof(UnityEvent.Invoke), bindingFlags);

                    targetInfos.Add(
                        new TargetInfo
                        {
                            name = fieldInfo.Name,
                            value = fieldInfo.GetValue(target),
                            type = fieldInfo.FieldType,
                            methodInfo = invokeMethod
                        });
                }
            }
        }

        protected virtual void Forward(FieldInfo unityEventFieldInfo, object[] arguments)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            foreach (TargetInfo targetInfo in targetInfos)
            {
                if (targetInfo.name != unityEventFieldInfo.Name
                    || !unityEventFieldInfo.FieldType.IsAssignableFrom(targetInfo.type))
                {
                    continue;
                }

                targetInfo.methodInfo.Invoke(targetInfo.value, arguments);
            }
        }
    }
}
