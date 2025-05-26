using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        ServiceProvider.SetService(this);

        LoadScene("MainMenu", LoadSceneMode.Additive, false);
    }

    public void LoadScene(int index, LoadSceneMode mode, bool Async)
    {
        if (IsPlayableScene(index) && !IsSceneLoaded(2))
            SceneManager.LoadScene(2, LoadSceneMode.Single);

        if (!IsSceneLoaded(index))
        {
            if (Async)
                SceneManager.LoadSceneAsync(index, mode);
            else
                SceneManager.LoadScene(index, mode);
        }
        else
        {
            Scene expectedScene = SceneManager.GetSceneAt(index);

            if (expectedScene != null)
                Debug.LogWarning($"Scene: {expectedScene.name} ({expectedScene.buildIndex}) IS loaded.");
            else
                Debug.LogError($"Scene: {index} Doesn't Exist.");

        }
    }

    public void LoadScene(int[] scenes, LoadSceneMode mode, bool Async)
    {
        for (int i = 0; i < scenes.Length; i++)
            LoadScene(scenes[i], mode, Async);
    }

    public void LoadScene(string scene, LoadSceneMode mode, bool Async)
    {
        if (IsPlayableScene(scene) && !IsSceneLoaded("PersistantGameplay"))
            SceneManager.LoadScene("PersistantGameplay", LoadSceneMode.Additive);

        if (!IsSceneLoaded(scene))
        {
            if (Async)
                SceneManager.LoadSceneAsync(scene, mode);
            else
                SceneManager.LoadScene(scene, mode);
        }
        else
        {
            Scene expectedScene = SceneManager.GetSceneByName(scene);

            if (expectedScene != null)
                Debug.LogWarning($"Scene: {scene} ({expectedScene.buildIndex}) IS loaded.");
            else
                Debug.LogError($"Scene: {scene} Doesn't Exist.");
        }
    }

    public void LoadScene(string[] scenes, LoadSceneMode mode, bool Async)
    {
        for (int i = 0; i < scenes.Length; i++)
            LoadScene(scenes, mode, Async);
    }

    public void UnloadScene(int index)
    {
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

    public void UnloadScene(int[] scenes)
    {
        for (int i = 0; i < scenes.Length; i++)
            UnloadScene(scenes[i]);
    }

    public void UnloadScene(string scene)
    {
        if (IsSceneLoaded(scene))
        {
            SceneManager.UnloadSceneAsync(scene);
        }
        else
        {
            Scene expectedScene = SceneManager.GetSceneByName(scene);

            Debug.LogWarning($"Scene: {name} ({expectedScene.buildIndex}) isn't loaded.");
        }
    }

    public void UnloadScene(string[] scenes)
    {
        for (int i = 0; i < scenes.Length; i++)
            UnloadScene(scenes[i]);
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

    private bool IsPlayableScene(string scene)
    {
        if (scene == "Gameplay" || scene == "Tutorial")
            return true;

        return false;
    }

    private bool IsPlayableScene(int scene)
    {
        if (scene == 4 || scene == 5)
            return true;

        return false;
    }
}
