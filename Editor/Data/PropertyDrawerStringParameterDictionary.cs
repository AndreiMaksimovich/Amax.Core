// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEditor;
using UnityEngine;

#endregion

namespace Amax.Editor
{
    
    [CustomPropertyDrawer(typeof (StringParameterDictionary<>), useForChildren: true)]
    public class PropertyDrawerStringParameterDictionaryGeneric: PropertyDrawer
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
				
				var key = keys.GetArrayElementAtIndex(i);
				var value = values.GetArrayElementAtIndex (i);
				var fieldWidth = Mathf.Max((rect.width - RemoveButtonWidth) / 2f, 20f);

				// Key
                var newKeyValue = EditorGUI.TextField(new Rect(x - 5, y, fieldWidth, RowHeight), key.stringValue);
                if (newKeyValue != key.stringValue) 
                {
                    if (!KeyExist(newKeyValue, keys)) 
                    {
                        key.stringValue = newKeyValue;
                    } 
                    else 
                    {
                        Debug.LogWarning("Key exists");
                    }
                }
                
                x += fieldWidth;

                // Value
                EditorGUI.ObjectField(new Rect(x, y, fieldWidth, RowHeight), value, GUIContent.none);
                
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
				if (!KeyExist("", keys))
				{
					keys.InsertArrayElementAtIndex(keys.arraySize);
					keys.GetArrayElementAtIndex(keys.arraySize - 1).stringValue = "";
					values.InsertArrayElementAtIndex(values.arraySize);
				}
			}
			
			// Remove
			if (removeIndex >= 0) {
				keys.DeleteArrayElementAtIndex (removeIndex);
				values.DeleteArrayElementAtIndex (removeIndex);
			}

            EditorGUI.EndProperty ();

		}
		
		private bool KeyExist(string key, SerializedProperty keys) 
		{
			for (var i = 0; i < keys.arraySize; i++) 
			{
				if (key == keys.GetArrayElementAtIndex(i).stringValue) return true;
			}
			return false;
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
			=> property.FindPropertyRelative(PropertyNameKeys).arraySize * RowHeight + BottomMargin + (string.IsNullOrEmpty(label.text) ? 0 : LabelHeight);
		
    }
    
}