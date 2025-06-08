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
    }

    public void GoToMenu()
    {
        Time.timeScale = 1.0f;
    }

    public void Continue()
    {
        Time.timeScale = 1.0f;

        gameObject.SetActive(false);
    }
}
