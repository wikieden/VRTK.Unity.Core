namespace VRTK.Core.Utility
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(WrappedUnityEvent), true)]
    public class WrappedUnityEventDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.tooltip = EditorHelper.GetTooltipAttribute(fieldInfo)?.tooltip ?? string.Empty;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("wrappedEvent"), label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("wrappedEvent"), label);
        }
    }
}