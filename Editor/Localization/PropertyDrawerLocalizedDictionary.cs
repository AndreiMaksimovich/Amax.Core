// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEditor;
using UnityEngine;

#endregion

namespace Amax.Localization.Editor
{
    
    [CustomPropertyDrawer(typeof (LocalizedDictionary<>),true)]
    public class PropertyDrawerLocalizedDictionary: PropertyDrawer
    {
	    
	    private const string
		    PropertyNameKeys = "keys",
		    PropertyNameValues = "values";
        
        private const int
			RowHeight = 20,
			RemoveButtonWidth = 30,
			RemoveButtonHeight = 18,
			LabelHeight = 20,
			BottomMargin = 30,
			AddButtonWidth = 30,
			AddButtonHeight = 24;

		public override void OnGUI (Rect rect, SerializedProperty property, GUIContent label) {

			var keys = property.FindPropertyRelative (PropertyNameKeys);
			var values = property.FindPropertyRelative (PropertyNameValues);
			var enumValues = Enum.GetNames(typeof(SystemLanguage));
			var removeIndex = -1;
			
			// Label
			label = EditorGUI.BeginProperty (rect, label, property);
			if (!string.IsNullOrEmpty (label.text)) {
                GUI.Label(new Rect(rect.xMin, rect.yMin, rect.width, LabelHeight), label, EditorStyles.boldLabel);
                rect.y += LabelHeight;
			}
			
			// Values
			for (var i = 0; i < keys.arraySize; i++) 
			{
				
				var y = rect.y + i * RowHeight;
				var x = rect.x;
				
				var key = keys.GetArrayElementAtIndex (i);
				var value = values.GetArrayElementAtIndex (i);
				var fieldWidth = Mathf.Max((rect.width - RemoveButtonWidth) / 2f, 20f);

				// Language
                int enumValueIndex = EditorGUI.Popup(new Rect(x, y, fieldWidth, RowHeight), key.enumValueIndex, enumValues);
                if (key.enumValueIndex!=enumValueIndex) 
                {
                    if (!KeyExist(enumValueIndex, keys)) 
                    {
                        key.enumValueIndex = enumValueIndex;
                    } 
                    else 
                    {
                        Debug.LogWarning("Key exists");
                    }
                }
                
                x += fieldWidth;

                // Value
                //(new Rect(x, y, fieldWidth, RowHeight), value, GUIContent.none);
                EditorGUI.ObjectField(new Rect(x, y, fieldWidth, RowHeight), value, GUIContent.none);
                //EditorGUI.PropertyField(new Rect(0, y, rect.width, RowHeight), value, GUIContent.none);
                
				x += fieldWidth;
				
				// Remove Button
				if (GUI.Button(new Rect(x, rect.y + i*RowHeight, RemoveButtonWidth, RemoveButtonHeight), "-")) 
				{
					removeIndex = i;
				}
			}

			// Add New
			if (GUI.Button (new Rect(rect.x + rect.width - AddButtonWidth, rect.y + RowHeight * keys.arraySize, AddButtonWidth, AddButtonHeight),"+")) 
			{
				keys.InsertArrayElementAtIndex(keys.arraySize);
                keys.GetArrayElementAtIndex(keys.arraySize-1).enumValueIndex = Enum.GetNames(typeof(SystemLanguage)).Length-1;
				values.InsertArrayElementAtIndex(values.arraySize);
			}
			
			// Remove
			if (removeIndex >= 0) {
				keys.DeleteArrayElementAtIndex (removeIndex);
				values.DeleteArrayElementAtIndex (removeIndex);
			}

            EditorGUI.EndProperty ();

		}
		
		private bool KeyExist(int enumValueIndex, SerializedProperty keys) 
		{
			for (var i = 0; i < keys.arraySize; i++) 
			{
				var key = keys.GetArrayElementAtIndex(i);
				if (key.enumValueIndex == enumValueIndex) return true;
			}
			return false;
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
			=> property.FindPropertyRelative(PropertyNameKeys).arraySize * RowHeight + BottomMargin + (string.IsNullOrEmpty(label.text) ? 0 : LabelHeight);
		
    }
    
}