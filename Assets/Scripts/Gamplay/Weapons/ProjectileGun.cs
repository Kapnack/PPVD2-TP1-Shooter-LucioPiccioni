using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileGun : Gun
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform tip;

    protected override void OnFire()
    {
        var newBullet = Instantiate(bulletPrefab, tip.transform.position, tip.transform.rotation);

        SceneManager.MoveGameObjectToScene(newBullet, gameObject.scene);

        Bullet bulletScrip = newBullet.GetComponent<Bullet>();

        bulletScrip.SetUpDamage(damage);

        bulletScrip.Fire();

        magazine.ReduceCurrentAmmo();

        if (rumbleManager != null)
            rumbleManager.RumblePulse(lowFrequency, highFrequency, duration);
    }
}