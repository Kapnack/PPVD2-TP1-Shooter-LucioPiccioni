using UnityEngine;

public interface IGameplayManager
{
    void NotifyEnemyDeath(Characters enemy);
    void NotifyPlayerDeath();
}
