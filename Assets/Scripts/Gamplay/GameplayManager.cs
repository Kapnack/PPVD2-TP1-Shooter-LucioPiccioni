using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour, IGameplayManager
{
    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int enemiesToSpawn = 5;

    [SerializeField] private GameObject platform;

    private List<Characters> activeEnemies = new List<Characters>();

    private bool gameEnded = false;

    ISceneLoader sceneLoader;

    private void Awake()
    {
        if (ServiceProvider.TryGetService<ISceneLoader>(out var sceneLoader))
            this.sceneLoader = sceneLoader;

        Enemy.gameplayManager = this;
        Player.gameplayManager = this;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            if (platform != null)
                SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("<color=orange>[GameplayManager]</color> No hay enemigos en el array.");
            return;
        }

        Collider platformCollider = platform.GetComponent<Collider>();
        if (platformCollider == null)
        {
            Debug.LogError("<color=red>[GameplayManager]</color> La plataforma no tiene un Collider.");
            return;
        }

        Bounds bounds = platformCollider.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.max.y;

        Vector3 spawnPos = new Vector3(x, y, z);
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemyGO = Instantiate(prefab, spawnPos, Quaternion.identity);

        // ⬇️ ¡Mover a la misma escena del GameplayManager!
        SceneManager.MoveGameObjectToScene(enemyGO, gameObject.scene);

        if (enemyGO.TryGetComponent<Characters>(out var enemy))
        {
            activeEnemies.Add(enemy);
        }
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
        if (activeEnemies.Count > 0)
            return;

        if (!gameEnded)
        {
            gameEnded = true;
            OnPlayerWin();
        }
    }

    private void OnPlayerWin()
    {
        Debug.Log("<color=green>[GameplayManager]</color> ¡El jugador ha ganado!");
        LoadMainMenu();
    }

    private void OnPlayerLose()
    {
        Debug.Log("<color=red>[GameplayManager]</color> El jugador ha muerto. Has perdido.");

        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        sceneLoader.UnloadScene("PersistantGameplay");
        sceneLoader.UnloadScene("Gameplay");
        sceneLoader.LoadScene("MainMenu", LoadSceneMode.Additive);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
