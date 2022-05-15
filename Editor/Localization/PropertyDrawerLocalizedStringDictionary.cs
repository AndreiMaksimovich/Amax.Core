// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEditor;
using UnityEngine;

#endregion

namespace Amax.Localization.Editor
{
	
	[CustomPropertyDrawer(typeof (LocalizedStringDictionary), useForChildren: true)]
	public class PropertyDrawerLocalizedStringDictionary : PropertyDrawer
	{

		private const string
			PropertyNameKeys = "keys",
			PropertyNameValues = "values";

		private const int
			RowHeight = 100,
			KeyFiledHeight = 20,
			ValueFieldHeight = 70,
			ButtonWidth = 30,
			ButtonHeight = 18,
			LabelHeight = 20,
			BottomMargin = 30,
			AddButtonSize = 24;

		public override void OnGUI (Rect rect, SerializedProperty property, GUIContent label) {

			label = EditorGUI.BeginProperty (rect, label, property);
			var enumValues = Enum.GetNames(typeof(SystemLanguage));
			
			if (!string.IsNullOrEmpty (label.text)) 
			{
                GUI.Label(new Rect(rect.xMin, rect.y, rect.width, LabelHeight), label, EditorStyles.boldLabel);
                rect.y += LabelHeight;
			}
			
			var keys = property.FindPropertyRelative (PropertyNameKeys);
			var values = property.FindPropertyRelative (PropertyNameValues);
			
			var removeIndex = -1;
			for (var i = 0; i < keys.arraySize; i++)
			{
				
				var y = rect.y + i * RowHeight;
				var x = rect.x;
				
				var key = keys.GetArrayElementAtIndex (i);
				var value = values.GetArrayElementAtIndex (i);
				
				// Language
                var enumValueIndex = EditorGUI.Popup(new Rect(x, y, rect.width - ButtonWidth, KeyFiledHeight), key.enumValueIndex, enumValues);
                if (key.enumValueIndex != enumValueIndex) 
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
                
                // Delete button
                if (GUI.Button(new Rect(x + rect.width - ButtonWidth, y, ButtonWidth, ButtonHeight), "-")) 
                {
                	removeIndex = i;
                }

                y += KeyFiledHeight;

                // Text
				value.stringValue = EditorGUI.TextArea(new Rect(x, y, rect.width, ValueFieldHeight), value.stringValue);
			}

			// Add New
			if (GUI.Button (new Rect(rect.x + rect.width - AddButtonSize, rect.y + RowHeight * keys.arraySize, AddButtonSize, AddButtonSize),"+")) 
			{
				keys.InsertArrayElementAtIndex(keys.arraySize);
                keys.GetArrayElementAtIndex(keys.arraySize-1).enumValueIndex = GetNextLanguageEnumIndex(keys);
				values.InsertArrayElementAtIndex(values.arraySize);
				values.GetArrayElementAtIndex(keys.arraySize-1).stringValue = "value";
			}
			
			// Remove
			if (removeIndex >= 0) 
			{
				keys.DeleteArrayElementAtIndex (removeIndex);
				values.DeleteArrayElementAtIndex (removeIndex);
			}

            EditorGUI.EndProperty ();

		}

		private int GetNextLanguageEnumIndex(SerializedProperty keys)
        {
	        return GetSystemLanguageEnumIndex(SystemLanguage.Unknown);
        }

		private int GetSystemLanguageEnumIndex(SystemLanguage language) 
		{
			var names = Enum.GetNames(typeof(SystemLanguage));
			for (int i=0; i<names.Length; i++)
            {
				if (names[i] == language.ToString()) return i;
            }
			return 0;
		}

        private bool KeyExist(int enumValueIndex, SerializedProperty keys) 
        {
            for (int i = 0; i < keys.arraySize; i++) {
                SerializedProperty key = keys.GetArrayElementAtIndex(i);
                if (key.enumValueIndex == enumValueIndex) return true;
            }
            return false;
        }

        public override float GetPropertyHeight (SerializedProperty property, GUIContent label) 
        {
			return property.FindPropertyRelative(PropertyNameKeys).arraySize * RowHeight + BottomMargin + (string.IsNullOrEmpty(label.text) ? 0 : LabelHeight);
		}

	}

}