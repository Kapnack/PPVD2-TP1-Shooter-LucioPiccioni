using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour, IGameplayManager
{
    [Header("TutorialSetting")]
    [SerializeField] private bool isTutorial = false;

    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int enemiesToSpawn = 5;


    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();


    private List<Characters> activeEnemies = new List<Characters>();
    private bool gameEnded = false;

    private ISceneLoader sceneLoader;
    private int spawnPointIndex;

    private void Awake()
    {
        ServiceProvider.SetService<IGameplayManager>(this, true);

        if (ServiceProvider.TryGetService<ISceneLoader>(out var loader))
            sceneLoader = loader;

        Enemy.gameplayManager = this;
        Player.gameplayManager = this;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            if (spawnPoints.Count != 0)
                SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("<color=orange>[GameplayManager]</color> No enemies found in the array.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("<color=yellow>[GameplayManager]</color> No spawn points assigned.");
            return;
        }

        Transform spawnPos = spawnPoints[spawnPointIndex];
        spawnPointIndex = (spawnPointIndex + 1) % spawnPoints.Count;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        GameObject enemyGO = Instantiate(prefab, spawnPos.position, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(enemyGO, gameObject.scene);

        if (enemyGO.TryGetComponent<Characters>(out var enemy))
            activeEnemies.Add(enemy);
    }


    public void NotifyEnemyDeath(Characters enemy)
    {
        if (gameEnded) return;

        activeEnemies.Remove(enemy);
        CheckVictoryCondition();
    }

    public void NotifyPlayerDeath()
    {
        if (gameEnded) return;

        gameEnded = true;
        OnPlayerLose();
    }

    private void CheckVictoryCondition()
    {
        if (activeEnemies.Count > 0) return;

        if (!gameEnded)
        {
            gameEnded = true;
            OnPlayerWin();
        }
    }

    private void OnPlayerWin()
    {
        Debug.Log("<color=green>[GameplayManager]</color> Player has won!");

        LoadNextLevel();
    }

    private void OnPlayerLose()
    {
        Debug.Log("<color=red>[GameplayManager]</color> Player is dead. You lost.");
        LoadMainMenu();
    }

    public void LoadNextLevel()
    {
        if (isTutorial)
        {
            Debug.Log("<color=blue>[GameplayManager]</color> Tutorial completed. Loading Gameplay...");
            StartCoroutine(LoadGameplayCoroutine());
        }
        else
        {
            Debug.Log("<color=magenta>[GameplayManager]</color> Level completed. Returning to main menu.");
            LoadMainMenu();
        }
    }

    private IEnumerator LoadGameplayCoroutine()
    {
        sceneLoader.UnloadScene("PersistantGameplay");
 
        yield return new WaitForSeconds(0.25f);

        sceneLoader.LoadScene("PersistantGameplay", LoadSceneMode.Additive);

        yield return new WaitForSeconds(0.25f);

        sceneLoader.LoadScene("Gameplay", LoadSceneMode.Additive);

        yield return new WaitForSeconds(0.25f);

        HideCursor();
        sceneLoader.UnloadScene("Tutorial");
    }


    private void LoadMainMenu()
    {
        sceneLoader.UnloadScene("PersistantGameplay");

        if (isTutorial)
            sceneLoader.UnloadScene("Tutorial");
        else
            sceneLoader.UnloadScene("Gameplay");

        sceneLoader.LoadScene("MainMenu", LoadSceneMode.Additive);
        ShowCursor();
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
