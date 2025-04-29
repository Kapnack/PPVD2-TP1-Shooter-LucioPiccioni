using UnityEngine;

public class HitScanGun : Gun
{
    [SerializeField] float maxDistance = 0.0f;

    public override void Shoot()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, maxDistance))
        {
            Debug.Log(hitInfo.transform.name);
            Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red, 1f);

            if (hitInfo.transform.TryGetComponent<Enemy>(out Enemy enemyScript))
            {
                enemyScript.ReduceHealth(damage);
            }
        }

        magazine.ReduceCurrentAmmo();

        RumbleManager.Instance.RumblePulse(lowFrequency, highFrequency, duration);

    }
}