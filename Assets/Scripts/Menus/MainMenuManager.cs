using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    private IGameManager iGameManager;

    [Header("Canvas")] [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject credits;

    [Header("ESC Action")] [SerializeField]
    private @InputSystem_Actions action;

    [Header("Bot�n de Jugar")] [SerializeField]
    private GameObject playButton;

    private void Awake()
    {
        if (ServiceProvider.TryGetService<IGameManager>(out var gameManager))
            this.iGameManager = gameManager;

        credits.SetActive(false);

        if (gameManager.CurrentLevelIndex > 0 && playButton != null)
            playButton.SetActive(true);
        else if (playButton != null)
            playButton.SetActive(false);
    }

    public void LoadGameplay()
    {
        if (iGameManager.IsTutorialCompleted)
            iGameManager.LoadCurrentLevel();
    }

    public void LoadTutorial()
    {
        iGameManager.LoadTutorial();
    }

    public void LoadCredits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);

        if (action != null)
            action.UI.Confirm.started += OnCreditsInput;
    }

    public void GoToMainMenu()
    {
        credits.SetActive(false);
        mainMenu.SetActive(true);

        if (action != null)
            action.UI.Confirm.started -= OnCreditsInput;
    }

    public void ExitGame()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }

    private void OnCreditsInput(InputAction.CallbackContext ctx)
    {
        GoToMainMenu();
    }
}