using System.Collections;
using UnityEngine;

public class Gun : BaseGun
{
    [SerializeField] float maxDistance = 0.0f;

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
        if (!ammo.IsEmpty())
        {
            Debug.Log("SHOOTING");

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, maxDistance))
            {
                Debug.Log(hitInfo.transform.name);
                Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red, 1f);

                if (hitInfo.transform.TryGetComponent<Enemy>(out Enemy enemyComponent))
                {
                    enemyComponent.OnDamage(damage);
                }
            }

            ammo.ReduceCurrentAmmo();

            RumbleManager.Instance.RumblePulse(lowFrequency, highFrequency, duration);
        }
    }
}
