using System;
using TMPro;
using UnityEngine;

public class PlayerHudManager : Singleton<PlayerHudManager>
{
    [SerializeField] private GameObject player;
    private Player playerScript;

    private Canvas canvas;

    [SerializeField] private GameObject killsCanvas;
    [SerializeField] private GameObject healthCanvas;
    [SerializeField] private GameObject shieldCanvas;
    [SerializeField] private GameObject ammoCanvas;

    private TextMeshProUGUI killsText;
    private TextMeshProUGUI healthText;
    private TextMeshProUGUI shieldText;
    private TextMeshProUGUI ammoText;

    private bool shouldUpdateAmmo = false;

    protected override void Awake()
    {
        base.Awake();

        playerScript = player.GetComponent<Player>();

        canvas = GetComponent<Canvas>();

        killsText = killsCanvas.GetComponent<TextMeshProUGUI>();
        healthText = healthCanvas.GetComponent<TextMeshProUGUI>();
        shieldText = shieldCanvas.GetComponent<TextMeshProUGUI>();
        ammoText = ammoCanvas.GetComponent<TextMeshProUGUI>();
    }

    public void OnEnable()
    {
        InputReader.ChangeWeapon1Event += UpdateAmmoHud;
        InputReader.ChangeWeapon2Event += UpdateAmmoHud;
        InputReader.FireEvent += UpdateAmmoHud;
        InputReader.HoldigFireEvent += OnHoldingFire;
        InputReader.StopHoldigFireEvent += OnCanceledHoldingFire;

        InputReader.ReloadEvent += UpdateAmmoHud;
    }

    public void OnDisable()
    {
        InputReader.ChangeWeapon1Event -= UpdateAmmoHud;
        InputReader.ChangeWeapon2Event -= UpdateAmmoHud;
        InputReader.FireEvent -= UpdateAmmoHud;
        InputReader.HoldigFireEvent -= OnHoldingFire;
        InputReader.StopHoldigFireEvent -= OnCanceledHoldingFire;

        InputReader.ReloadEvent -= UpdateAmmoHud;
    }

    private void Update()
    {
        if (shouldUpdateAmmo)
            UpdateAmmoHud();
    }

    public void UpdateKillsHud()
    {
        killsText.text = "Kills: " + playerScript.Kills;
    }

    public void UpdateHealthsHud(int health)
    {
        healthText.text = "Health: " + health;
    }

    public void UpdateShieldHud(int health)
    {
        shieldText.text = "Shield: " + health;
    }


    public void OnHoldingFire() => shouldUpdateAmmo = true;
    public void OnCanceledHoldingFire() => shouldUpdateAmmo = false;

    public void UpdateAmmoHud()
    {
        ammoText.text = playerScript.GetCurrentWeaponAmmo() + " / " + playerScript.GetCurrentWeaponMaxAmmo();
    }

    private void DisableCanvas()
    {
        gameObject.SetActive(false);
    }

    private void EnableCanvas()
    {
        gameObject.SetActive(true);
    }
}
