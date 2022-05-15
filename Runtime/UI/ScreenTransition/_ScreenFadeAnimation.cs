// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections;

#endregion

namespace Amax.UI.ScreenTransition
{

    public interface IScreenFadeAnimation
    {
        public void FadeIn(Action onFinish = null);
        public void FadeOut(Action onFinish = null);
        public IEnumerator FadeInCoroutine(Action onFinish = null);
        public IEnumerator FadeOutCoroutine(Action onFinish = null);
    }
    
}