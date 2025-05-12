using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] protected int damage;
    [SerializeField][Range(0, 1)] private float timeBetweenShots = 0.10f;
    private float lastShotTime = 0f;

    [Header("Rumble")]
    [SerializeField] protected float duration;
    [SerializeField][Range(0, 1)] protected float lowFrequency;
    [SerializeField][Range(0, 1)] protected float highFrequency;

    [SerializeField] public Magazine magazine = new Magazine();

    protected bool isHoldingFire = false;

    private float timeHoldingFire = 0;

    protected Characters owner;

    private void Awake()
    {
        owner = GetComponentInParent<Characters>();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        magazine.CancelReload();

        OnStopHoldingFire();
    }

    private void Update()
    {
        if (isHoldingFire)
        {
            timeHoldingFire += Time.deltaTime;

            if (timeHoldingFire > 0.1f)
                TryShoot();
        }

        if (magazine.Reloading)
        {
           magazine.DoneReloading();
        }
    }

    public abstract void Shoot();

    protected void TryShoot()
    {
        if (!magazine.IsEmpty() && !magazine.Reloading && Time.time - lastShotTime >= timeBetweenShots)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    public void TryReload()
    {
        if (!magazine.Reloading && !magazine.IsFull())
        {
           magazine.StartReload();
        }
    }

    public void OnHoldingFire() => isHoldingFire = true;

    protected void OnStopHoldingFire()
    {
        isHoldingFire = false;
        timeHoldingFire = 0;
    }

    private void OnValidate()
    {
        magazine.Validate();
    }
}
