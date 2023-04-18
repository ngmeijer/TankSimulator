using System;
using UnityEngine;

[Serializable]
public enum E_TankState
{
    Inspection,
    Combat,
    Death,
    HostileInspection
};

public abstract class TankState : FSMState
{
    protected TankComponentManager _componentManager;
    protected TankProperties Properties { get; private set; }
    public E_TankState ThisState;
    
    protected void Awake()
    {
        _componentManager = GetComponentInParent<TankComponentManager>();
        Properties = _componentManager.Properties;
    }

    protected virtual void Start()
    {
        Debug.Assert(_componentManager != null, "ComponentManager in a TankState is null. Drag it into the inspector slot.");
    }
}