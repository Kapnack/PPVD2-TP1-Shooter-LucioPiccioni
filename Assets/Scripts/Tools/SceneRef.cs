using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public class SceneRef : ISerializationCallbackReceiver
{

#if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneAsset;
#endif

    [field: SerializeField] public int Index { get; private set; }
    [field: SerializeField] public string Name { get; private set; }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (!sceneAsset)
            return;

        Index = SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(sceneAsset));

        Name = sceneAsset.name;

#endif
    }

    public void OnAfterDeserialize()
    {
    }
}