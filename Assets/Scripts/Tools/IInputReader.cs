using System;
using UnityEngine;

public interface IInputReader
{
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

    public Vector2 GetLookVector2();
}