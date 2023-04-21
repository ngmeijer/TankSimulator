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
    [SerializeField] private CameraState _deathCamState;
    
    public CameraState CurrentCameraState { get; private set; }
    public E_CameraState CamStateEnum;
    
    public CameraState LastCameraState { get; private set; }
    
    private void Start()
    {
        Debug.Assert(_tpState != null, "ThirdPersonView reference is null. Drag it into the inspector slot.");
        Debug.Assert(_adsState != null, "ADSView reference is null. Drag it into the inspector slot.");
        Debug.Assert(_inspectorCamState != null, "InspectorView reference is null. Drag it into the inspector slot.");

        _tpState.ExitState();
        _inspectorCamState.ExitState();
        _adsState.ExitState();
        _hostileInspectorCamState.ExitState();
        _deathCamState.ExitState();
        
        SwitchToTankState(DefaultTankState);
        SwitchToCamState(DefaultCamState);
        HUDStateSwitcher.Instance.SwitchToHUDState(DefaultTankState);
        
        GameManager.Instance.Player.EventManager.OnTankDestruction.AddListener(SwitchToDeathState);
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
            LastCameraState = CurrentCameraState;
        }
        
        CameraState newState = newStateEnum switch
        {
            E_CameraState.ADS => _adsState,
            E_CameraState.ThirdPerson => _tpState,
            E_CameraState.InspectMode => _inspectorCamState,
            E_CameraState.HostileInspection => _hostileInspectorCamState,
            E_CameraState.Death => _deathCamState
        };

        if (CurrentCameraState == null)
        {
            newState.ViewCam.transform.position = newState.CameraTargetDestination.position;
        }
        else newState.ViewCam.transform.position = CurrentCameraState.ViewCam.transform.position;
        newState.EnterState();
        CurrentCameraState = newState;
        CamStateEnum = CurrentCameraState.ThisState;
    }

    private void SwitchToDeathState(int index)
    {
        SwitchToCamState(E_CameraState.Death);
        HUDStateSwitcher.Instance.SwitchToHUDState(E_TankState.Death);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if(CurrentCameraState != null)
            Gizmos.DrawSphere(CurrentCameraState.ViewCam.transform.position, 0.2f);
    }
}