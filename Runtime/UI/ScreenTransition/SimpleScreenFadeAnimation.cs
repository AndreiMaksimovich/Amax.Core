// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Amax.UI.ScreenTransition
{
    public class SimpleScreenFadeAnimation: MonoBehaviour, IScreenFadeAnimation
    {
        
        [field: SerializeField] public bool AutoFadeIn { get; set; }
        [field: SerializeField] public float FadeInDuration { get; set; } = 0.3f;
        [field: SerializeField] public float FadeOutDuration { get; set; } = 0.3f;
        [field: SerializeField] public Image Image { get; set; }
        
        private Color ImageInitialColor { get; set; }

        private void Awake()
        {
            ImageInitialColor = Image.color;
            if (AutoFadeIn) FadeIn();
        }

        public void FadeIn(Action onFinish = null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeInCoroutine(onFinish));
        }

        public void FadeOut(Action onFinish = null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutCoroutine(onFinish));
        }

        public IEnumerator FadeInCoroutine(Action onFinish = null)
            => FadeAnimationCoroutine(onFinish, false);

        public IEnumerator FadeOutCoroutine(Action onFinish = null)
            => FadeAnimationCoroutine(onFinish, true);

        private IEnumerator FadeAnimationCoroutine(Action onFinish, bool fadeOut)
        {
            Image.gameObject.SetActive(true);

            var duration = fadeOut ? FadeOutDuration : FadeInDuration;
            var time = Time.deltaTime;

            while (time < duration)
            {
                var nt = time / duration;
                if (!fadeOut) nt = 1 - nt;
                var color = ImageInitialColor;
                color.a = ImageInitialColor.a * nt;
                Image.color = color;
                yield return null;
                time += Time.deltaTime;
            }

            Image.color = ImageInitialColor;
            Image.gameObject.SetActive(fadeOut);
            
            onFinish?.Invoke();
        }
        
    }
}