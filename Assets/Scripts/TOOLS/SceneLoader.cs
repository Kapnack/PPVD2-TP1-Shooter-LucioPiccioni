using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class SceneLoader : MonoBehaviour, ISceneLoader
{
#if UNITY_EDITOR
    [SerializeField] private SceneRef exclude = null;
#endif

    private List<Scene> activeScenes = new List<Scene>();

    private void Awake()
    {
        ServiceProvider.SetService<ISceneLoader>(this);
    }

#if UNITY_EDITOR
    private IEnumerator Start()
    {
        if (SceneManager.sceneCount == 0)
            yield return null;

        CheckAndAddActiveScenesInEditor();
    }
#endif

    private IEnumerator LoadSceneCoroutine(SceneRef sceneRef, LoadSceneMode mode)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneRef.Index, mode);

        while (!asyncOp.isDone)
            yield return null;

        Scene loadedScene = SceneManager.GetSceneByBuildIndex(sceneRef.Index);

        if (loadedScene.IsValid() && loadedScene.isLoaded)
        {
            activeScenes.Add(loadedScene);
        }
        else
        {
            Debug.LogWarning($"The Scene with name {sceneRef.Name} didn't load correctly.");
        }
    }

    private IEnumerator UnLoadSceneCoroutine(Scene activeScenes)
    {
        AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(activeScenes);

        while (!asyncOp.isDone)
            yield return null;
    }

    public void LoadScene(SceneRef sceneRef, LoadSceneMode mode = LoadSceneMode.Additive)
    {
        if (sceneRef == null)
        {
            Debug.LogWarning("No SceneRef asigned");
            return;
        }
        else if (IsSceneLoaded(sceneRef))
        {
            Debug.LogWarning($"Scene is already loaded.");
            return;
        }
        else if (sceneRef.Index < 0)
        {
            Debug.LogWarning($"Not Valid SceneRef. Cause SceneIndex: {sceneRef.Index} is < 0");
            return;
        }

        StartCoroutine(LoadSceneCoroutine(sceneRef, mode));
    }

    public void LoadSceneAsync(SceneRef[] sceneRef, LoadSceneMode mode = LoadSceneMode.Additive)
    {
        for (int i = 0; i < sceneRef.Length; i++)
            LoadScene(sceneRef[i], mode);
    }

    public void UnloadAll()
    {
        for (int i = activeScenes.Count - 1; i >= 0; i--)
        {
            StartCoroutine(UnLoadSceneCoroutine(activeScenes[i]));
            activeScenes.RemoveAt(i);
        }

    }
    public void UnloadAll(SceneRef exeption)
    {
        for (int i = activeScenes.Count - 1; i >= 0; i--)
        {
            if (activeScenes[i].buildIndex != exeption.Index)
            {
                StartCoroutine(UnLoadSceneCoroutine(activeScenes[i]));
                activeScenes.RemoveAt(i);
            }
        }
    }

    public void UnloadAll(SceneRef[] exeptions)
    {
        for (int i = activeScenes.Count - 1; i >= 0; i--)
        {
            if (IsSceneInRefArray(activeScenes[i].buildIndex, exeptions))
            {
                StartCoroutine(UnLoadSceneCoroutine(activeScenes[i]));

                activeScenes.RemoveAt(i);
            }
        }
    }

    private bool IsSceneInRefArray(int index, SceneRef[] sceneRefs)
    {
        for (int i = 0; i < sceneRefs.Length; i++)
        {
            if (index == sceneRefs[i].Index)
                return true;
        }

        return false;
    }

    public bool IsSceneLoaded(SceneRef sceneRef)
    {
        for (int i = 0; i < activeScenes.Count; i++)
        {
            if (activeScenes[i].buildIndex == sceneRef.Index)
                return true;
        }

        return false;
    }

#if UNITY_EDITOR
    public void CheckAndAddActiveScenesInEditor()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.buildIndex != exclude.Index)
            {
                activeScenes.Add(scene);
            }

        }
    }
#endif
}
