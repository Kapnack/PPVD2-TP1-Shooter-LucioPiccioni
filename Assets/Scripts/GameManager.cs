using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    private ISceneLoader iSceneLoader;

    [SerializeField] private SceneRef mainMenu;

    [SerializeField] private SceneRef persistantGameplay;

    [SerializeField] private List<SceneRef> levelScenes = new List<SceneRef>();

    private int currentLevelIndex = 0;

    public int CurrentLevelIndex
    {
        get => currentLevelIndex;
    }

    private void Awake()
    {
        ServiceProvider.SetService<IGameManager>(this);
    }

    private IEnumerator Start()
    {
        ISceneLoader iSceneLoader;

        while (!ServiceProvider.TryGetService<ISceneLoader>(out iSceneLoader))
            yield return null;

        this.iSceneLoader = iSceneLoader;

#if !UNITY_EDITOR
        LoadMainMenu();
#endif
    }


    private void TryLoadPersistantGameplay()
    {
        if (!iSceneLoader.IsSceneLoaded(persistantGameplay))
        {
            HideCursor();
            iSceneLoader.LoadScene(persistantGameplay);
        }
    }

    public void LoadMainMenu()
    {
        ShowCursor();

        iSceneLoader.UnloadAll();

        iSceneLoader.LoadScene(mainMenu);
    }
    public void LoadTutorial()
    {
        iSceneLoader.UnloadAll(persistantGameplay);

        TryLoadPersistantGameplay();

        iSceneLoader.LoadScene(levelScenes[0]);
    }
    public void LoadCurrentLevel()
    {
        iSceneLoader.UnloadAll(persistantGameplay);

        TryLoadPersistantGameplay();

        if (currentLevelIndex < levelScenes.Count)
            iSceneLoader.LoadScene(levelScenes[currentLevelIndex]);
        else
        {
            currentLevelIndex--;
            LoadMainMenu();
        }
    }

    public void LevelCompleted()
    {
        currentLevelIndex++;

        LoadCurrentLevel();
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
