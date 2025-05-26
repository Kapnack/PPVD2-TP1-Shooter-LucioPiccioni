using System;
using System.Collections.Generic;
using UnityEngine;

public enum Slots
{
    Slot0,
    Slot1
}

public class Player : Characters, IPlayer
{
    [Header("Dependencies")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform weaponGrabPos;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Weapon Settings")]
    [SerializeField] private GameObject[] gunsObj = new GameObject[2];
    [SerializeField] private float grabDistance = 25.0f;

    [Header("View Settings")]
    [SerializeField] private float viewAngleThreshold = 90f;

    private Gun[] gunsScripts = new Gun[2];
    private InputReader inputReader;

    private Slots currentSlot;
    private bool isHoldingFire = false;

    private int kills = 0;
    public int Kills => kills;

    public Action<float> TakeDamage => ReciveDamage;

    private void Awake()
    {
        if (ServiceProvider.TryGetService<InputReader>(out var input))
            inputReader = input;

        for (int i = 0; i < gunsObj.Length; i++)
        {
            if (gunsObj[i].TryGetComponent<Gun>(out var script))
            {
                gunsScripts[i] = script;
                gunsScripts[i].Grab(this, weaponGrabPos);
            }

            gunsObj[i].SetActive(i == 0); // Activa sólo el primer arma
        }

        ServiceProvider.SetService<IPlayer>(this, true);
    }

    private void OnEnable()
    {
        inputReader.FireEvent += Fire;
        inputReader.HoldigFireEvent += HoldingFire;
        inputReader.StopHoldigFireEvent += StopFire;

        inputReader.ReloadEvent += Reload;
        inputReader.ChangeWeapon1Event += ChangeToWeapon1;
        inputReader.ChangeWeapon2Event += ChangeToWeapon2;

        inputReader.InteractEvent += GrabWeapon;
        inputReader.DropWeaponEvent += DropGun;
    }

    private void OnDisable()
    {
        inputReader.FireEvent -= Fire;
        inputReader.HoldigFireEvent -= HoldingFire;
        inputReader.StopHoldigFireEvent -= StopFire;

        inputReader.ReloadEvent -= Reload;
        inputReader.ChangeWeapon1Event -= ChangeToWeapon1;
        inputReader.ChangeWeapon2Event -= ChangeToWeapon2;

        inputReader.InteractEvent -= GrabWeapon;
        inputReader.DropWeaponEvent -= DropGun;
    }

    private void Update()
    {
        if (isHoldingFire)
            Fire();
    }

    public override bool IsDead()
    {
        return base.IsDead();
    }

    protected override void OnDead()
    {
        // TODO: Implementar comportamiento de muerte si es necesario
    }

    // ───────────────────────────────────────────── Fire & Weapons ─────────────────────────────────────────────

    private void Fire()
    {
        if (gunsScripts[(int)currentSlot] != null)
            gunsScripts[(int)currentSlot].TryFire();
        else
            MeleeAttack();
    }

    private void HoldingFire() => isHoldingFire = true;
    private void StopFire() => isHoldingFire = false;

    private void Reload()
    {
        gunsScripts[(int)currentSlot]?.TryReload();
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
    }

    private void DropGun()
    {
        gunsScripts[(int)currentSlot]?.Drop();
        gunsScripts[(int)currentSlot] = null;
        gunsObj[(int)currentSlot] = null;
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
    }

    private Gun FindBestInteractable()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, grabDistance, interactableLayer))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * grabDistance, Color.red, 1f);
            if (hit.transform.TryGetComponent(out Gun gun))
                return gun;
        }
        return null;
    }

    private void MeleeAttack()
    {
        float rangeX = 2f;
        float rangeY = 2f;

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
            closestTarget.ReciveDamage(30f); // Daño cuerpo a cuerpo
            Debug.Log("<color=yellow>[Player]</color> Melee hit to " + closestTarget.name);
        }

        Debug.DrawLine(transform.position, center, Color.green, 1f);
    }

    public override void ReciveDamage(float damage)
    {
        base.ReciveDamage(damage);
    }

    public void AddKill() => kills++;

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
}
