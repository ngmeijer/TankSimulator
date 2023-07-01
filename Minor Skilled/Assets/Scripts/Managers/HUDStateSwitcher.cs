using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using DG.Tweening;
using FSM.HUDStates;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HUDStateSwitcher : SingletonMonobehaviour<HUDStateSwitcher>
{
    public HUDState HUDCombatState;
    public HUDState HUDDeathState;
    public HUDState HUDInspectState;
    public HUDState HUDHostileInspectState;
    public HUDState HUDPauseState;
    public HUDState CurrentHUDState { get; private set; }
    public E_TankState LastState;

    private bool _isPaused;
    private PlayerInputActions _inputActions;
    
    private void Start()
    {
        HUDDeathState.Exit();
        HUDInspectState.Exit();
        HUDHostileInspectState.Exit();
    }

    public void SwitchToHUDState(E_TankState newStateEnum)
    {
        if (_isPaused)
            return;
        
        if (CurrentHUDState != null)
        {
            if (CurrentHUDState.ThisState == newStateEnum)
                return;

            LastState = CurrentHUDState.ThisState;
            CurrentHUDState.Exit();
        }
        
        HUDState newState = newStateEnum switch
        {
            E_TankState.Inspection => HUDInspectState,
            E_TankState.Combat => HUDCombatState,
            E_TankState.Death => HUDDeathState,
            E_TankState.HostileInspection => HUDHostileInspectState,
            E_TankState.Pause => HUDPauseState
        };
        
        newState.Enter();
        CurrentHUDState = newState;
    }
}