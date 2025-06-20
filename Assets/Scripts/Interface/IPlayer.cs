using System;
using UnityEngine;

public interface IPlayer
{
    public event Action OnAmmoChange;
    public event Action OnKill;
    public event Action OnHealthChange;

    public float CurrentHealth
    {
        get;
    }

    public int Kills
    {
        get;
    }

    public float CurrentShield
    {
        get;
    }

    public float MaxHealth
    {
        get;
    }

    public float MaxShield
    {
        get;
    }

    public float GetCurrentWeaponAmmo();
    public float GetCurrentWeaponMaxAmmo();

    public bool IsDead();
    Vector3 getPos();
    bool IsLookingAt(Vector3 position);
    void ReciveDamage(float amount);
    void ChangeInmortalState();
}
