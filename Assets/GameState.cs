using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    private void OnEnable()
    {
        InputReader.Instance.PauseEvent += OnPause;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnPause()
    {
        if (!SceneManager.GetSceneByName("Pause").isLoaded)
        {
            SceneManager.LoadScene("Pause", LoadSceneMode.Additive);

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            Time.timeScale = 0.0f;
        }
        else
        {
            SceneManager.UnloadSceneAsync("Pause");

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1.0f;
        }
    }
}
