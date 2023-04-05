using System;
using UnityEngine;

public abstract class TankState : MonoBehaviour
{
    public TankComponentManager ComponentManager;
    public CameraComponent CameraComponent;
    public TankProperties Properties { get; private set; }
    
    public bool StateActive;

    protected void Awake()
    {
        Properties = ComponentManager.Properties;
    }

    protected virtual void Start()
    {
        Debug.Assert(CameraComponent != null, "CameraComponent in a TankState is null. Drag it into the inspector slot.");
    }

    public virtual void EnterState()
    {
        StateActive = true;
    }

    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void LateUpdateState();
    protected abstract void GetInputValues();

    public virtual void ExitState()
    {
        StateActive = false;
    }
}
