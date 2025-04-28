using UnityEngine;

public class Enemy : ChactersBase
{

    void Update()
    {

    }

    protected override void OnDead()
    {
        Destroy(gameObject);
        PlayerHudManager.Instance.UpdateKillsHud();
    }
}
