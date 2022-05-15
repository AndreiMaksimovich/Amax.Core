// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using UnityEngine.AddressableAssets;

#endregion

namespace Amax.Navigation
{
    public class Scene
    {

        public int BuildId { get; protected set; } = -1;
        public string BuildName { get; protected set; }
        public AssetReference SceneAssetReference { get; protected set; }
        
        public ESceneType Type { get; set; }
        
        public object Configuration { get; set; }
        public T GetConfiguration<T>() where T: class => Configuration as T;
        
        public object SavedState { get; set; }
        public T GetSavedState<T>() where T: class => SavedState as T;

        public ParameterDictionary Parameters { get; set; } = new ParameterDictionary();
        
        public Scene(int buildId, object configuration = null, object savedState = null)
        {
            Type = ESceneType.BuiltInBuildId;
            BuildId = buildId;
            Configuration = configuration;
            SavedState = savedState;
        }
        
        public Scene(string buildName, object configuration = null, object savedState = null)
        {
            Type = ESceneType.BuiltInBuildName;
            BuildName = buildName;
            Configuration = configuration;
            SavedState = savedState;
        }
        
        public Scene(AssetReference sceneSceneAssetReference, object configuration = null, object savedState = null)
        {
            Type = ESceneType.Addressable;
            SceneAssetReference = sceneSceneAssetReference;
            Configuration = configuration;
            SavedState = savedState;
        }
        
        public enum ESceneType
        {
            BuiltInBuildId,
            BuiltInBuildName,
            Addressable
        }

        public bool IsSameScene(Scene scene)
        {
            if (scene.Type != Type) return false;
            return 
            (
                (Type == ESceneType.BuiltInBuildId && BuildId == scene.BuildId) ||
                (Type == ESceneType.BuiltInBuildName && BuildName.Equals(scene.BuildName)) ||
                (Type == ESceneType.Addressable && SceneAssetReference.RuntimeKey.Equals(scene.SceneAssetReference.RuntimeKey))
            );
        }

    }
}