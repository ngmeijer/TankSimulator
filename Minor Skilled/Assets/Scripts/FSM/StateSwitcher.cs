using System;
using UnityEngine;

public class StateSwitcher : MonoBehaviour
{
    [Header("Tank states")]
    public E_TankState DefaultTankState = E_TankState.Combat;
    [SerializeField] private TankState _inspectionState;
    public TankState CombatState;
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
            
            CurrentTankState.Exit();
            LastTankState = CurrentTankState;
        }

        TankState newState = newStateEnum switch
        {
            E_TankState.Inspection => _inspectionState,
            E_TankState.Combat => CombatState,
            E_TankState.Death => _deathState,
            E_TankState.HostileInspection => _hostileInspectionState,
        };

        newState.Enter();
        CurrentTankState = newState;
        TankStateEnum = CurrentTankState.ThisState;
    }
}