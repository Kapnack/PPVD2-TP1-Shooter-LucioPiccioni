using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void LoadGameplay() => SceneLoader.Instance.LoadGameplay();
    public void LoadTutorial() => SceneLoader.Instance.LoadTutorial();
    public void LoadCredits() => SceneLoader.Instance.LoadCredits();
    public void LoadConfirmExit() => SceneLoader.Instance.LoadConfirmExit();
}
