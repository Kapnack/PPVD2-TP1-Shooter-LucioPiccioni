using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : Singleton<InputReader> 
{
    [SerializeField] private InputActionAsset inputAsset;
    InputActionMap actionMap;

    InputAction actionMove;
    InputAction actionJump;
    InputAction actionLook;
    InputAction actionFire;
    InputAction actionReload;


    public static event Action<Vector2> MoveEvent;
    public static event Action<Vector2> lookEvent;

    public static event Action JumpEvent;
    public static event Action JumpHoldEvent;
    public static event Action JumpReleaseEvent;

    public static event Action FireEvent;
    public static event Action HoldigFireEvent;
    public static event Action StopHoldigFireEvent;

    public static event Action ReloadEvent;

    private void Awake()
    {
        actionMap = inputAsset.FindActionMap("Player");

        actionMove = actionMap.FindAction("Move");
        actionJump = actionMap.FindAction("Jump");

        actionLook = actionMap.FindAction("Look");

        actionFire = actionMap.FindAction("Fire");

        actionReload = actionMap.FindAction("Reload");
    }

    private void OnEnable()
    {
        actionMap?.Enable();
        actionMove.started += HandleMoveInput;
        actionMove.performed += HandleMoveInput;
        actionMove.canceled += HandleMoveInput;

        actionJump.started += HandleJumpInput;
        actionJump.performed += IsHoldingJump;
        actionJump.canceled += IsNotHoldingJump;

        actionLook.started += HandleLook;
        actionLook.performed += HandleLook;
        actionLook.canceled += HandleLook;

        actionFire.started += HandleFire;
        actionFire.performed += HandleHoldingFire;
        actionFire.canceled += HandleStopHoldingFire;

        actionReload.started += HandleReload;
    }

    private void OnDisable()
    {
        actionMap?.Disable();
        actionMove.started -= HandleMoveInput;
        actionMove.performed -= HandleMoveInput;
        actionMove.canceled -= HandleMoveInput;

        actionJump.started -= HandleJumpInput;
        actionJump.performed -= IsHoldingJump;
        actionJump.canceled -= IsNotHoldingJump;

        actionLook.started -= HandleLook;
        actionLook.performed -= HandleLook;
        actionLook.canceled -= HandleLook;

        actionFire.started -= HandleFire;
        actionFire.performed -= HandleHoldingFire;
        actionFire.canceled -= HandleStopHoldingFire;

        actionReload.started -= HandleReload;
        actionReload.performed -= HandleHoldingFire;
    }

    private void HandleMoveInput(InputAction.CallbackContext ctx)
    {
        MoveEvent?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void HandleJumpInput(InputAction.CallbackContext ctx)
    {
        JumpEvent?.Invoke();
    }

    private void IsHoldingJump(InputAction.CallbackContext ctx)
    {
        JumpHoldEvent?.Invoke();
    }

    private void IsNotHoldingJump(InputAction.CallbackContext ctx)
    {
        JumpReleaseEvent?.Invoke();
    }
    private void HandleLook(InputAction.CallbackContext ctx)
    {
        lookEvent?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void HandleFire(InputAction.CallbackContext ctx)
    {
        FireEvent?.Invoke();
    }

    private void HandleHoldingFire(InputAction.CallbackContext ctx)
    {
        HoldigFireEvent?.Invoke();
    }
    private void HandleStopHoldingFire(InputAction.CallbackContext ctx)
    {
        StopHoldigFireEvent?.Invoke();
    }

    private void HandleReload(InputAction.CallbackContext ctx)
    {
        ReloadEvent?.Invoke();
    }

}
