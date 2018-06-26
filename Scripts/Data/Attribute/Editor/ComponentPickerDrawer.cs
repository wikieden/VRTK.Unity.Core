namespace VRTK.Core.Data.Attribute
{
    using UnityEditor;
    using UnityEngine;
    using System;
    using System.Linq;
    using VRTK.Core.Utility;

    [CustomPropertyDrawer(typeof(ComponentPickerAttribute), true)]
    public class ComponentPickerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                TooltipAttribute currentTooltip = EditorHelper.GetTooltipAttribute(fieldInfo);
                label.tooltip = currentTooltip?.tooltip ?? string.Empty;

                position.height = EditorGUIUtility.singleLineHeight;

                Component targetComponent = (Component)property.objectReferenceValue;
                GameObject gameObject = targetComponent == null ? null : targetComponent.gameObject;
                Type componentType = fieldInfo.FieldType.GenericTypeArguments.SingleOrDefault() ?? fieldInfo.FieldType;
                bool isDraggingValidObject = componentType.IsInstanceOfType(DragAndDrop.objectReferences.FirstOrDefault());

                if (isDraggingValidObject && position.Contains(Event.current.mousePosition))
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
                else
                {
                    using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
                    {
                        gameObject = (GameObject)EditorGUI.ObjectField(
                            position,
                            label,
                            gameObject,
                            typeof(GameObject),
                            Selection.activeObject == null || !EditorUtility.IsPersistent(Selection.activeObject));

                        if (changeCheckScope.changed)
                        {
                            if (gameObject == null)
                            {
                                property.objectReferenceValue = null;
                            }
                            else
                            {
                                Component similarComponent = property.objectReferenceValue == null
                                    ? gameObject.transform
                                    : gameObject.GetComponents<Component>()
                                        .FirstOrDefault(
                                            component =>
                                                component.GetType().IsInstanceOfType(property.objectReferenceValue)
                                                || property.objectReferenceValue.GetType()
                                                    .IsInstanceOfType(component));
                                property.objectReferenceValue = similarComponent == null
                                    ? gameObject.transform
                                    : similarComponent;
                            }
                        }
                    }
                }

                Component[] components = gameObject == null
                    ? Array.Empty<Component>()
                    : gameObject.GetComponents(componentType);
                string[] displayedOptions = components.Select(component => component.GetType().Name)
                    .DefaultIfEmpty("None")
                    .ToArray();
                int selectedIndex = Math.Max(0, Array.IndexOf(components, (Component)property.objectReferenceValue));

                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledGroupScope(components.Length == 0))
                using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
                {
                    position.y += EditorGUIUtility.singleLineHeight;
                    selectedIndex = EditorGUI.Popup(
                        position,
                        ObjectNames.NicifyVariableName(componentType.Name),
                        selectedIndex,
                        displayedOptions);
                    if (changeCheckScope.changed)
                    {
                        property.objectReferenceValue = components[selectedIndex];
                    }
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2f;
        }
    }
}
