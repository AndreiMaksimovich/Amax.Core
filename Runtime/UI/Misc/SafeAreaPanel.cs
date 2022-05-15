// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Amax.UI
{ 

    public class SafeAreaPanel : MonoBehaviour 
    {
        private RectTransform RectTransform => transform as RectTransform;
        private Rect AppliedSafeArea { get; set; } = new Rect(0, 0, 0, 0);

        private void Awake() 
        {
#if UNITY_EDITOR
            if (devSafeAreaRect.width == 0)
            {
                appliedDevZoneType = devSaveZoneType;
                appliedDevZoneTypeLocal = devSaveZoneType;
                devSafeAreaZone = DevSafeZones[devSaveZoneType];
            }
            else
            {
                devSaveZoneType = appliedDevZoneType;
                appliedDevZoneTypeLocal = appliedDevZoneType;
            }
#endif
            Refresh();
        }

        private void Update() {
#if UNITY_EDITOR
            if (devSaveZoneType!=appliedDevZoneType)
            {
                if (appliedDevZoneTypeLocal == devSaveZoneType)
                {
                    devSaveZoneType = appliedDevZoneType;
                    appliedDevZoneTypeLocal = appliedDevZoneType;
                }
                else
                {
                    appliedDevZoneType = devSaveZoneType;
                    appliedDevZoneTypeLocal = devSaveZoneType;
                    devSafeAreaZone = DevSafeZones[devSaveZoneType];
                }
            }
#endif
            Refresh();
        }

        private void Refresh() {
#if UNITY_EDITOR
            if (devSafeAreaRect != AppliedSafeArea) 
            {
                ApplySafeArea(devSafeAreaRect);
            }
#else
            if (Screen.safeArea != AppliedSafeArea) ApplySafeArea(Screen.safeArea);
#endif
        }

        private Rect ScreenSafeArea => Screen.safeArea;

        private void ApplySafeArea(Rect rect) 
        {
            AppliedSafeArea = rect;
            Vector2 anchorMin = rect.position;
            Vector2 anchorMax = rect.position + rect.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            RectTransform.anchorMin = anchorMin;
            RectTransform.anchorMax = anchorMax;
        }

#if UNITY_EDITOR

        public enum EDevSafeZoneType
        {
            None,
            IphoneXS
        }

        static readonly Dictionary<EDevSafeZoneType, Rect> DevSafeZones = new()
        {
            { EDevSafeZoneType.None, new Rect(0, 0, 1, 1) },
            { EDevSafeZoneType.IphoneXS, new Rect(0.05f, 0.05f, 0.9f, 0.9f) },
        };

        public EDevSafeZoneType devSaveZoneType = EDevSafeZoneType.None;
        private EDevSafeZoneType appliedDevZoneTypeLocal;
        static EDevSafeZoneType appliedDevZoneType;
        static Rect devSafeAreaZone = Rect.zero;
        Rect devSafeAreaRect => new Rect(devSafeAreaZone.x * Screen.width, devSafeAreaZone.y * Screen.height, devSafeAreaZone.width * Screen.width, devSafeAreaZone.height * Screen.height);

#endif

    }

}
