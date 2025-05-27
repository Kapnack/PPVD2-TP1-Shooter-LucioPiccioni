using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheatManager : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string actionMapName = "Cheats";

    private InputAction fastTimeAction;
    private InputAction immortalAction;
    private InputAction changeLevel;

    [SerializeField] private GameObject playerObj;

    private IPlayer iPlayer;
    private IGameplayManager gameplayManager;

    private void Awake()
    {
        if (playerObj.TryGetComponent<IPlayer>(out var iPlayer))
            this.iPlayer = iPlayer;

        if (inputActions == null)
        {
            Debug.LogError("[CheatManager] InputActionAsset not assigned.");
            enabled = false;
            return;
        }

        var map = inputActions.FindActionMap(actionMapName, true);
        if (map == null)
        {
            Debug.LogError($"[CheatManager] Could not find ActionMap: {actionMapName}");
            enabled = false;
            return;
        }

        changeLevel = map.FindAction("F9", true);
        immortalAction = map.FindAction("F10", true);
        fastTimeAction = map.FindAction("F11", true);
    }

    private void OnEnable()
    {
        changeLevel?.Enable();
        immortalAction?.Enable();
        fastTimeAction?.Enable();

        changeLevel.started += ChangeLevel;
        immortalAction.started += ToggleImmortal;
        fastTimeAction.started += ToggleFastTime;

        StartCoroutine(WaitForGameplayManager());
    }

    private void OnDisable()
    {
        changeLevel?.Disable();
        immortalAction?.Disable();
        fastTimeAction?.Disable();

        changeLevel.started -= ChangeLevel;
        immortalAction.started -= ToggleImmortal;
        fastTimeAction.started -= ToggleFastTime;
    }

    private IEnumerator WaitForGameplayManager()
    {
        while (gameplayManager == null)
        {
            if (ServiceProvider.TryGetService<IGameplayManager>(out var gm))
            {
                gameplayManager = gm;
                Debug.Log("<color=cyan>[CheatManager]</color> GameplayManager found.");
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ToggleFastTime(InputAction.CallbackContext context)
    {
        Time.timeScale = Time.timeScale == 1f ? 2f : 1f;
        Debug.Log($"[CheatManager] Time scale set to: {Time.timeScale}");
    }

    private void ToggleImmortal(InputAction.CallbackContext context)
    {
        if (iPlayer == null) return;
        Debug.Log("<color=yellow>[CheatManager]</color> Immortality toggled.");
        iPlayer.ChangeInmortalState();
    }

    private void ChangeLevel(InputAction.CallbackContext context)
    {
        if (this.gameplayManager != null)
        {
            this.gameplayManager.LoadNextLevel();
            Debug.Log("<color=yellow>[CheatManager]</color> Forced level change.");
        }
        else
        {
            Debug.LogWarning("[CheatManager] GameplayManager not available yet.");
        }

        gameplayManager = null;

        StartCoroutine(WaitForGameplayManager());
    }
}

