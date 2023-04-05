using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    private const int PLAYER_ID = 0;
    [SerializeField] private Transform _player;
    public Transform GetPlayer() => _player;

    [SerializeField] private Transform _spawnedShellsParent;
    public Transform GetShellParent() => _spawnedShellsParent;
    [SerializeField] private Transform _vfxParent;
    public Transform GetVFXParent() => _vfxParent;

    private Dictionary<int, TankComponentManager> _entities = new();
    public Dictionary<int, TankComponentManager> GetEntities() => _entities;
    [SerializeField] private EventManager _eventManager;
    public Vector3 CurrentBarrelCrosshairPos;
    public Vector3 TargetBarrelCrosshairPos;

    public Vector3 InspectCameraPosition;
    
    public static Vector3 HandlesOffset = new Vector3(0.5f, 0.5f, 0);

    private void OnValidate()
    {
        Debug.Assert(_player != null,
            $"Player reference in GameManager ({gameObject.name}) is null. Drag into the inspector");
        Debug.Assert(_spawnedShellsParent != null,
            $"SpawnedShellsParent reference in GameManager ({gameObject.name}) is null. Drag into the inspector");
        Debug.Assert(_vfxParent != null,
            $"VFXParent reference in GameManager ({gameObject.name}) is null. Drag into the inspector");
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
    }

    private void Start()
    {
        int id = PLAYER_ID + 1;
        _entities.Add(id, _player.GetComponent<TankComponentManager>());
        List<Transform> enemies = EnemyManager.Instance.GetEnemies();

        foreach (var enemyInstance in enemies)
        {
            if (!TryGetComponent(out TankComponentManager tank)) continue;

            id++;
            tank.ID = id;
            _entities.Add(id, enemyInstance.GetComponent<TankComponentManager>());
        }
    }
}