using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public enum TankParts
{
    Turret,
    LeftTrack,
    RightTrack,
    Hull
}

public class DamageRegistrationComponent : TankComponent
{
    public TankData CurrentData = new TankData();

    public TankPart CurrentSelectedPart;
    public bool RecentlyShot;
    public float CalmDownTimer;

    private void Start()
    {
        //HUDManager.Instance.UpdateMaxHealthForEntity(_componentManager.ID, CurrentData.GetMaxTotalHealth());
        //HUDManager.Instance.UpdateCurrentHealthForEntity(_componentManager.ID, CurrentData.GetCurrentTotalHealth(), CurrentData.GetMaxTotalHealth());

        Debug.Assert(CurrentData.TankParts.Count != 0, $"Tank parts have not been added in DamageRegistrationComponent, GameObject '{gameObject.name}' ");
        foreach (var part in CurrentData.TankParts)
        {
            part.OnDamageRegistered.AddListener(UpdateGeneralStats);
            part.OnZeroHealthReached.AddListener(OnTankDestruction);
        }
    }

    private void OnTankDestruction()
    {
        _componentManager.StateSwitcher.SwitchToTankState(E_TankState.Death);
    }

    private void UpdateGeneralStats()
    {
        RecentlyShot = true;
    }

    public void ShowUI(bool enabled)
    {
        foreach (var part in CurrentData.TankParts)
        {
            part.EnableCanvas(enabled);
        }
    }

    public void SelectPart(TankPart part)
    {
        CurrentSelectedPart = part;
    }
    
    public void RepairPart(InputAction.CallbackContext cb)
    {
        CurrentSelectedPart.RepairPart();
    }

    public void RepairAllParts()
    {
        foreach (var part in CurrentData.TankParts)
        {
            part.RepairPart();
        }
    }
}