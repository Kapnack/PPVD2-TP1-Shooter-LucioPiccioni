using System.Collections;
using UnityEngine;

public class ProjectileGun : Gun
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform tip;

    public override void Shoot()
    {
        var newBullet = Instantiate(bulletPrefab, tip.transform.position, tip.transform.rotation);

        Bullet bulletScrip = newBullet.GetComponent<Bullet>();

        bulletScrip.SetUpDamage(damage);

        bulletScrip.Fire();

        magazine.ReduceCurrentAmmo();

        RumbleManager.Instance.RumblePulse(lowFrequency, highFrequency, duration);
    }
}