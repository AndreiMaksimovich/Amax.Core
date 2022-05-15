// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.Localization
{
    public abstract class ALocalizedStringProvider: ScriptableObject, ILocalizedStringProvider
    {
        public abstract string Id { get; set; }
        public abstract void GetStrings(Action<string, Dictionary<SystemLanguage, string>> processString);
    }
    
}