using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class Ammo
{
    [SerializeField] private float maxAmmo;
    [SerializeField] private float consumtionRate;
    [SerializeField] private float reloadingTime;
    private float currentAmmo;

    public event Action FinishReloading;

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

    public void ReduceCurrentAmmo() => currentAmmo--;

    public virtual IEnumerator OnReload()
    {
        yield return new WaitForSeconds(reloadingTime);

        CurrentAmmo = maxAmmo;

        FinishReloading?.Invoke();

        PlayerHudManager.Instance.UpdateAmmoHud();
    }

    public bool IsEmpty() => CurrentAmmo == 0;
    public bool IsFull() => CurrentAmmo == maxAmmo;

    public void Validate()
    {
        if (maxAmmo < 0) maxAmmo = 0;
        currentAmmo = maxAmmo;
    }
}
