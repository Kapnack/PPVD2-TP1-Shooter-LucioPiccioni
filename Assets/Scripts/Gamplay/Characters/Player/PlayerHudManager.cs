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

    private InputReader inputReader;

    private bool shouldUpdateAmmo = false;

    protected void Awake()
    {
        if (ServiceProvider.TryGetService<InputReader>(out var inputReader))
            this.inputReader = inputReader;

        playerScript = player.GetComponent<Player>();

        canvas = GetComponent<Canvas>();

        killsText = killsCanvas.GetComponent<TextMeshProUGUI>();
        healthText = healthCanvas.GetComponent<TextMeshProUGUI>();
        shieldText = shieldCanvas.GetComponent<TextMeshProUGUI>();
        ammoText = ammoCanvas.GetComponent<TextMeshProUGUI>();
    }

    public void OnEnable()
    {
        inputReader.ChangeWeapon1Event += UpdateAmmoHud;
        inputReader.ChangeWeapon2Event += UpdateAmmoHud;
        inputReader.FireEvent += UpdateAmmoHud;
        inputReader.HoldigFireEvent += OnHoldingFire;
        inputReader.StopHoldigFireEvent += OnCanceledHoldingFire;

        inputReader.ReloadEvent += UpdateAmmoHud;

        inputReader.DropWeaponEvent += UpdateAmmoHud;
        inputReader.InteractEvent += UpdateAmmoHud;
    }

    public void OnDisable()
    {
        inputReader.ChangeWeapon1Event -= UpdateAmmoHud;
        inputReader.ChangeWeapon2Event -= UpdateAmmoHud;
        inputReader.FireEvent -= UpdateAmmoHud;
        inputReader.HoldigFireEvent -= OnHoldingFire;
        inputReader.StopHoldigFireEvent -= OnCanceledHoldingFire;

        inputReader.ReloadEvent -= UpdateAmmoHud;

        inputReader.DropWeaponEvent -= UpdateAmmoHud;
        inputReader.InteractEvent -= UpdateAmmoHud;
    }

    private void Update()
    {
        if (shouldUpdateAmmo)
            UpdateAmmoHud();

        UpdateHealthHud();
        UpdateShieldHud();
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
