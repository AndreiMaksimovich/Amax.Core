// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;

#endregion

namespace Amax
{
    
    [Serializable]
    public class StringParameterDictionary: SerializableDictionary<string, string> { }

    [Serializable]
    public class StringParameterDictionary<T> : SerializableDictionary<string, T> { }
    
}