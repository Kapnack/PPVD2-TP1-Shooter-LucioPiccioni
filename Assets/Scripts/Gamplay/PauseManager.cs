using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    SceneLoader sceneLoader;

    public void Awake()
    {
        if (ServiceProvider.TryGetService<SceneLoader>(out var sceneLoader))
            this.sceneLoader = sceneLoader;
    }

    private void OnEnable()
    {
        Time.timeScale = 0.0f;
    }

    public void WantToExit()
    {
        sceneLoader.LoadScene("ConfirmExit", LoadSceneMode.Additive, false);
    }

    public void GoToMenu()
    {
        sceneLoader.LoadScene("MainMenu", LoadSceneMode.Additive, false);

        Time.timeScale = 1.0f;
    }

    public void Continue()
    {
        Time.timeScale = 1.0f;

        gameObject.SetActive(false);
    }
}
