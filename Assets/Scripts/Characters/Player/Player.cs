using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum WeaponsType
{
    HitScan,
    Projectile
}

public class Player : Characters
{
    [SerializeField] GameObject[] gunsPrefabs;
    private List<Gun> gunsScripts = new List<Gun>();

    private WeaponsType currentWeapon;

    private int kills = 0;
    public int Kills
    {
        get => kills;
    }

    private void Awake()
    {
        for (int i = 0; i < gunsPrefabs.Length; i++)
        {
            if (gunsPrefabs[i].TryGetComponent<Gun>(out var script))
            {
                gunsScripts.Add(script);

                if (i == 0)
                    currentWeapon = WeaponsType.HitScan;
            }
            else
            {
                gunsScripts.Add(gunsPrefabs[i].GetComponent<ProjectileGun>());

                if (i == 0)
                    currentWeapon = WeaponsType.Projectile;
            }

            gunsPrefabs[i].SetActive(i == 0);
        }
    }

    private void OnEnable()
    {
        InputReader.ChangeWeapon1Event += ChangeToWeapon1;
        InputReader.ChangeWeapon2Event += ChangeToWeapon2;
    }

    private void OnDisable()
    {
        InputReader.ChangeWeapon1Event -= ChangeToWeapon1;
        InputReader.ChangeWeapon2Event -= ChangeToWeapon2;
    }

    protected override void OnDead()
    {
        GameplayManager.Instance.NotifyPlayerDead();
    }

    private void ChangeWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= gunsPrefabs.Length)
            return;

        for (int i = 0; i < gunsPrefabs.Length; i++)
        {
            gunsPrefabs[i].SetActive(i == weaponIndex);
        }

        if (gunsPrefabs[weaponIndex].GetComponent<HitScanGun>() != null)
            currentWeapon = WeaponsType.HitScan;
        else
            currentWeapon = WeaponsType.Projectile;
    }

    private void MeleeAttack()
    {

    }

    private void ChangeToWeapon1()
    {
        ChangeWeapon(0);
    }

    private void ChangeToWeapon2()
    {
        ChangeWeapon(1);
    }

    public float GetCurrentWeaponAmmo()
    {
        return gunsScripts[(int)currentWeapon].magazine.CurrentAmmo;
    }

    public float GetCurrentWeaponMaxAmmo()
    {
        return gunsScripts[(int)currentWeapon].magazine.MaxAmmo;
    }
}
