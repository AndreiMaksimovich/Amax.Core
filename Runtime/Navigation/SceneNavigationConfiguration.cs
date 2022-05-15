// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Amax.Navigation
{
    
    [CreateAssetMenu(menuName = "Amax/SceneNavigation/Configuration", fileName = "SceneNavigationConfiguration.asset")]
    public class SceneNavigationConfiguration: ScriptableObject
    {
        
        [field: SerializeField] public SceneConfiguration HomeScene { get; set; }
        [field: SerializeField] public SceneConfiguration MainMenuScene { get; set; }
        
        [Serializable]
        public class SceneConfiguration
        {

            [SerializeField] private Scene.ESceneType type = Scene.ESceneType.BuiltInBuildId;
            [SerializeField] private int buildId = 0;
            [SerializeField] private string buildName = "";
            [SerializeField] private AssetReference sceneAssetReference;
            [SerializeField] private StringParameterDictionary parameters;

            public Scene.ESceneType Type
            {
                get => type;
                set => type = value;
            }
            
            public int BuildId 
            { 
                get => buildId;
                set => buildId = value;
            } 
            
            public string BuildName
            { 
                get => buildName;
                set => buildName = value;
            } 
            
            public AssetReference SceneAssetReference
            { 
                get => sceneAssetReference;
                set => sceneAssetReference = value;
            }

            public StringParameterDictionary Parameters
            {
                get => parameters;
                set => parameters = value;
            }

            public Scene ToScene()
            {
                Scene scene;
                switch (Type)
                {
                    case Scene.ESceneType.BuiltInBuildName: 
                        scene = new Scene(BuildName); break;
                    case Scene.ESceneType.Addressable: 
                        scene = new Scene(SceneAssetReference); break;      
                    case Scene.ESceneType.BuiltInBuildId: 
                    default: 
                        scene = new Scene(BuildId); break;
                }
                foreach (var keyValuePair in Parameters)
                {
                    scene.Parameters.Set(keyValuePair.Key, keyValuePair.Value);
                }
                return scene;
            }
            
        }
        
    }
    
}