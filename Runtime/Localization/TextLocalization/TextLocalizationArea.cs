// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.Localization
{
    public class TextLocalizationArea: MonoBehaviour, ITextLocalizationArea
    {
        
        [field: SerializeField] public bool IsRoot { get; set; }

        [SerializeField] private string stringIdPrefix;

        public void SetLocalStringIdPrefix(string value)
        {
            this.stringIdPrefix = value;
        }
        
        public string StringIdPrefix
            => IsRoot
                ? stringIdPrefix
                : LocalizedStringIdUtils.Join( transform.parent?.GetComponentInParent<ITextLocalizationArea>()?.StringIdPrefix, stringIdPrefix);

    }
}