using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public int damage;
    public float maxDistance;
    public int consumtionRate;

    [HideInInspector] public int currentAmmo;
    [Header("Reloading")]
    public int maxAmmo;
    public float fireRate;
    public float reloadingTime;

    [Header("Rumble")]
    public float duration;
    [SerializeField] [Range(0,1)] public float lowFrequency;
    [SerializeField] [Range(0,1)] public float highFrequency;

    [HideInInspector]
    public bool reloading = false;

    private void OnValidate()
    {
        if (maxAmmo < 0)
            maxAmmo = 0;

        if (consumtionRate < 0)
            consumtionRate = 0;

        if (maxDistance < 1)
            maxDistance = 1;

        currentAmmo = maxAmmo;
    }
}
