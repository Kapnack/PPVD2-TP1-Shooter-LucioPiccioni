using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    private ISceneLoader sceneLoader;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject credits;

    [SerializeField] private InputActionReference actionRef;

    private bool isCredits = false;

    private void Awake()
    {
        if (ServiceProvider.TryGetService<ISceneLoader>(out var loader))
            sceneLoader = loader;

        credits.SetActive(false);
    }

    private void OnEnable()
    {
        if (actionRef != null && actionRef.action != null)
            actionRef.action.performed += OnCreditsInput;
    }

    private void OnDisable()
    {
        if (actionRef != null && actionRef.action != null)
            actionRef.action.performed -= OnCreditsInput;
    }

    public void LoadGameplay()
    {
        sceneLoader.UnloadScene("MainMenu");
        sceneLoader.LoadScene("Gameplay", UnityEngine.SceneManagement.LoadSceneMode.Additive, false);
    }

    public void LoadTutorial()
    {
        sceneLoader.UnloadScene("MainMenu");
        sceneLoader.LoadScene("Tutorial", UnityEngine.SceneManagement.LoadSceneMode.Additive, false);
    }

    public void LoadCredits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
        isCredits = true;

        if (actionRef != null && actionRef.action != null)
            actionRef.action.Enable();
    }

    public void GoToMainMenu()
    {
        credits.SetActive(false);
        mainMenu.SetActive(true);
        isCredits = false;
    }

    public void ExitGame()
    {
#if !DEBUG
        Application.Quit();
#endif
    }

    private void OnCreditsInput(InputAction.CallbackContext ctx)
    {
        if (isCredits)
        {
            GoToMainMenu();
        }
    }
}
