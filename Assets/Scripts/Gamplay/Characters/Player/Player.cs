﻿using System;
using UnityEngine;

public enum Slots
{
    Slot0,
    Slot1
}

public class Player : Characters, IPlayer
{
    public static IGameplayManager gameplayManager;

    [Header("Dependencies")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform weaponGrabPos;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Weapon Settings")]
    [SerializeField] private GameObject[] gunsObj = new GameObject[2];
    [SerializeField] private float grabDistance = 25.0f;
    [SerializeField] float grabSphereRadius = 1.25f;

    [Header("Meele Attack")]
    [SerializeField] float rangeX = 2f;
    [SerializeField] float rangeY = 2f;

    [Header("View Settings")]
    [SerializeField] private float viewAngleThreshold = 30f;

    private Gun[] gunsScripts = new Gun[2];
    private IInputReader iInputReader;

    private Slots currentSlot;
    private bool isHoldingFire = false;

    private bool isInmortal = false;

    private int _kills = 0;
    public int Kills
    {
        get => _kills;
    }

    public event Action OnAmmoChange;
    public event Action OnKill;
    public event Action OnHealthChange;

    private void Awake()
    {
        OnAwake();

        if (ServiceProvider.TryGetService<IInputReader>(out var iInputReader))
            this.iInputReader = iInputReader;

        for (int i = 0; i < gunsObj.Length; i++)
        {
            if (gunsObj[i].TryGetComponent<Gun>(out var script))
            {
                gunsScripts[i] = script;
                gunsScripts[i].Grab(this, weaponGrabPos);
            }

            gunsObj[i].SetActive(i == 0);
        }

        ServiceProvider.SetService<IPlayer>(this, true);
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        iInputReader.FireEvent += Fire;
        iInputReader.HoldigFireEvent += HoldingFire;
        iInputReader.StopHoldigFireEvent += StopFire;

        iInputReader.ReloadEvent += Reload;
        iInputReader.ChangeWeapon1Event += ChangeToWeapon1;
        iInputReader.ChangeWeapon2Event += ChangeToWeapon2;

        iInputReader.InteractEvent += GrabWeapon;
        iInputReader.DropWeaponEvent += DropGun;

        iInputReader.MeleeAttackEvent += MeleeAttack;
    }

    private void OnDisable()
    {
        iInputReader.FireEvent -= Fire;
        iInputReader.HoldigFireEvent -= HoldingFire;
        iInputReader.StopHoldigFireEvent -= StopFire;

        iInputReader.ReloadEvent -= Reload;
        iInputReader.ChangeWeapon1Event -= ChangeToWeapon1;
        iInputReader.ChangeWeapon2Event -= ChangeToWeapon2;

        iInputReader.InteractEvent -= GrabWeapon;
        iInputReader.DropWeaponEvent -= DropGun;

        iInputReader.MeleeAttackEvent -= MeleeAttack;
    }

    private void Update()
    {
        if (isHoldingFire)
            Fire();
    }

    protected override void OnDead()
    {
        gameplayManager?.NotifyPlayerDeath();
    }

    private void Fire()
    {
        if (gunsScripts[(int)currentSlot] != null)
        {
            gunsScripts[(int)currentSlot].TryFire();
            OnAmmoChange?.Invoke();
        }
        else
            MeleeAttack();

    }

    private void HoldingFire() => isHoldingFire = true;
    private void StopFire() => isHoldingFire = false;

    private void Reload()
    {
        gunsScripts[(int)currentSlot]?.TryReload(OnAmmoChange);
    }

    private void ChangeToWeapon1() => ChangeWeapon(0);
    private void ChangeToWeapon2() => ChangeWeapon(1);

    private void ChangeWeapon(int index)
    {
        if (index == (int)currentSlot || index < 0 || index >= gunsObj.Length)
            return;

        if (gunsScripts[(int)currentSlot] != null)
            gunsObj[(int)currentSlot].SetActive(false);

        if (gunsScripts[index] != null)
            gunsObj[index].SetActive(true);

        currentSlot = (Slots)index;

        OnAmmoChange?.Invoke();
    }

    private void DropGun()
    {
        gunsScripts[(int)currentSlot]?.Drop();
        gunsScripts[(int)currentSlot] = null;
        gunsObj[(int)currentSlot] = null;

        OnAmmoChange?.Invoke();
    }

    private void GrabWeapon()
    {
        if (gunsScripts[(int)currentSlot]) return;

        Gun gun = FindBestInteractable();
        if (gun != null)
        {
            gun.Grab(this, weaponGrabPos);
            gunsScripts[(int)currentSlot] = gun;
            gunsObj[(int)currentSlot] = gun.gameObject;
        }

        OnAmmoChange?.Invoke();
    }

    private Gun FindBestInteractable()
    {
        Vector3 origin = cam.transform.position;
        Vector3 direction = cam.transform.forward;

        if (Physics.SphereCast(origin, grabSphereRadius, direction, out RaycastHit hit, grabDistance, interactableLayer))
        {
            if (hit.transform.TryGetComponent(out Gun gun))
                return gun;
        }

        return null;
    }

    private void MeleeAttack()
    {
        Vector3 center = transform.position + cam.transform.forward * 2f;
        Collider[] hits = Physics.OverlapBox(center, new Vector3(rangeX, rangeY, rangeX), Quaternion.identity);

        Characters closestTarget = null;
        float closestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Characters>(out var target) && target != this)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestTarget = target;
                }
            }
        }

        if (closestTarget != null)
        {
            closestTarget.ReciveDamage(30f);
            Debug.Log("<color=yellow>[Player]</color> Melee hit to " + closestTarget.name);
        }

        Debug.DrawLine(transform.position, center, Color.green, 1f);
    }

    public override void ReciveDamage(float damage)
    {
        if (isInmortal) return;

        base.ReciveDamage(damage);

        OnHealthChange?.Invoke();
    }

    public void AddKill()
    {
        _kills++;
        OnKill?.Invoke();
    }

    public Vector3 getPos() => transform.position;

    public bool IsLookingAt(Vector3 worldPos)
    {
        Vector3 dir = (worldPos - cam.transform.position).normalized;
        float angle = Vector3.Angle(cam.transform.forward, dir);
        return angle < viewAngleThreshold;
    }

    public float GetCurrentWeaponAmmo()
    {
        return gunsScripts[(int)currentSlot]?.magazine.CurrentAmmo ?? 0f;
    }

    public float GetCurrentWeaponMaxAmmo()
    {
        return gunsScripts[(int)currentSlot]?.magazine.MaxAmmo ?? 0f;
    }

    public void ChangeInmortalState()
    {
        isInmortal = !isInmortal;
    }
}
