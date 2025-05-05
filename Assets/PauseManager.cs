using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    AsyncOperation scene;

    public void Awake()
    {
    }

    public void WantToExit()
    {
        SceneLoader.Instance.LoadConfirmExit();
    }

    public void GoToMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
        Time.timeScale = 1.0f;
    }

    public void Continue()
    {
        scene = SceneManager.UnloadSceneAsync("Pause");

        StartCoroutine(UnloadScene());

        Time.timeScale = 1.0f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private IEnumerator UnloadScene()
    {
        while (!scene.isDone)
            yield return null;
    }
}
