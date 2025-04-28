using System.Collections;
using UnityEngine;

public abstract class BaseGun : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] protected int damage;
    [SerializeField] private float fireRate = 600f; // rounds per minute
    private float timeBetweenShots;
    private float lastShotTime = 0f;

    [Header("Rumble")]
    [SerializeField] protected float duration;
    [SerializeField][Range(0, 1)] protected float lowFrequency;
    [SerializeField][Range(0, 1)] protected float highFrequency;

    [SerializeField] public Ammo ammo = new Ammo();

    Coroutine realoadRef = null;

    protected bool isHoldingFire = false;
    protected bool reloading = false;

    private float timeHoldingFire = 0;

    private void Awake()
    {
        timeBetweenShots = 60f / fireRate;
    }

    private void OnEnable()
    {
        InputReader.FireEvent += TryShoot;
        InputReader.HoldigFireEvent += OnHoldingFire;
        InputReader.StopHoldigFireEvent += OnStopHoldingFire;

        InputReader.ReloadEvent += TryReload;

        InputReader.MeleeAttackEvent += CancelReload;
        InputReader.MeleeAttackEvent += IsDoneReaload;

        ammo.FinishReloading += IsDoneReaload;
    }

    private void OnDisable()
    {
        InputReader.FireEvent -= TryShoot;
        InputReader.HoldigFireEvent -= OnHoldingFire;
        InputReader.StopHoldigFireEvent -= OnStopHoldingFire;

        InputReader.ReloadEvent -= TryReload;

        InputReader.MeleeAttackEvent -= CancelReload;
        InputReader.MeleeAttackEvent -= IsDoneReaload;

        ammo.FinishReloading -= IsDoneReaload;
    }

    private void Update()
    {
        if (isHoldingFire)
        {
            timeHoldingFire += Time.deltaTime;

            if (timeHoldingFire > 0.1f)
                TryShoot();
        }
    }

    public abstract void Shoot();

    protected void TryShoot()
    {
        if (!ammo.IsEmpty() && !reloading && Time.time - lastShotTime >= timeBetweenShots)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    protected void TryReload()
    {
        if (!reloading && !ammo.IsFull())
        {
            reloading = true;
            realoadRef = StartCoroutine(ammo.OnReload());
        }
    }

    public void CancelReload()
    {
        if (realoadRef != null)
        {
            StopCoroutine(realoadRef);
        }
    }

    protected void IsDoneReaload() => reloading = false;

    public void OnHoldingFire() => isHoldingFire = true;

    protected void OnStopHoldingFire()
    {
        isHoldingFire = false;
        timeHoldingFire = 0;
    }

    private void OnValidate()
    {
        ammo.Validate();
    }
}
