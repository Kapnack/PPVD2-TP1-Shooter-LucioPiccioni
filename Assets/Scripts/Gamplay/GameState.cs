using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    InputReader inputReader;
    SceneLoader sceneLoader;

    private void Awake()
    {
        if (ServiceProvider.TryGetService<InputReader>(out var inputReader))
            this.inputReader = inputReader;

        if (ServiceProvider.TryGetService<SceneLoader>(out var sceneLoader))
            this.sceneLoader = sceneLoader;

        pauseMenu.SetActive(false);

    }

    private void Start()
    {
        HideCursor();
    }

    private void OnEnable()
    {
        inputReader.PauseEvent += OnPause;

       ShowCursor();
    }

    private void OnDisable()
    {
        inputReader.PauseEvent -= OnPause;
    }

    private void OnPause()
    {
        if (!pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);

            ShowCursor();

            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;

            pauseMenu.SetActive(false);

            HideCursor();

        }
    }

    public void ExitGame()
    {
#if !DEBUG
        Application.Quit();
#endif
    }

    public void GoToMenu()
    {
        Time.timeScale = 1.0f;

        sceneLoader.UnloadScene("Gameplay");

        sceneLoader.LoadScene("MainMenu", LoadSceneMode.Additive, false);

        sceneLoader.UnloadScene("PersistantGameplay");
    }

    public void Continue()
    {
        Time.timeScale = 1.0f;

        pauseMenu.SetActive(false);

        HideCursor();
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
