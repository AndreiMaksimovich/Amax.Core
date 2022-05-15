// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;

#endregion

namespace Amax
{
    
    public class ParameterDictionary: SerializableDictionary<string, object>
    {

        public T Get<T>(string key, T defaultValue = null) where T: class
            => ContainsKey(key) ? this[key] as T : defaultValue;
        
        public int GetInt(string key, int defaultValue = 0)
            => ContainsKey(key) ? (int) this[key] : defaultValue;
        
        public float GetFloat(string key, float defaultValue = 0)
            => ContainsKey(key) ? (float) this[key] : defaultValue;
        
        public string GetString(string key, string defaultValue = null)
            => ContainsKey(key) ? (string) this[key] : defaultValue;
        
        public long GetLong(string key, long defaultValue = 0)
            => ContainsKey(key) ? (long) this[key] : defaultValue;

        public T GetFromXml<T>(string key, T defaultValue = null) where T: class
        {
            try
            {
                return XmlUtils.Deserialize<T>(GetString(key));
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR
                Debug.LogError(ex);
#endif
            }
            return defaultValue;
        }

        public void SetAsXml<T>(string key, T value) where T : class
            => Set(key, XmlUtils.Serialize(value));
        
        public T GetFromJson<T>(string key, T defaultValue = null) where T: class
        {
            try
            {
                return JsonUtility.FromJson<T>(GetString(key));
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR
                Debug.LogError(ex);
#endif
            }
            return defaultValue;
        }
        
        public void SetAsJson<T>(string key, T value) where T : class
            => Set(key, JsonUtility.ToJson(value));

        public void Set<T>(string key, T value) where T: class
            => this[key] = value;
        
    }
    
}