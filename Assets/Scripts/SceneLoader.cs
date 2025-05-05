using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using Unity.Loading;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private List<string> scenesToKeep = new List<string> { "PersistantScene" };

    protected override void Awake()
    {
        base.Awake();
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void LoadMainMenu()
    {
        if (scenesToKeep.Contains("PersistantGameplay"))
            scenesToKeep.Remove("PersistantGameplay");

        StartCoroutine(UnloadAllExpetNeeded());

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }

    public void LoadCredits()
    {
        SceneManager.UnloadSceneAsync("MainMenu");
        SceneManager.LoadScene("Credits", LoadSceneMode.Additive);
    }

    public void LoadTutorial()
    {
        if (!scenesToKeep.Contains("PersistantGameplay"))
            scenesToKeep.Add("PersistantGameplay");

        StartCoroutine(UnloadAllExpetNeeded());

        if (!SceneManager.GetSceneByName("PersistantGameplay").isLoaded)
            SceneManager.LoadScene("PersistantGameplay", LoadSceneMode.Additive);

        SceneManager.LoadSceneAsync("Tutorial", LoadSceneMode.Additive);
    }

    public void LoadGameplay()
    {
        if (!scenesToKeep.Contains("PersistantGameplay"))
            scenesToKeep.Add("PersistantGameplay");

        StartCoroutine(UnloadAllExpetNeeded());

        if (!SceneManager.GetSceneByName("PersistantGameplay").isLoaded)
            SceneManager.LoadScene("PersistantGameplay", LoadSceneMode.Additive);

        SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);
    }

    public void LoadConfirmExit() => SceneManager.LoadSceneAsync("ConfirmExit", LoadSceneMode.Additive);

    private IEnumerator UnloadAllExpetNeeded()
    {
        int sceneCount = SceneManager.sceneCount;

        List<Scene> scenesToUnload = new List<Scene>();

        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (!scenesToKeep.Contains(scene.name))
            {
                scenesToUnload.Add(scene);
            }
        }

        foreach (Scene scene in scenesToUnload)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(scene);

            while (!asyncUnload.isDone)
            {
                yield return null;
            }
        }
    }
}
