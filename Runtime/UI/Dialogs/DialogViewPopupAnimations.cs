// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Amax.UI.Dialogs
{
    public class DialogViewPopupAnimations: MonoBehaviour, IDialogViewAnimations
    {

        [field: SerializeField] public float AppearAnimationDuration { get; set; } = 0.2f;
        [field: SerializeField] public float DisappearAnimationDuration { get; set; } = 0.2f;
        
        public IEnumerator AppearAnimationCoroutine(RectTransform dialogRoot, RectTransform dialogWindow, RectTransform background)
            => AnimationCoroutine(dialogRoot, dialogWindow, background, true);

        public IEnumerator DisappearAnimationCoroutine(RectTransform dialogRoot, RectTransform dialogWindow,
            RectTransform background)
            => AnimationCoroutine(dialogRoot, dialogWindow, background, false);

        private IEnumerator AnimationCoroutine(RectTransform dialogRoot, RectTransform dialogWindow, RectTransform background, bool appear)
        {
            var image = background.GetComponent<Image>();
            var initialColor = image.color;
            var duration = appear ? AppearAnimationDuration : DisappearAnimationDuration;

            var time = Time.deltaTime;

            while (time < duration)
            {
                var normalizedTime = time / duration;
                if (!appear) normalizedTime = 1 - normalizedTime;
                
                // color
                var color = initialColor;
                color.a = initialColor.a * normalizedTime;
                image.color = color;
                
                // scale
                dialogWindow.transform.localScale = Vector3.one * normalizedTime;
                
                yield return null;
                time += Time.deltaTime;
            }

            if (appear)
            {
                image.color = initialColor;
                dialogWindow.transform.localScale = Vector3.one;
            }

        }
        
    }
}