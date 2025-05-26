using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private ISceneLoader sceneLoader;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject credits;

    private void Awake()
    {
        if (ServiceProvider.TryGetService<ISceneLoader>(out var sceneLoader))
            this.sceneLoader = sceneLoader;

        credits.SetActive(false);
    }

    private void Update()
    {
        
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
    }

    public void GoToMainMenu()
    {
        credits.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void LoadConfirmExit() => sceneLoader.LoadScene("ConfirmExit", UnityEngine.SceneManagement.LoadSceneMode.Additive, false);
}
