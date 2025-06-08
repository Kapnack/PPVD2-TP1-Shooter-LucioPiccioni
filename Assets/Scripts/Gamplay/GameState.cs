using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    InputReader inputReader;
    IGameManager iGameManager;

    private void Awake()
    {
        if (ServiceProvider.TryGetService<InputReader>(out var inputReader))
            this.inputReader = inputReader;

        if (ServiceProvider.TryGetService<IGameManager>(out var iGameManager))
            this.iGameManager = iGameManager;

        pauseMenu.SetActive(false);

        iGameManager.HideCursor();
    }

    private void OnEnable()
    {
        inputReader.PauseEvent += OnPause;
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

            iGameManager.ShowCursor();

            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;

            pauseMenu.SetActive(false);

            iGameManager.HideCursor();

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

        iGameManager.LoadMainMenu();
    }

    public void Continue()
    {
        Time.timeScale = 1.0f;

        pauseMenu.SetActive(false);

        iGameManager.HideCursor();
    }
}
