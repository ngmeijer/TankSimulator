using System;
using UnityEngine;

public enum E_TankState
{
    Inspection,
    Combat,
    Death
};

public abstract class TankState : FSMState
{
    public TankComponentManager ComponentManager;
    public CameraComponent CameraComponent;
    public TankProperties Properties { get; private set; }
    public E_TankState ThisState;
    
    protected void Awake()
    {
        Properties = ComponentManager.Properties;
    }

    protected virtual void Start()
    {
        Debug.Assert(ComponentManager != null, "ComponentManager in a TankState is null. Drag it into the inspector slot.");
        Debug.Assert(CameraComponent != null, "CameraComponent in a TankState is null. Drag it into the inspector slot.");
    }
}