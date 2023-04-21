using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    public static Vector3 HandlesOffset = new Vector3(0.5f, 0.5f, 0);
    public const int PLAYER_ID = 0;
    
    [HideInInspector] public float BarrelRotationValue;
    [HideInInspector] public Transform HostileTargetTransform;
    [HideInInspector] public Vector3 CurrentBarrelCrosshairPos;
    [HideInInspector] public Vector3 TargetBarrelCrosshairPos;

    public TankComponentManager Player;
    public Transform GetShellParent() => _spawnedShellsParent;
    [SerializeField] private Transform _spawnedShellsParent;
    [SerializeField] private EventManager _eventManager;
    private readonly Dictionary<int, TankComponentManager> _entities = new();
    public Dictionary<int, TankComponentManager> GetEntities() => _entities;
    public readonly Dictionary<int, Vector3> EntityWorldPositions = new();
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

        foreach (var entity in _entities)
        {
            if (!entity.Value.TryGetComponent(out TankComponentManager tankManager)) continue;
            if (tankManager.ID == PLAYER_ID) continue;
            _entities.Add(tankManager.ID, tankManager);
            tankManager.EventManager.OnTankDestruction.AddListener(RemoveEntityFromWorld);
        }
    }

    private void Start()
    {
        foreach (var entity in _entities)
        {
            EntityWorldPositions.Add(entity.Key, Vector2.zero);
        }
        
        _hudCombatState = HUDStateSwitcher.Instance.HUDCombatState as HUDCombatState;
    }

    private void Update()
    {
        foreach (var entity in _entities)
        {
            EntityWorldPositions[entity.Key] = entity.Value.EntityOrigin.position;
        }
    }

    private void RemoveEntityFromWorld(int entityID)
    {
        EntityWorldPositions.Remove(entityID);
        _entities.Remove(entityID);
    }
}