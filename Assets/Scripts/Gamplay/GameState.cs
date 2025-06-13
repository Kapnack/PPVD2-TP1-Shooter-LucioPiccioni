using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    IInputReader iInputReader;
    IGameManager iGameManager;
    IEventSystemManager iEventSystemManager;

    [SerializeField] GridLayoutGroup gridLayoutGroup;

    private void Awake()
    {
        gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();

        if (ServiceProvider.TryGetService<IInputReader>(out var iInputReader))
            this.iInputReader = iInputReader;

        if (ServiceProvider.TryGetService<IGameManager>(out var iGameManager))
            this.iGameManager = iGameManager;

        if (ServiceProvider.TryGetService<IEventSystemManager>(out var iEventSystemManager))
            this.iEventSystemManager = iEventSystemManager;

        pauseMenu.SetActive(false);

        iGameManager.HideCursor();
    }

    private void OnEnable()
    {
        iInputReader.PauseEvent += OnPause;
    }

    private void OnDisable()
    {
        iInputReader.PauseEvent -= OnPause;
    }

    private void OnPause()
    {
        if (!pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);

            iEventSystemManager.SetSelectedObject(gridLayoutGroup);

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
