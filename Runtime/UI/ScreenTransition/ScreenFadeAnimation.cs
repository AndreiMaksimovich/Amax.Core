// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections;
using UnityEngine;

#endregion

namespace Amax.UI.ScreenTransition
{
    public class ScreenFadeAnimation: MonoBehaviour
    {
        
        public static IScreenFadeAnimation Instance { get; set; }

        private void Awake()
        {
            Instance ??= GetComponent<IScreenFadeAnimation>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public static void FadeIn(Action onFinish = null) => Instance.FadeIn(onFinish);
        public static void FadeOut(Action onFinish = null) => Instance.FadeOut(onFinish);
        
        public static IEnumerator FadeInCoroutine(Action onFinish = null) => Instance.FadeInCoroutine(onFinish);
        public static IEnumerator FadeOutCoroutine(Action onFinish = null) => Instance.FadeOutCoroutine(onFinish);
        
    }
}