using UnityEngine;

public class Enemy : Characters
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
