using System.Collections;
using UnityEngine;

public class ProjectileGun : BaseGun
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform tip;
    bool isHoldingFire = false;

    private float timeHoldingFire = 0;

    private void OnEnable()
    {
        InputReader.FireEvent += OnShoot;
        InputReader.HoldigFireEvent += OnHoldingFire;
        InputReader.StopHoldigFireEvent += OnStopHoldingFire;

        InputReader.ReloadEvent += OnReload;
    }

    private void OnDisable()
    {
        InputReader.FireEvent -= OnShoot;
        InputReader.HoldigFireEvent -= OnHoldingFire;
        InputReader.StopHoldigFireEvent -= OnStopHoldingFire;

        InputReader.ReloadEvent -= OnReload;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (isHoldingFire)
        {
            timeHoldingFire += Time.deltaTime;

            if (timeHoldingFire > 0.1f)
                OnShoot();
        }
    }

    protected override void OnReload()
    {
        if (!gunData.reloading)
            StartCoroutine(Reload());
    }

    protected override IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadingTime);

        gunData.currentAmmo = gunData.maxAmmo;

        gunData.reloading = false;
    }

    public override void OnShoot()
    {
        if (CanShoot())
        {
            var newBullet = Instantiate(bulletPrefab, tip.transform.position, tip.transform.rotation);

            newBullet.GetComponent<Bullet>().Fire();

            gunData.currentAmmo -= gunData.consumtionRate;
            timeSinceLastShot = 0;

            RumbleManager.Instance.RumblePulse(gunData.lowFrequency, gunData.highFrequency, gunData.duration);
        }
    }

    public void OnHoldingFire() => isHoldingFire = true;
    public void OnStopHoldingFire()
    {
        isHoldingFire = false;
        timeHoldingFire = 0;
    }

    protected override bool CanShoot() => gunData.currentAmmo > 0 && !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
}