using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int enemiesToSpawn = 5;

    [SerializeField] private GameObject platform;

    private List<Characters> activeEnemies = new List<Characters>();
    private IPlayer player;

    private bool gameEnded = false;

    private void Start()
    {
        if (ServiceProvider.TryGetService<IPlayer>(out var iPlayer))
            player = iPlayer;

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

        if (enemyGO.TryGetComponent<Characters>(out var enemy))
        {
            activeEnemies.Add(enemy);
        }
    }

    private void CheckGameState()
    {
        if (gameEnded) return;

        if (player == null || player.IsDead())
        {
            gameEnded = true;
            OnPlayerLose();

            return;
        }

        bool allEnemiesDead = true;
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null && !enemy.IsDead())
            {
                allEnemiesDead = false;
                break;
            }
        }

        if (allEnemiesDead)
        {
            gameEnded = true;
            OnPlayerWin();
        }
    }

    private void OnPlayerWin()
    {
        Debug.Log("<color=green>[GameplayManager]</color> ¡El jugador ha ganado!");
    }

    private void OnPlayerLose()
    {
        Debug.Log("<color=red>[GameplayManager]</color> El jugador ha muerto. Has perdido.");
    }
}
