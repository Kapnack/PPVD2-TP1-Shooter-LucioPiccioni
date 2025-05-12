using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        ServiceProvider.SetService(this);

        LoadScene(2, LoadSceneMode.Additive, false);
    }

    public void LoadScene(int scene, LoadSceneMode mode, bool Async)
    {
        if (!IsSceneLoaded(scene))
        {
            if (Async)
                SceneManager.LoadSceneAsync(name, mode);
            else
                SceneManager.LoadScene(name, mode);
        }
        else
        {
            Scene expectedScene = SceneManager.GetSceneAt(scene);

            if (expectedScene != null)
                Debug.LogWarning($"Scene: {expectedScene.name} ({expectedScene.buildIndex}) IS loaded.");
            else
                Debug.LogError($"Scene: {scene} Doesn't Exist.");

        }
    }

    public void LoadScene(int[] scenes, LoadSceneMode mode, bool Async)
    {
        for (int i = 0; i < scenes.Length; i++)
            LoadScene(scenes[i], mode, Async);
    }

    public void LoadScene(string scenes, LoadSceneMode mode, bool Async)
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            if (!IsSceneLoaded(scenes))
            {
                if (Async)
                    SceneManager.LoadSceneAsync(name, mode);
                else
                    SceneManager.LoadScene(name, mode);
            }
            else
            {
                Scene expectedScene = SceneManager.GetSceneByName(scenes);

                if (expectedScene != null)
                    Debug.LogWarning($"Scene: {scenes} ({expectedScene.buildIndex}) IS loaded.");
                else
                    Debug.LogError($"Scene: {scenes} Doesn't Exist.");
            }
        }
    }

    public void LoadScene(string[] scenes, LoadSceneMode mode, bool Async)
    {
        for (int i = 0; i < scenes.Length; i++)
            LoadScene(scenes, mode, Async);
    }

    public void UnloadScene(int[] scenes)
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            int index = scenes[i];

            if (IsSceneLoaded(index))
            {
                SceneManager.UnloadSceneAsync(index);
            }
            else
            {
                Scene expectedScene = SceneManager.GetSceneAt(index);

                Debug.LogWarning($"Scene: {expectedScene.name} ({expectedScene.buildIndex}) INS'T loaded.");
            }
        }
    }

    public void UnloadScene(string[] scenes)
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            string name = scenes[i];

            if (IsSceneLoaded(name))
            {
                SceneManager.UnloadSceneAsync(name);
            }
            else
            {
                Scene expectedScene = SceneManager.GetSceneByName(scenes[i]);

                Debug.LogWarning($"Scene: {name} ({expectedScene.buildIndex}) isn't loaded.");
            }
        }
    }

    private bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name == sceneName)
                return true;
        }

        return false;
    }

    private bool IsSceneLoaded(int index)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.buildIndex == index)
                return true;
        }

        return false;
    }
}
