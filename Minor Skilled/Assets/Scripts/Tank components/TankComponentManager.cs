using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TankComponentManager : MonoBehaviour
{
    public StateSwitcher ThisStateSwitcher;
    public TankProperties Properties;
    public EventManager EventManager { get; private set; }
    public NewMoveComponent MoveComp { get; private set; }
    public TurretControlComponent TurretControlComp { get; private set; }
    public ShootComponent ShootComp { get; private set; }
    public DamageRegistrationComponent DamageComp { get; private set; }

    public Transform EntityOrigin;
    public Transform Raycaster;
    
    public bool HasDied;
    public int ID;
    
    private void Awake()
    {
        ThisStateSwitcher = GetComponent<StateSwitcher>();
        MoveComp = GetComponentInChildren<NewMoveComponent>();
        TurretControlComp = GetComponentInChildren<TurretControlComponent>();
        ShootComp = GetComponentInChildren<ShootComponent>();
        DamageComp = GetComponentInChildren<DamageRegistrationComponent>();
        EventManager = GetComponent<EventManager>();
    }

    private void Start()
    {
        EventManager.OnTankDestruction.AddListener(OnTankDeath);
    }

    private void OnTankDeath(int entityID)
    {
        HasDied = true;
    }

    public Vector3 GetCurrentBarrelDirection() => TurretControlComp.GetCurrentBarrelDirection();
    
    public List<WheelCollider> GetLeftWheelColliders()
    {
        // if (MoveComponent != null)
        //     return MoveComponent.GetLeftWheelColliders();
        
        Debug.LogError($"There is no MoveComponent attached to {this.gameObject.name}. Cannot retrieve wheels");
        return null;
    }

    public List<WheelCollider> GetRightWheelColliders()
    {
        // if (MoveComponent != null)
        //     return MoveComponent.GetRightWheelColliders();
        
        Debug.LogError($"There is no MoveComponent attached to {this.gameObject.name}. Cannot retrieve wheels");
        return null;
    }
}