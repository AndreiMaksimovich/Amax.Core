// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Amax.UI.Toasts
{
    public class ToastViewAnimator: MonoBehaviour, IToastViewAnimator
    {

        [SerializeField] private float appearAnimationDuration = 0.2f;
        [SerializeField] private float disappearAnimationDuration = 0.2f;
        [SerializeField] private GameObject toastContainerFillerPrefab;

        public IEnumerator AppearAnimationCoroutine(RectTransform toastContainer, RectTransform toastRoot)
        {
            
            var toastLayoutElement = toastRoot.GetComponent<LayoutElement>();
            var toastContentSizeFitter = toastRoot.GetComponent<ContentSizeFitter>();

            var containerFillerGameObject = Instantiate(toastContainerFillerPrefab, toastContainer);
            var containerFillerLayoutElement = containerFillerGameObject.GetComponent<LayoutElement>();
            containerFillerLayoutElement.preferredHeight = 0;

            // Move down
            var position = toastRoot.position;
            position.y = -1024;
            toastRoot.position = position;
            
            yield return null;
            
            // Wait for canvas update
            while (toastRoot.rect.height <= 0)
            {
                yield return null;
            }
            var toastHeight = toastRoot.rect.height;

            // Animation
            var time = Time.deltaTime;
            while (time<appearAnimationDuration)
            {
                var normalizedTime = time / appearAnimationDuration;
                containerFillerLayoutElement.preferredHeight = toastHeight * normalizedTime;
                toastRoot.localScale = new Vector3(1, normalizedTime, 1);
                toastRoot.position = containerFillerGameObject.transform.position;
                yield return null;
                time += Time.deltaTime;
            }
            
            // Remove filler & set up toast layout
            toastRoot.localScale = Vector3.one;
            Destroy(containerFillerGameObject);
            Destroy(toastContentSizeFitter);
            toastLayoutElement.ignoreLayout = false;

        }

        public IEnumerator DisappearAnimationCoroutine(RectTransform toastContainer, RectTransform toastRoot)
        {
            
            var siblingIndex = toastRoot.GetSiblingIndex();
            
            var toastHeight = toastRoot.rect.height;
            
            var containerFillerGameObject = Instantiate(toastContainerFillerPrefab, toastContainer);
            var containerFillerLayoutElement = containerFillerGameObject.GetComponent<LayoutElement>();
            containerFillerLayoutElement.preferredHeight = 0;

            yield return null;
            containerFillerGameObject.transform.SetSiblingIndex(siblingIndex);
            yield return null;
            
            toastRoot.GetComponent<LayoutElement>().ignoreLayout = true;
            
            // Animation
            var time = Time.deltaTime;
            while (time<disappearAnimationDuration)
            {
                var normalizedTime = 1 - time / disappearAnimationDuration;
                containerFillerLayoutElement.preferredHeight = toastHeight * normalizedTime;
                yield return null;
                toastRoot.localScale = new Vector3(1, normalizedTime, 1);
                toastRoot.position = containerFillerGameObject.transform.position;
                time += Time.deltaTime;
            }
            
            yield return null;
            
            // Remove filler & set up toast layout
            toastRoot.localScale = Vector3.zero;
            containerFillerLayoutElement.preferredHeight = 0;
            
            yield return null;
            
            // Move down
            var position = toastRoot.position;
            position.y = -2048;
            toastRoot.position = position;
            
            yield return null;
            Destroy(containerFillerGameObject);
        }

        private IEnumerator SimpleAnimationCoroutine(RectTransform toastContainer, RectTransform toastRoot, bool appear)
        {
            var duration = appear ? appearAnimationDuration : disappearAnimationDuration;
            var time = Time.deltaTime;
            
            while (time < duration)
            {
                var normalizedTime = time / duration;
                if (!appear) normalizedTime = 1 - normalizedTime;
                toastRoot.localScale = new Vector3(1f, normalizedTime, 1f);
                yield return null;
                time += Time.deltaTime;
            }
            
            toastRoot.localScale = appear ? Vector3.one : Vector3.zero;
        }
        
    }
}