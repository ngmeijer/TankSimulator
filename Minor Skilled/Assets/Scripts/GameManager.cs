using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    public const int PLAYER_ID = 0;
    [SerializeField] private Transform _player;
    public float RotationValue;
    public bool ValidTargetInSight;
    public Transform HostileTargetTransform;

    public Transform GetPlayer() => _player;

    [SerializeField] private Transform _spawnedShellsParent;
    public Transform GetShellParent() => _spawnedShellsParent;

    [SerializeField] private List<TankComponentManager> _entitiesList;
    private Dictionary<int, TankComponentManager> _entities = new();
    public Dictionary<int, TankComponentManager> GetEntities() => _entities;
    public Dictionary<int, Vector3> EntityWorldPositions = new Dictionary<int, Vector3>();
    [SerializeField] private EventManager _eventManager;
    public Vector3 CurrentBarrelCrosshairPos;
    public Vector3 TargetBarrelCrosshairPos;
    
    public static Vector3 HandlesOffset = new Vector3(0.5f, 0.5f, 0);

    private void OnValidate()
    {
        Debug.Assert(_player != null,
            $"Player reference in GameManager ({gameObject.name}) is null. Drag into the inspector");
        Debug.Assert(_spawnedShellsParent != null,
            $"SpawnedShellsParent reference in GameManager ({gameObject.name}) is null. Drag into the inspector");
        Debug.Assert(_eventManager != null,
            $"EventManager reference in GameManager ({gameObject.name}) is null. Drag into the inspector");
    }

    protected override void Awake()
    {
        base.Awake();
        
        _eventManager.OnCameraChanged.AddListener((newMode) =>
        {
            HUDManager.Instance.HandleCamModeUI(newMode);
        });
        
        foreach (var entity in _entitiesList)
        {
            if (!entity.TryGetComponent(out TankComponentManager tankManager)) continue;
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
        HUDManager.Instance.DestroyEntityIndicator(entityID);
    }
}