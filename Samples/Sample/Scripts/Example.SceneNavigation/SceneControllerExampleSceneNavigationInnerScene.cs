// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using Amax.Navigation;
using TMPro;
using UnityEngine;

#endregion

namespace Amax.Core.Examples
{

    public class SceneControllerExampleSceneNavigationInnerScene : ASceneController
    {

        [SerializeField] private TextMeshProUGUI textC;
        [SerializeField] private TextMeshProUGUI textD;

        public override void Configure(Scene scene)
        {
            if (scene.Configuration is InnerSceneConfiguration configuration)
            {
                textC.text = $"C: {configuration.C}";
                textD.text = $"D: {configuration.D}";
            }
            else
            {
                textC.text = $"Configuration not available";
            }

        }

    }

}
