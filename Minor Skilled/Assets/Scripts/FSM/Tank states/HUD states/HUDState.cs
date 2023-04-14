using System;
using UnityEngine;

public abstract class HUDState : FSMState
{
    public E_TankState ThisState;
    
    [Header("If null, uses the GameObject on which the script is attached to.")]
    public GameObject HUDContainer;

    protected virtual void Awake()
    {
        if (HUDContainer == null)
            HUDContainer = this.gameObject;
    }

    public override void EnterState()
    {
        HUDContainer.SetActive(true);
    }

    public override void ExitState()
    {
        HUDContainer.SetActive(false);
    }
}