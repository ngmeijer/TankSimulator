using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FSM.HUDStates;
using Tank_components;
using TankComponents;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    public static Vector3 HandlesOffset = new Vector3(0.5f, 0.5f, 0);
    public const int PLAYER_ID = 0;
    
    [HideInInspector] public float BarrelRotationValue;
    [HideInInspector] public TankComponentManager HostileManager;
    [HideInInspector] public Vector3 CurrentBarrelCrosshairPos;
    [HideInInspector] public Vector3 TargetBarrelCrosshairPos;

    public TankComponentManager Player;
    public Transform GetShellParent() => _spawnedShellsParent;
    [SerializeField] private Transform _spawnedShellsParent;
    [SerializeField] private EventManager _eventManager;
    public List<TankComponentManager> Entities;
    private HUDCombatState _hudCombatState;

    public Transform InspectionCameraTrans;

    private void OnValidate()
    {
        Debug.Assert(Player != null,
            $"Player reference in GameManager ({gameObject.name}) is null. Drag into the inspector");
        Debug.Assert(_spawnedShellsParent != null,
            $"SpawnedShellsParent reference in GameManager ({gameObject.name}) is null. Drag into the inspector");
        Debug.Assert(_eventManager != null,
            $"EventManager reference in GameManager ({gameObject.name}) is null. Drag into the inspector");
    }

    protected override void Awake()
    {
        base.Awake();

        Entities = FindObjectsOfType<TankComponentManager>().ToList();
    }

    private void Start()
    {
        _hudCombatState = HUDStateSwitcher.Instance.HUDCombatState as HUDCombatState;
    }

    public void ResumeGame()
    {
        Player.ThisStateSwitcher.SwitchToTankState(Player.ThisStateSwitcher.LastTankState.ThisState);
        HUDStateSwitcher.Instance.SwitchToHUDState(HUDStateSwitcher.Instance.LastState);
    }
}