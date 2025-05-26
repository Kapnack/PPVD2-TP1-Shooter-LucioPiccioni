using UnityEngine;

public class Enemy : Characters
{
    public static IPlayer iPlayer;
    protected override void OnDead()
    {
        Destroy(gameObject);
    }
}