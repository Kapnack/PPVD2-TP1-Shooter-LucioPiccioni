using TMPro;
using UnityEngine;

public class PlayerHudManager : MonoBehaviour
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

    protected void Awake()
    {
        playerScript = player.GetComponent<Player>();

        canvas = GetComponent<Canvas>();

        killsText = killsCanvas.GetComponent<TextMeshProUGUI>();
        healthText = healthCanvas.GetComponent<TextMeshProUGUI>();
        shieldText = shieldCanvas.GetComponent<TextMeshProUGUI>();
        ammoText = ammoCanvas.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        UpdateAmmoHud();

        UpdateHealthHud();
        UpdateShieldHud();

        UpdateKillsHud();
    }

    public void UpdateKillsHud()
    {
        killsText.text = "Kills: " + playerScript.Kills;
    }

    public void UpdateHealthHud()
    {
        healthText.text = "HP: " + playerScript.ActualHealth + " / " + playerScript.maxHealth;
    }

    public void UpdateShieldHud()
    {
        shieldText.text = "ARM: " + playerScript.ActualShield + " / " + playerScript.maxShield;
    }

    public void UpdateAmmoHud()
    {
        ammoText.text = playerScript.GetCurrentWeaponAmmo() + " / " + playerScript.GetCurrentWeaponMaxAmmo();
    }
}
