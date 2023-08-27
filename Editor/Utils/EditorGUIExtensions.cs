using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Utils
{
    public static class EditorGUIExtensions
    {
        #region DRAW DEFAULT INSPECTOR
        public static void DefaultScriptField(Object target)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", target, target.GetType(), false);
            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();
        }

        public static void DoDrawDefaultInspector(SerializedObject obj, bool showScriptField = true)
        {
            EditorGUI.BeginChangeCheck();
            obj.UpdateIfRequiredOrScript();
            SerializedProperty iterator = obj.GetIterator();

            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                // Check if the property is "m_Script" and whether to show it
                if (iterator.propertyPath == "m_Script")
                {
                    if (showScriptField)
                    {
                        using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                        {
                            EditorGUILayout.PropertyField(iterator, true);
                        }
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(iterator, true);
                }
            }

            obj.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }
        #endregion
        
        #region HORIZONTAL LINE
        private static void HorizontalLine(Color color, float height = 1f, float marginTop = 2f, float marginBottom = 2f)
        {
            EditorGUILayout.Space(marginTop);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, height), color);
            EditorGUILayout.Space(marginBottom);
        }

        public static void HorizontalLine(Color color, float height, Vector2 margin)
        {
            HorizontalLine(color, height, margin.x, margin.y);
        }
        
        public static void HorizontalLine(float height, Vector2 margin)
        {
            HorizontalLine(new Color(0f, 0f, 0f, 0.3f), height, margin.x, margin.y);
        }
        
        public static void HorizontalLine(Vector2 margin)
        {
            HorizontalLine(new Color(0f, 0f, 0f, 0.3f), 1f, margin.x, margin.y);
        }
        #endregion

        #region PROPERTY SLIDER
        /// <summary>
        /// Creates slider field for serialized property
        /// </summary>
        /// <param name="property"> serialized property </param>
        /// <param name="leftValue"> min value </param>
        /// <param name="rightValue"> max value </param>
        /// <param name="label"> label (name, tooltip) </param>
        public static void PropertyFloatSlider(SerializedProperty property, float leftValue, float rightValue, GUIContent label)
        {
            var position = EditorGUILayout.GetControlRect();
            
            label = EditorGUI.BeginProperty(position, label, property);
            
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.Slider(position, label, property.floatValue, leftValue, rightValue);
            
            if (EditorGUI.EndChangeCheck())
                property.floatValue = newValue;
            
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Creates slider field for serialized property
        /// </summary>
        /// <param name="property"> serialized property </param>
        /// <param name="leftValue"> min value </param>
        /// <param name="rightValue"> max value </param>
        /// <param name="label"> label (name, tooltip) </param>
        public static void PropertyIntSlider(SerializedProperty property, int leftValue, int rightValue, GUIContent label)
        {
            var position = EditorGUILayout.GetControlRect();
            
            label = EditorGUI.BeginProperty(position, label, property);
            
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.IntSlider(position, label, property.intValue, leftValue, rightValue);
            
            if (EditorGUI.EndChangeCheck())
                property.intValue = newValue;
            
            EditorGUI.EndProperty();
        }
        #endregion
    }
}