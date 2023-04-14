using System;
using UnityEngine;

public class PlayerStateSwitcher : StateSwitcher
{
    [Header("Camera states")]
    public E_CameraState DefaultCamState = E_CameraState.ThirdPerson;
    [SerializeField] private CameraState _tpState;
    [SerializeField] private CameraState _adsState;
    [SerializeField] private CameraState _inspectorCamState;
    [SerializeField] private CameraState _hostileInspectorCamState;
    
    public CameraState CurrentCameraState { get; private set; }

    private void Awake()
    {
        _inspectorCamState.ExitState();
        _adsState.ExitState();
        _hostileInspectorCamState.ExitState();
    }

    private void Start()
    {
        Debug.Assert(_tpState != null, "ThirdPersonView reference is null. Drag it into the inspector slot.");
        Debug.Assert(_adsState != null, "ADSView reference is null. Drag it into the inspector slot.");
        Debug.Assert(_inspectorCamState != null, "InspectorView reference is null. Drag it into the inspector slot.");
        
        SwitchToTankState(DefaultTankState);
        SwitchToCamState(DefaultCamState);
    }

    private void Update()
    {
        base.Update();

        if (CurrentCameraState == null) return;
        CurrentCameraState.UpdateState();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (CurrentCameraState == null) return;
        CurrentCameraState.FixedUpdateState();
    }

    private void LateUpdate()
    {
        base.LateUpdate();
        
        if (CurrentCameraState == null) return;
        CurrentCameraState.LateUpdateState();
    }

    public void SwitchToCamState(E_CameraState newStateEnum)
    {
        if (CurrentCameraState != null)
        {
            if (CurrentCameraState.ThisState == newStateEnum)
                return;
            
            CurrentCameraState.ExitState();
        }

        CameraState newState = null;
        
        switch (newStateEnum)
        {
            case E_CameraState.ADS:
                newState = _adsState;
                break;
            case E_CameraState.ThirdPerson:
                newState = _tpState;
                break;
            case E_CameraState.InspectMode:
                newState = _inspectorCamState;
                break;
            case E_CameraState.HostileInspection:
                newState = _hostileInspectorCamState;
                break;
        }
        
        newState.EnterState();
        CurrentCameraState = newState;
    }
}