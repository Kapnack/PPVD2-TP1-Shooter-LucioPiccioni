using UnityEngine;

public interface IGameplayManager
{
    public void NotifyEnemyDeath(Characters enemy);
    public void NotifyPlayerDeath();
    public void LoadNextLevel();
}
