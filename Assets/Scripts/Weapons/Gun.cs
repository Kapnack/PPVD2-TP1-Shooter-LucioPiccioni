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

    private bool isReloading = false;

    protected bool isHoldingFire = false;

    private float timeHoldingFire = 0;

    protected Characters owner;

    private void Awake()
    {
        owner = GetComponentInParent<Characters>();
    }

    private void OnEnable()
    {
        InputReader.Instance.FireEvent += TryShoot;
        InputReader.Instance.HoldigFireEvent += OnHoldingFire;
        InputReader.Instance.StopHoldigFireEvent += OnStopHoldingFire;

        InputReader.Instance.ReloadEvent += TryReload;

        InputReader.Instance.MeleeAttackEvent += CancelReload;

        magazine.OnEnable();
    }

    private void OnDisable()
    {
        InputReader.Instance.FireEvent -= TryShoot;
        InputReader.Instance.HoldigFireEvent -= OnHoldingFire;
        InputReader.Instance.StopHoldigFireEvent -= OnStopHoldingFire;

        InputReader.Instance.ReloadEvent -= TryReload;

        InputReader.Instance.MeleeAttackEvent -= CancelReload;

        magazine.OnDisable();

        OnStopHoldingFire();

        isReloading = false;
    }

    private void Update()
    {
        if (isHoldingFire)
        {
            timeHoldingFire += Time.deltaTime;

            if (timeHoldingFire > 0.1f)
                TryShoot();
        }

        if (isReloading)
        {
            isReloading = magazine.Reloading();
        }
    }

    public abstract void Shoot();

    protected void TryShoot()
    {
        if (!magazine.IsEmpty() && !isReloading && Time.time - lastShotTime >= timeBetweenShots)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    protected void TryReload()
    {
        if (!isReloading && !magazine.IsFull())
        {
            isReloading = true;
        }
    }

    public void CancelReload() => isReloading = false;

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
