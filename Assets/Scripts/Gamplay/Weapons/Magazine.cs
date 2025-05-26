using UnityEngine;
using System;

[Serializable]
public class Magazine
{
    [SerializeField] private float maxAmmo;
    [SerializeField] private float consumtionRate;
    [SerializeField] private float reloadingTime;
    private float currentAmmo;

    float realoadStarted = 0.0f;

    bool reloading = false;

    public float MaxAmmo
    {
        get => maxAmmo;
    }
    public float CurrentAmmo
    {
        get => currentAmmo;
        set => currentAmmo = Mathf.Clamp(value, 0, maxAmmo);
    }
    public float ConsumtionRate
    {
        get => consumtionRate;

        set => consumtionRate = value;
    }

    public bool Reloading
    {
        get => reloading;
    }

    public void ReduceCurrentAmmo() => currentAmmo--;

    public void DoneReloading()
    {
        reloading = true;

        if (Time.time > realoadStarted + reloadingTime && reloading)
        {
            CurrentAmmo = maxAmmo;
            reloading = false;
        }
    }

    public void StartReload()
    {
        if (!reloading)
        {
            realoadStarted = Time.time;
            reloading = true;
        }
    }

    public void CancelReload() => reloading = false;

    public bool IsEmpty() => CurrentAmmo == 0;
    public bool IsFull() => CurrentAmmo == maxAmmo;

    public void Validate()
    {
        if (maxAmmo < 0) maxAmmo = 0;
        currentAmmo = maxAmmo;
    }
}
