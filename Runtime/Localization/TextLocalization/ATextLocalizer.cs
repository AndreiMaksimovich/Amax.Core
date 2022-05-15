// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.Localization
{
    public abstract class ATextLocalizer: MonoBehaviour, ITextLocalizer, IEventBusListener<OnTextLocalizationLanguageChanged>
    {
        
        [field: SerializeField] public string StringId { get; set; }

        protected string FullStringId
            => LocalizedStringIdUtils.Join(GetComponentInParent<ITextLocalizationArea>()?.StringIdPrefix, StringId);
        
        protected virtual void Awake()
        {
            EventBus.AddListener(this);
        }

        protected virtual void OnDestroy()
        {
            EventBus.RemoveListener(this);
        }

        public void OnEvent(OnTextLocalizationLanguageChanged data)
        {
            Refresh();
        }

        protected virtual void Start()
        {
            Refresh();
        }
        
        public abstract void Refresh();

    }
}