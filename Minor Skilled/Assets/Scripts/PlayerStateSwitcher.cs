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
    
    public CameraState CurrentCameraState
    {
        get => _currentCameraState;
        set => _currentCameraState = value;
    }
    private CameraState _currentCameraState;
    
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

        if (_currentCameraState == null) return;
        _currentCameraState.UpdateState();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (_currentCameraState == null) return;
        _currentCameraState.FixedUpdateState();
    }

    private void LateUpdate()
    {
        base.LateUpdate();
        
        if (_currentCameraState == null) return;
        _currentCameraState.LateUpdateState();
    }

    public void SwitchToCamState(E_CameraState newStateEnum)
    {
        if (_currentCameraState != null)
        {
            if (_currentCameraState.ThisState == newStateEnum)
                return;
            
            _currentCameraState.ExitState();
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

        if (_currentCameraState != null)
            _currentCameraState.NextCameraTrans = newState.ViewCam.transform;
        newState.EnterState();
        _currentCameraState = newState;
    }
}