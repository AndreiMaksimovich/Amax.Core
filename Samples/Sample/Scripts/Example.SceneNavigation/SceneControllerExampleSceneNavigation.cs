// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using Amax.Navigation;
using TMPro;
using UnityEngine;

#endregion

namespace Amax.Core.Examples
{

    public class SceneControllerExampleSceneNavigation : ASceneController
    {

        private const string InnerSceneBuildName = "Amax.Sample.Example.SceneNavigation.InnerScene";

        [SerializeField] private TMP_InputField inputFieldStateA;
        [SerializeField] private TMP_InputField inputFieldStateB;

        [SerializeField] private TMP_InputField inputFieldConfigurationC;
        [SerializeField] private TMP_InputField inputFieldConfigurationD;

        public override void Configure(Scene scene)
        {
            if (scene.SavedState is SceneState state)
            {
                inputFieldStateA.text = state.A;
                inputFieldStateB.text = state.B;
            }
        }

        public override object CurrentState => new SceneState()
        {
            A = inputFieldStateA.text,
            B = inputFieldStateB.text
        };

        public void OnOpenNextSceneButtonClick()
        {
            SceneNavigation.LoadScene
            (
                new Scene(
                    InnerSceneBuildName,
                    new InnerSceneConfiguration()
                    {
                        C = inputFieldConfigurationC.text, D = inputFieldConfigurationD.text
                    }
                )
            );
        }

    }

}
