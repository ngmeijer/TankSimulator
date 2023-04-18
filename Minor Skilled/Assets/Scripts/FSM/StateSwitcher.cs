using System;
using UnityEngine;

public class StateSwitcher : MonoBehaviour
{
    [Header("Tank states")]
    public E_TankState DefaultTankState = E_TankState.Combat;
    [SerializeField] private TankState _inspectionState;
    [SerializeField] private TankState _combatState;
    [SerializeField] private TankState _deathState;
    [SerializeField] private TankState _hostileInspectionState;

    public TankState CurrentTankState { get; private set; }
    public E_TankState TankStateEnum;
    public TankState LastTankState { get; private set; }

    protected virtual void Update()
    {
        if (CurrentTankState == null) return;
        CurrentTankState.UpdateState();
    }

    protected virtual void FixedUpdate()
    {
        if (CurrentTankState == null) return;
        CurrentTankState.FixedUpdateState();
    }

    protected virtual void LateUpdate()
    {
        if (CurrentTankState == null) return;
        CurrentTankState.LateUpdateState();
    }
    
    public void SwitchToTankState(E_TankState newStateEnum)
    {
        if (CurrentTankState != null)
        {
            if (CurrentTankState.ThisState == newStateEnum)
                return;
            
            CurrentTankState.ExitState();
            LastTankState = CurrentTankState;
        }

        TankState newState = newStateEnum switch
        {
            E_TankState.Inspection => _inspectionState,
            E_TankState.Combat => _combatState,
            E_TankState.Death => _deathState,
            E_TankState.HostileInspection => _hostileInspectionState,
        };

        newState.EnterState();
        CurrentTankState = newState;
        TankStateEnum = CurrentTankState.ThisState;
    }
}