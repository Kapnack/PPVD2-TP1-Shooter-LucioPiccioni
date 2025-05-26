using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAsset;
    InputActionMap actionMap;

    private InputAction actionMove;
    private InputAction actionJump;
    private InputAction actionLook;
    private InputAction actionFire;
    private InputAction actionReload;
    private InputAction actionChangeWeapon1;
    private InputAction actionChangeWeapon2;
    private InputAction actionMeleeAttack;
    private InputAction actionPause;
    private InputAction actionInteract;
    private InputAction actionDropWeapon;

    public event Action<Vector2> MoveEvent;

    public event Action<Vector2> lookEvent;
    public event Action<Vector2> lookEventHolding;
    public event Action<Vector2> lookEventCanceled;

    public event Action JumpEvent;
    public event Action JumpHoldEvent;
    public event Action JumpReleaseEvent;

    public event Action FireEvent;
    public event Action HoldigFireEvent;
    public event Action StopHoldigFireEvent;

    public event Action ReloadEvent;

    public event Action ChangeWeapon1Event;
    public event Action ChangeWeapon2Event;

    public event Action MeleeAttackEvent;

    public event Action PauseEvent;

    public event Action InteractEvent;

    public event Action DropWeaponEvent;

    protected void Awake()
    {
        ServiceProvider.SetService(this);

        actionMap = inputAsset.FindActionMap("Player");

        actionMove = actionMap.FindAction("Move");
        actionJump = actionMap.FindAction("Jump");

        actionLook = actionMap.FindAction("Look");

        actionFire = actionMap.FindAction("Fire");

        actionReload = actionMap.FindAction("Reload");

        actionChangeWeapon1 = actionMap.FindAction("SelectWeapon1");
        actionChangeWeapon2 = actionMap.FindAction("SelectWeapon2");

        actionMeleeAttack = actionMap.FindAction("MeleeAttack");

        actionPause = actionMap.FindAction("Pause");

        actionInteract = actionMap.FindAction("Interact");

        actionDropWeapon = actionMap.FindAction("DropWeapon");
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
        actionLook.performed += HandleLookHolding;
        actionLook.canceled += HandleLookCanceled;

        actionFire.started += HandleFire;
        actionFire.performed += HandleHoldingFire;
        actionFire.canceled += HandleStopHoldingFire;

        actionReload.started += HandleReload;

        actionChangeWeapon1.started += HandleChangeWeapon1;
        actionChangeWeapon2.started += HandleChangeWeapon2;

        actionMeleeAttack.started += HandleMeleeAttack;

        actionPause.started += HandlePause;

        actionInteract.started += HandleInteract;

        actionDropWeapon.started += HandleDropWeapon;
    }

    private void HandleDropWeapon(InputAction.CallbackContext context)
    {
        DropWeaponEvent?.Invoke();
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        InteractEvent?.Invoke();
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

        actionReload.started -= HandleReload;

        actionChangeWeapon1.started -= HandleChangeWeapon1;
        actionChangeWeapon2.started -= HandleChangeWeapon2;

        actionMeleeAttack.started -= HandleMeleeAttack;

        actionPause.started -= HandlePause;

        actionInteract.started -= HandleInteract;

        actionDropWeapon.started -= HandleDropWeapon;
    }


    //----------------------------- MOVEMENT -------------------------------------------------
    private void HandleMoveInput(InputAction.CallbackContext ctx)
    {
        MoveEvent?.Invoke(ctx.ReadValue<Vector2>());
    }


    //----------------------------- JUMPING --------------------------------------------------
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


    //----------------------------- LOOK -----------------------------------------------------
    private void HandleLook(InputAction.CallbackContext ctx)
    {
        lookEvent?.Invoke(ctx.ReadValue<Vector2>());
    }
    private void HandleLookHolding(InputAction.CallbackContext ctx)
    {
        lookEventHolding?.Invoke(ctx.ReadValue<Vector2>());
    }
    private void HandleLookCanceled(InputAction.CallbackContext ctx)
    {
        lookEventCanceled?.Invoke(ctx.ReadValue<Vector2>());
    }
    public Vector2 GetLookVector2() => actionLook.ReadValue<Vector2>();


    //----------------------------- FIRE -----------------------------------------------------
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


    //----------------------------- RELOAD ---------------------------------------------------
    private void HandleReload(InputAction.CallbackContext ctx)
    {
        ReloadEvent?.Invoke();
    }


    //----------------------------- ChangeWeapong --------------------------------------------
    private void HandleChangeWeapon1(InputAction.CallbackContext ctx)
    {
        ChangeWeapon1Event?.Invoke();
    }
    private void HandleChangeWeapon2(InputAction.CallbackContext ctx)
    {
        ChangeWeapon2Event?.Invoke();
    }

    //----------------------------- MeleeAttack --------------------------------------------
    private void HandleMeleeAttack(InputAction.CallbackContext ctx)
    {
        MeleeAttackEvent?.Invoke();
    }

    private void HandlePause(InputAction.CallbackContext ctx)
    {
        PauseEvent?.Invoke();
    }
}
