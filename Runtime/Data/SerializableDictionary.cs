// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax {

	[Serializable]
	public class SerializableDictionary<TK, TV> : Dictionary<TK, TV>, ISerializationCallbackReceiver 
	{

		[SerializeField] private List<TK> keys = new List<TK>();
		[SerializeField] private List<TV> values = new List<TV>();

		public void OnBeforeSerialize() 
		{
			keys.Clear();
			values.Clear();
			foreach(var pair in this)
			{
				keys.Add(pair.Key);
				values.Add(pair.Value);
			}
		}
			
		public void OnAfterDeserialize() 
		{
			Clear();
			
			if (keys.Count != values.Count)
			{
				throw new Exception( $"there are {keys.Count} keys and {values.Count} values after deserialization. Make sure that both key and value types are serializable.");
			}
				
			for (int i = 0; i < keys.Count; i++) 
			{
				if (!ContainsKey(keys[i])) 
				{
					Add (keys [i], values [i]);
				}
			}
		}

	}
}

