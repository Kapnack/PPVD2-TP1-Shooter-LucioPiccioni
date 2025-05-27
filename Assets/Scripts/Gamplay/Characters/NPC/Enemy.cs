public class Enemy : Characters
{
    protected IPlayer iPlayer;
    static public IGameplayManager gameplayManager;

    protected virtual void Start()
    {
        OnAwake();

        if (ServiceProvider.TryGetService<IPlayer>(out var player))
            iPlayer = player;
    }

    protected virtual void DieAndNotify()
    {
        gameplayManager?.NotifyEnemyDeath(this);
        Destroy(gameObject);
    }

    protected override void OnDead()
    {
        DieAndNotify();
    }
}