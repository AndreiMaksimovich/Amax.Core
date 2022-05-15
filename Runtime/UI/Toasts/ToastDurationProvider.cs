// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine;

#endregion

namespace Amax.UI.Toasts
{
    public class ToastDurationProvider: MonoBehaviour, IToastDurationProvider
    {

        [SerializeField] private float durationShort = 2f;
        [SerializeField] private float durationNormal = 4f;
        [SerializeField] private float durationLong = 6f;
        [SerializeField] private float durationVeryLong = 10f;
        
        public float GetDuration(EToastDuration duration)
            => duration switch
            {
                EToastDuration.Short => durationShort,
                EToastDuration.Long => durationLong,
                EToastDuration.VeryLong => durationVeryLong,
                _ => durationNormal
            };
        
    }
}