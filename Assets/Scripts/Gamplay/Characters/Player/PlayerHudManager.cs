using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHudManager : MonoBehaviour
{
    private IPlayer iPlayer;

    private Canvas canvas;

    [SerializeField] private GameObject killsCanvas;
    [SerializeField] private GameObject healthCanvas;
    [SerializeField] private GameObject shieldCanvas;
    [SerializeField] private GameObject ammoCanvas;

    private TextMeshProUGUI killsText;
    private TextMeshProUGUI healthText;
    private TextMeshProUGUI shieldText;
    private TextMeshProUGUI ammoText;

    protected void Awake()
    {
        canvas = GetComponent<Canvas>();

        killsText = killsCanvas.GetComponent<TextMeshProUGUI>();
        healthText = healthCanvas.GetComponent<TextMeshProUGUI>();
        shieldText = shieldCanvas.GetComponent<TextMeshProUGUI>();
        ammoText = ammoCanvas.GetComponent<TextMeshProUGUI>();

        StartCoroutine(FindIPlayer());
    }

    private void OnDisable()
    {
        if (iPlayer == null) return;

        iPlayer.OnKill -= UpdateKillsHud;
        iPlayer.OnHealthChange -= UpdateHealthHud;
        iPlayer.OnHealthChange -= UpdateShieldHud;
        iPlayer.OnAmmoChange -= UpdateAmmoHud;
    }

    private IEnumerator FindIPlayer()
    {
        IPlayer iPlayer;

        while (!ServiceProvider.TryGetService<IPlayer>(out iPlayer))
            yield return null;

        this.iPlayer = iPlayer;

        SubscribeToEvents();
        UpdateAll();
    }

    private void SubscribeToEvents()
    {
        iPlayer.OnKill += UpdateKillsHud;
        iPlayer.OnHealthChange += UpdateHealthHud;
        iPlayer.OnHealthChange += UpdateShieldHud;
        iPlayer.OnAmmoChange += UpdateAmmoHud;
    }

    private void UpdateAll()
    {
        UpdateKillsHud();
        UpdateHealthHud();
        UpdateShieldHud();
        UpdateAmmoHud();
    }

    public void UpdateKillsHud()
    {
        if (iPlayer == null) return;

        killsText.text = "Kills: " + iPlayer.Kills;
    }

    public void UpdateHealthHud()
    {
        if (iPlayer == null) return;

        healthText.text = "HP: " + iPlayer.ActualHealth + " / " + iPlayer.MaxHealth;
    }

    public void UpdateShieldHud()
    {
        if (iPlayer == null) return;

        shieldText.text = "ARM: " + iPlayer.ActualShield + " / " + iPlayer.MaxShield;
    }

    public void UpdateAmmoHud()
    {
        if (iPlayer == null) return;

        ammoText.text = iPlayer.GetCurrentWeaponAmmo() + " / " + iPlayer.GetCurrentWeaponMaxAmmo();
    }
}
