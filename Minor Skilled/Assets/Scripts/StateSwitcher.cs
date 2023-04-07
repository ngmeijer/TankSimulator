using System;
using UnityEngine;

public class StateSwitcher : MonoBehaviour
{
    [Header("Tank states")]
    public E_TankState DefaultTankState = E_TankState.Combat;
    [SerializeField] private TankState _inspectionState;
    [SerializeField] private TankState _combatState;
    [SerializeField] private TankState _deathState;
    public TankState CurrentTankState
    {
        get => _currentTankState;
        set => _currentTankState = value;
    }
    private TankState _currentTankState;

    [Header("Camera states")]
    public E_CameraState DefaultCamState = E_CameraState.ThirdPerson;
    [SerializeField] private CameraState _tpState;
    [SerializeField] private CameraState _adsState;
    [SerializeField] private CameraState _inspectorCamState;
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
        if (_currentTankState == null) return;
        _currentTankState.UpdateState();

        if (_currentCameraState == null) return;
        _currentCameraState.UpdateState();
    }

    private void FixedUpdate()
    {
        if (_currentTankState == null) return;
        
        _currentTankState.FixedUpdateState();
        
        if (_currentCameraState == null) return;
        _currentCameraState.FixedUpdateState();
    }

    private void LateUpdate()
    {
        if (_currentTankState == null) return;
        
        _currentTankState.LateUpdateState();
        
        if (_currentCameraState == null) return;
        _currentCameraState.LateUpdateState();
    }

    public void SwitchToTankState(E_TankState newStateEnum)
    {
        if(_currentTankState != null)
            _currentTankState.ExitState();

        TankState newState = null;
        
        switch (newStateEnum)
        {
            case E_TankState.Inspection:
                newState = _inspectionState;
                break;
            case E_TankState.Combat:
                newState = _combatState;
                break;
            case E_TankState.Death:
                newState = _deathState;
                break;
        }
        
        newState.EnterState();
        _currentTankState = newState;
    }

    public void SwitchToCamState(E_CameraState newStateEnum)
    {
        if(_currentTankState != null)
            _currentTankState.ExitState();

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
        }
        
        newState.EnterState();
        _currentCameraState = newState;
        Debug.Log($"Switched to {_currentCameraState.ThisState}");
    }
}