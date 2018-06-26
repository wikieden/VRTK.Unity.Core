namespace VRTK.Core.Rule
{
    using UnityEditor;
    using UnityEditor.IMGUI.Controls;
    using UnityEditorInternal;
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using VRTK.Core.Utility;

    public class ComponentCreatorWindow : EditorWindow
    {
        protected static string searchText = string.Empty;

        protected Type type;
        protected Type[] componentTypes;
        protected Action<Type> selectAction;

        protected SearchField searchField;
        protected GUIStyle labelStyle;

        protected Vector2 scrollPosition = Vector2.zero;
        protected int selectionIndex;

        public static void Show(Rect sourceRect, Type type, Action<Type> selectAction)
        {
            ComponentCreatorWindow window = CreateInstance<ComponentCreatorWindow>();
            window.type = type;
            window.componentTypes = type.Assembly.GetTypes()
                .Where(
                    possibleComponentType => type.IsAssignableFrom(possibleComponentType)
                        && possibleComponentType.IsSubclassOf(typeof(Component))
                        && !possibleComponentType.IsAbstract)
                .ToArray();
            window.selectAction = selectAction;
            window.ShowAsDropDown(sourceRect, new Vector2(Mathf.Max(250f, sourceRect.width), 250f));
        }

        protected virtual void Awake()
        {
            titleContent = new GUIContent("Component Creator");
            wantsMouseMove = true;
            searchField = new SearchField();
            labelStyle = new GUIStyle(EditorStyles.label);
        }

        protected virtual void OnGUI()
        {
            Event currentEvent = Event.current;
            if (type == null
                || currentEvent.type == EventType.KeyDown
                && currentEvent.keyCode == KeyCode.Escape
                && string.IsNullOrEmpty(searchText))
            {
                Close();
                return;
            }

            Type[] matchingComponentTypes = componentTypes
                .Where(type1 => Regex.IsMatch(type1.Name, $".*{searchText}.*", RegexOptions.IgnoreCase))
                .ToArray();
            if (currentEvent.type == EventType.KeyDown)
            {
                if (currentEvent.keyCode == KeyCode.DownArrow)
                {
                    selectionIndex++;
                    Repaint();
                    return;
                }

                if (currentEvent.keyCode == KeyCode.UpArrow)
                {
                    selectionIndex--;
                    Repaint();
                    return;
                }
            }

            selectionIndex = Math.Min(matchingComponentTypes.Length - 1, Math.Max(0, selectionIndex));

            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0
                || currentEvent.type == EventType.KeyDown
                && (currentEvent.keyCode == KeyCode.Return || currentEvent.keyCode == KeyCode.KeypadEnter))
            {
                selectAction(matchingComponentTypes[selectionIndex]);
                Close();
                return;
            }

            using (new EditorGUILayout.VerticalScope("grey_border"))
            {
                const float searchRectPadding = 8f;
                Rect searchRect = GUILayoutUtility.GetRect(36f, EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                searchRect.x += searchRectPadding;
                searchRect.y = 7f;
                searchRect.width -= searchRectPadding * 2f;
                searchRect.height = 30f;

                searchField.SetFocus();
                searchText = searchField.OnGUI(searchRect, searchText);
                EditorGUILayout.Separator();

                using (EditorGUILayout.ScrollViewScope scrollViewScope = new EditorGUILayout.ScrollViewScope(scrollPosition, EditorStyles.helpBox))
                {
                    scrollPosition = scrollViewScope.scrollPosition;

                    for (int index = 0; index < matchingComponentTypes.Length; index++)
                    {
                        Color previousBackgroundColor = GUI.backgroundColor;
                        Texture2D previousNormalBackground = labelStyle.normal.background;
                        if (selectionIndex == index)
                        {
                            GUI.backgroundColor = GUI.skin.settings.selectionColor;
                            labelStyle.normal.background = Texture2D.whiteTexture;
                            EditorGUILayout.BeginHorizontal();
                        }

                        EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(matchingComponentTypes[index].Name.Replace("Rule", string.Empty)), labelStyle);

                        if (selectionIndex == index)
                        {
                            EditorGUILayout.EndHorizontal();
                            labelStyle.normal.background = previousNormalBackground;
                            GUI.backgroundColor = previousBackgroundColor;
                        }

                        if (currentEvent.type == EventType.MouseMove && GUILayoutUtility.GetLastRect().Contains(currentEvent.mousePosition))
                        {
                            selectionIndex = index;
                            Repaint();
                            return;
                        }
                    }
                }
            }
        }
    }

    [CustomPropertyDrawer(typeof(RuleContainer), true)]
    public class RuleContainerDrawer : PropertyDrawer
    {
        protected static readonly Dictionary<int, bool> ShowChildrenByObjectId = new Dictionary<int, bool>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Type type = fieldInfo.FieldType;
            if (type.IsGenericType)
            {
                type = type.GenericTypeArguments.Single();
            }

            if (type.HasElementType)
            {
                type = type.GetElementType();
            }

            while (type != null && type.BaseType != typeof(InterfaceContainer))
            {
                type = type.BaseType;
            }

            if (type?.BaseType != typeof(InterfaceContainer))
            {
                throw new ArgumentException();
            }

            type = type.GenericTypeArguments.Single();

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                label.tooltip = EditorHelper.GetTooltipAttribute(fieldInfo)?.tooltip ?? string.Empty;
                position.height = EditorGUIUtility.singleLineHeight;

                SerializedObject serializedObject = property.serializedObject;
                SerializedProperty fieldProperty = property.FindPropertyRelative("field");

                bool showChildren;
                int hashCode = property.propertyPath.GetHashCode();
                ShowChildrenByObjectId.TryGetValue(hashCode, out showChildren);

                bool hasReference = fieldProperty.objectReferenceValue != null;
                bool isCircularReference = fieldProperty.objectReferenceValue == serializedObject.targetObject;

                Rect foldoutRect = position;
                if (hasReference && !isCircularReference)
                {
                    foldoutRect.width = EditorGUI.IndentedRect(new Rect(Vector2.zero, Vector2.zero)).x;
                    ShowChildrenByObjectId[hashCode] = EditorGUI.Foldout(foldoutRect, showChildren, GUIContent.none, true);
                }

                GUIContent buttonContent = new GUIContent(
                    hasReference ? "-" : "+",
                    $"{(hasReference ? "Remove" : "Add a new")} {type.Name} {(hasReference ? "from" : "to")} this game object.");
                float addButtonWidth;
                float removeButtonWidth;
                float buttonMaxWidth;
                GUI.skin.button.CalcMinMaxWidth(new GUIContent("+"), out addButtonWidth, out buttonMaxWidth);
                GUI.skin.button.CalcMinMaxWidth(new GUIContent("-"), out removeButtonWidth, out buttonMaxWidth);
                float buttonWidth = Mathf.Max(addButtonWidth, removeButtonWidth);

                using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
                {
                    Rect pickerRect = position;
                    pickerRect.width -= buttonWidth + 2f * EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.ObjectField(pickerRect, fieldProperty, type, label);
                    if (changeCheckScope.changed)
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                }

                Rect buttonRect = position;
                buttonRect.x = buttonRect.width - 2f * EditorGUIUtility.standardVerticalSpacing;
                buttonRect.width = buttonWidth;
                if (GUI.Button(buttonRect, buttonContent))
                {
                    if (hasReference)
                    {
                        UnityEngine.Object reference = fieldProperty.objectReferenceValue;
                        fieldProperty.objectReferenceValue = null;
                        serializedObject.ApplyModifiedProperties();
                        Undo.DestroyObjectImmediate(reference);

                        /*
                         * Because we remove a component on the same game object the inspector is drawing currently
                         * Unity will encounter the removed component and throw an exception trying to draw the
                         * inspector for it. This instruction will basically tell Unity to skip the current GUI
                         * loop iteration, preventing any errors.
                         */
                        GUIUtility.ExitGUI();

                        return;
                    }

                    Rect creatorRect = new Rect
                    {
                        min = GUIUtility.GUIToScreenPoint(position.min + Vector2.right * EditorGUIUtility.labelWidth),
                        max = GUIUtility.GUIToScreenPoint(buttonRect.max)
                    };
                    ComponentCreatorWindow.Show(
                        creatorRect,
                        type,
                        selectedType =>
                        {
                            fieldProperty.objectReferenceValue = Undo.AddComponent(Selection.activeGameObject, selectedType);
                            InternalEditorUtility.SetIsInspectorExpanded(fieldProperty.objectReferenceValue, false);
                            serializedObject.ApplyModifiedProperties();
                            ShowChildrenByObjectId[hashCode] = true;
                        });
                }

                if (!showChildren || !hasReference || isCircularReference)
                {
                    return;
                }

                /*
                 * Keep repainting this PropertyDrawer because a child is visible. This ensures the property fields
                 * of the child are updated when an undo operation is performed.
                 */
                EditorUtility.SetDirty(serializedObject.targetObject);

                position.y += position.height;
                using (new EditorGUI.IndentLevelScope())
                {
                    SerializedObject fieldSerializedObject = new SerializedObject(fieldProperty.objectReferenceValue);
                    SerializedProperty iteratedProperty = fieldSerializedObject.GetIterator();
                    iteratedProperty.NextVisible(true);

                    while (iteratedProperty.NextVisible(false))
                    {
                        using (new EditorGUI.PropertyScope(position, label, iteratedProperty))
                        using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
                        {
                            float propertyHeight = EditorGUI.GetPropertyHeight(iteratedProperty);
                            bool expandedChildren = EditorGUI.PropertyField(
                                new Rect(position.x, position.y, position.width, propertyHeight),
                                iteratedProperty,
                                true);
                            if (changeCheckScope.changed)
                            {
                                iteratedProperty.serializedObject.ApplyModifiedProperties();
                            }

                            if (!expandedChildren)
                            {
                                break;
                            }

                            position.y += propertyHeight;
                        }
                    }
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float result = EditorGUIUtility.singleLineHeight;

            SerializedProperty fieldProperty = property.FindPropertyRelative("field");
            bool isCircularReference = fieldProperty.objectReferenceValue == property.serializedObject.targetObject;
            bool showChildren;
            if (ShowChildrenByObjectId.TryGetValue(property.propertyPath.GetHashCode(), out showChildren)
                && !showChildren
                || fieldProperty.objectReferenceValue == null
                || isCircularReference)
            {
                return result;
            }

            using (SerializedObject serializedObject = new SerializedObject(fieldProperty.objectReferenceValue))
            {
                SerializedProperty iteratedProperty = serializedObject.GetIterator();
                iteratedProperty.NextVisible(true);
                while (iteratedProperty.NextVisible(false))
                {
                    result += EditorGUI.GetPropertyHeight(iteratedProperty, true);
                }
            }

            return result;
        }
    }
}