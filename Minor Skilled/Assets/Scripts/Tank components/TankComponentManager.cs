using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankComponentManager : MonoBehaviour
{
    public TankProperties Properties;
    public BaseHUDManager EntityHUD;
    public EventManager EventManager { get; private set; }
    public MoveComponent MoveComponent { get; private set; }
    public TurretControlComponent TurretControlComponent { get; private set; }
    public BaseShootComponent ShootComponent { get; private set; }

    public bool HasDied;

    private void Awake()
    {
        MoveComponent = GetComponent<MoveComponent>();
        TurretControlComponent = GetComponent<TurretControlComponent>();
        ShootComponent = GetComponent<BaseShootComponent>();
        
        EventManager = GetComponent<EventManager>();
    }

    private void Start()
    {
        EntityHUD.UpdateName(Properties.TankName);
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