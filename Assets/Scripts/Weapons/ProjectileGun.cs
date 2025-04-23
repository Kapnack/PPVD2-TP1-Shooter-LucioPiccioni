using System.Collections;
using UnityEngine;

public class ProjectileGun : BaseGun
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform tip;

    protected override void Reload()
    {
        if (!reloading && !ammo.IsFull())
        {
            reloading = true;
            StartCoroutine(ammo.OnReload());
        }
    }

    public override void Shoot()
    {
        var newBullet = Instantiate(bulletPrefab, tip.transform.position, tip.transform.rotation);

        Bullet bulletScrip = newBullet.GetComponent<Bullet>();

        bulletScrip.SetUpDamage(damage);

        bulletScrip.Fire();

        ammo.ReduceCurrentAmmo();

        RumbleManager.Instance.RumblePulse(lowFrequency, highFrequency, duration);
    }
}