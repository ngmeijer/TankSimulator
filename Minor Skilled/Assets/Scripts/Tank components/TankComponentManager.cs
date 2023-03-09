using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TankComponentManager : MonoBehaviour
{
    public TankProperties Properties;
    public BaseHUDManager EntityHUD;
    public EventManager EventManager { get; private set; }
    private MoveComponent moveComponent;
    private TurretControlComponent turretControlComponent;

    public bool HasDied;

    private void Awake()
    {
        moveComponent = GetComponent<MoveComponent>();
        turretControlComponent = GetComponent<TurretControlComponent>();
        EventManager = GetComponent<EventManager>();
    }

    private void Start()
    {
        EntityHUD.UpdateName(Properties.TankName);
    }

    public Vector3 GetCurrentBarrelDirection() => turretControlComponent.GetCurrentBarrelDirection();
    public Vector3 GetBarrelEuler() => turretControlComponent.GetBarrelEuler();
    
    public List<WheelCollider> GetLeftWheelColliders()
    {
        if (moveComponent != null)
            return moveComponent.GetLeftWheelColliders();
        
        Debug.LogError($"There is no MoveComponent attached to {this.gameObject.name}. Cannot retrieve wheels");
        return null;
    }

    public List<WheelCollider> GetRightWheelColliders()
    {
        if (moveComponent != null)
            return moveComponent.GetRightWheelColliders();
        
        Debug.LogError($"There is no MoveComponent attached to {this.gameObject.name}. Cannot retrieve wheels");
        return null;
    }
}