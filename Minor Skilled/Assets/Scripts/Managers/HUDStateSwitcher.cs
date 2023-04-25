using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDStateSwitcher : SingletonMonobehaviour<HUDStateSwitcher>
{
    [Header("Health properties")] 
    [SerializeField] private Slider _generalHealth;
    [SerializeField] private Slider _generalArmor;

    public HUDState HUDCombatState;
    public HUDState HUDDeathState;
    public HUDState HUDInspectState;
    public HUDState HUDHostileInspectState;
    
    public HUDState CurrentHUDState { get; private set; }

    private void Start()
    {
        HUDDeathState.Exit();
        HUDInspectState.Exit();
        HUDHostileInspectState.Exit();
    }

    public void SwitchToHUDState(E_TankState newStateEnum)
    {
        if (CurrentHUDState != null)
        {
            if (CurrentHUDState.ThisState == newStateEnum)
                return;
            
            CurrentHUDState.Exit();
        }
        
        HUDState newState = newStateEnum switch
        {
            E_TankState.Inspection => HUDInspectState,
            E_TankState.Combat => HUDCombatState,
            E_TankState.Death => HUDDeathState,
            E_TankState.HostileInspection => HUDHostileInspectState,
        };
        
        newState.Enter();
        CurrentHUDState = newState;
    }
}