// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Collections;
using Amax.Navigation;
using UnityEngine;

#endregion

namespace Amax.Core.Examples
{

    public class SceneControllerIntro : ASceneController
    {

        public float animationDuration = 1f;

        private void Start()
        {
            StartCoroutine(DelayedOpenMainMenuCoroutine());
        }

        private IEnumerator DelayedOpenMainMenuCoroutine()
        {
            yield return new WaitForSeconds(animationDuration);
            SceneNavigation.LoadMainMenuScene();
        }

    }

}
