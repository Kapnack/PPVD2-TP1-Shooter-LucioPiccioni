using System;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    public event Action OnPlayerDead;

    public void NotifyPlayerDead()
    {
        OnPlayerDead?.Invoke();
    }
}
