using System;
using JetBrains.Annotations;
using UnityEngine;

public abstract class HUDState : FSMState
{
    public E_TankState ThisState;
    
    [Header("If null, uses the GameObject on which the script is attached to.")]
    public GameObject HUDContainer;
    
    protected PlayerInputActions _inputActions;

    protected virtual void Awake()
    {
        if (HUDContainer == null)
            HUDContainer = this.gameObject;

        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    public override void Enter()
    {
        HUDContainer.SetActive(true);
    }

    public override void Exit()
    {
        HUDContainer.SetActive(false);
    }
}