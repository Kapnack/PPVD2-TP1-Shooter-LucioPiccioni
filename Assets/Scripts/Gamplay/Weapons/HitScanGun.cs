using UnityEngine;

public class HitScanGun : Gun
{
    [SerializeField] float maxDistance = 0.0f;

    protected override void OnFire()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, maxDistance))
        {
            Debug.Log(hitInfo.transform.name);
            Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red, 1f);

            if (hitInfo.transform.TryGetComponent<Enemy>(out Enemy enemyScript))
            {
                enemyScript.ReciveDamage(damage);

                if (enemyScript.IsDead() && owner is Player player)
                    player.AddKill();
            }
        }

        magazine.ReduceCurrentAmmo();

        if (rumbleManager != null)
            rumbleManager.RumblePulse(lowFrequency, highFrequency, duration);

    }
}