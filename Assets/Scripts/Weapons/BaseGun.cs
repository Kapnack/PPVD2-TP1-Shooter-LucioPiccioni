using System.Collections;
using UnityEngine;

public abstract class BaseGun : MonoBehaviour
{
    [SerializeField] protected GunData GunDataPrefab;
    protected GunData gunData;

    protected float timeSinceLastShot = 0;

    private void Awake()
    {
        gunData = Instantiate(GunDataPrefab);
    }

    protected abstract void OnReload();

    protected abstract IEnumerator Reload();

    public abstract void OnShoot();

    protected abstract bool CanShoot();
}
