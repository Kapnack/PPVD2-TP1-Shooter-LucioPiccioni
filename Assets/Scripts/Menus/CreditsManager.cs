using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public void GoBackToMenu() => SceneLoader.Instance.LoadMainMenu();
}
