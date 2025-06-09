using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    private ISceneLoader iSceneLoader;

    [Header("Scenes References")]
    [SerializeField] private SceneRef mainMenu;
    [SerializeField] private SceneRef persistantGameplay;
    [SerializeField] private SceneRef tutorial;
    [SerializeField] private List<SceneRef> levelScenes = new List<SceneRef>();

    private bool isTutorialCompleted = false;

    public bool IsTutorialCompleted
    {
        get => isTutorialCompleted;
    }
    
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

    [ContextMenu("Load Main Menu")]
    public void LoadMainMenu()
    {
        ShowCursor();

        iSceneLoader.UnloadAll();

        iSceneLoader.LoadScene(mainMenu);
    }
    
    [ContextMenu("Load Tutorial")]
    public void LoadTutorial()
    {
        iSceneLoader.UnloadAll(persistantGameplay);

        TryLoadPersistantGameplay();

        iSceneLoader.LoadScene(tutorial);
    }
    
    [ContextMenu("Load Current Level")]
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
    
    [ContextMenu("Complete Current Level")]
    public void LevelCompleted()
    {
        currentLevelIndex++;
        LoadCurrentLevel();
    }

    [ContextMenu("Complete Tutorial")]
    public void TutorialCompleted()
    {
        isTutorialCompleted = true;
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
