using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TankComponentManager : MonoBehaviour
{
    public StateSwitcher StateSwitcher;
    public TankProperties Properties;
    public EventManager EventManager { get; private set; }
    public MoveComponent MoveComponent { get; private set; }
    public TurretControlComponent TurretControlComponent { get; private set; }
    public ShootComponent ShootComponent { get; private set; }
    public DamageRegistrationComponent DamageComponent { get; private set; }

    public Transform EntityOrigin;
    
    public bool HasDied;
    public int ID;

    private void Awake()
    {
        StateSwitcher = GetComponent<StateSwitcher>();
        MoveComponent = GetComponentInChildren<MoveComponent>();
        TurretControlComponent = GetComponentInChildren<TurretControlComponent>();
        ShootComponent = GetComponentInChildren<ShootComponent>();
        DamageComponent = GetComponentInChildren<DamageRegistrationComponent>();
        
        EventManager = GetComponent<EventManager>();
    }

    private void Start()
    {
        if(HUDManager.Instance != null)
            HUDManager.Instance.UpdateEntityName(ID, Properties.TankName);
        EventManager.OnTankDestruction.AddListener(OnTankDeath);
    }

    private void OnTankDeath(int entityID)
    {
        HasDied = true;
    }

    public Vector3 GetCurrentBarrelDirection() => TurretControlComponent.GetCurrentBarrelDirection();
    public Vector3 GetBarrelEuler() => TurretControlComponent.GetBarrelEuler();
    
    public List<WheelCollider> GetLeftWheelColliders()
    {
        if (MoveComponent != null)
            return MoveComponent.GetLeftWheelColliders();
        
        Debug.LogError($"There is no MoveComponent attached to {this.gameObject.name}. Cannot retrieve wheels");
        return null;
    }

    public List<WheelCollider> GetRightWheelColliders()
    {
        if (MoveComponent != null)
            return MoveComponent.GetRightWheelColliders();
        
        Debug.LogError($"There is no MoveComponent attached to {this.gameObject.name}. Cannot retrieve wheels");
        return null;
    }
}