using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ProgramLab
{
    // взято отсюда:
    // https://discussions.unity.com/t/custom-inspector-display-properties-of-objects-in-array-that-inherit-from-same-base-abstract-class/872125/2
    [CustomPropertyDrawer(typeof(AbilityBase))]
    public class AbilityPropertyDrawer : PropertyDrawer
    {
        private Type[] _criteriaTypes;
        private string[] _criteriaTypeNames;
        private int index = -1;
        private AbilityBase _target;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GetCriteriaList();
            GetTarget(property);
            index = Array.IndexOf(_criteriaTypes, _target?.GetType());

            EditorGUI.BeginChangeCheck();
            var popupLabel = index == -1 ? "Select Type" : "";
            Rect popupRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            var selection = EditorGUI.Popup(popupRect, popupLabel, index, _criteriaTypeNames);

            EditorGUI.PropertyField(position, property, label, true);

            if (!EditorGUI.EndChangeCheck()
                || selection == index
                || selection == -1)
            {
                return;
            }
            
            var arrayFieldName = property.propertyPath.Substring(0, property.propertyPath.IndexOf('.'));
            var arrayIndex = Convert.ToInt32(new string(property.propertyPath.Where(char.IsDigit).ToArray()));
            var selectedType = _criteriaTypes.FirstOrDefault(t => t.Name == _criteriaTypeNames[selection]);
            var target = property.serializedObject.targetObject;

            var field = target.GetType().GetField(arrayFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            var array = field.GetValue(target) as AbilityBase[];
            array[arrayIndex] = (AbilityBase)Activator.CreateInstance(selectedType);
            property.isExpanded = true;
        }

        private void GetTarget(SerializedProperty property)
        {
            if (_target == null)
            {
                var objAr = fieldInfo.GetValue(property.serializedObject.targetObject);
                var index = Convert.ToInt32(new string(property.propertyPath.Where(char.IsDigit).ToArray()));
                _target = (objAr as object[])[index] as AbilityBase;
            }
        }

        private void GetCriteriaList()
        {
            if (_criteriaTypes == null)
            {
                _criteriaTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AbilityBase)))
                    .ToArray();

                _criteriaTypeNames = _criteriaTypes.Select(t => t.Name).ToArray();
            }
        }
    }
}