using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public enum CameraMode
{
    None,
    ADS,
    ThirdPerson,
    InspectMode
}

public class CameraComponent : TankComponent
{
    [Header("Targets")] [SerializeField] private Transform _raycaster;
    [SerializeField] private Transform _currentBarrelCrosshair;
    [SerializeField] private Transform _estimatedTargetCrosshair;

    public CameraMode CamMode { get; private set; } = CameraMode.None;
    private RaycastHit _currentHitData;
    private string _colliderTag;
    private bool _inTransition;
    [SerializeField] private float _transitionDuration = 1f;

    [SerializeField] private ThirdPersonState _tpState;
    [SerializeField] private AdsState _adsState;
    [SerializeField] private InspectorCamState _inspectorCamState;
    private CameraState _currentState;

    private void Start()
    {
        _inspectorCamState.ExitState();
        _adsState.ExitState();
        SwitchToState(CameraMode.ThirdPerson);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Assert(_tpState != null, "ThirdPersonView reference is null. Drag it into the inspector slot.");
        Debug.Assert(_adsState != null, "ADSView reference is null. Drag it into the inspector slot.");
        Debug.Assert(_inspectorCamState != null, "InspectorView reference is null. Drag it into the inspector slot.");
    }

    private void Update()
    {
        _currentState.UpdateState();

        _tpState.RotationValue = _componentManager.RotationValue;
    }

    private void LateUpdate()
    {
        _currentState.LateUpdateState();

        if (CamMode == CameraMode.InspectMode) return;

        GameManager.Instance.CurrentBarrelCrosshairPos = ConvertCurrentBarrelCrosshair();
        GameManager.Instance.TargetBarrelCrosshairPos = ConvertTargetBarrelCrosshair();

        if (GetEstimatedHitPoint())
        {
            _currentBarrelCrosshair.position = _currentHitData.point;
            Debug.DrawLine(_raycaster.position, _currentHitData.point, Color.green);
        }
        else Debug.DrawLine(_raycaster.position, _raycaster.forward * 1000f, Color.red);
    }

    public void ZoomADS()
    {
        _adsState.ZoomADS();
    }

    private Vector3 ConvertCurrentBarrelCrosshair()
    {
        Vector3 posToConvert = _currentHitData.point == Vector3.zero || _colliderTag == "Shell"
            ? _currentBarrelCrosshair.position
            : _currentHitData.point;

        return _currentState.ViewCam.WorldToScreenPoint(posToConvert);
    }

    private Vector3 ConvertTargetBarrelCrosshair()
    {
        Vector3 currentPos = _estimatedTargetCrosshair.position;
        currentPos.x = _currentBarrelCrosshair.position.x;
        currentPos.y = _currentBarrelCrosshair.position.y;
        currentPos.z = _currentBarrelCrosshair.position.z;
        _estimatedTargetCrosshair.position = currentPos;
        Vector3 currentLocalPos = _estimatedTargetCrosshair.localPosition;
        currentLocalPos.x = 0;
        _estimatedTargetCrosshair.localPosition = currentLocalPos;

        Vector3 convertedPos = _currentState.ViewCam.WorldToScreenPoint(_estimatedTargetCrosshair.position);
        convertedPos.y = GameManager.Instance.CurrentBarrelCrosshairPos.y;
        return convertedPos;
    }

    private void AnimateCamera(CameraState newState)
    {
        _currentState.ViewCam.transform.DOMove(newState.ViewCam.transform.position, _transitionDuration);
        _currentState.ViewCam.transform.DORotate(newState.ViewCam.transform.rotation.eulerAngles, _transitionDuration);
        _currentState.ViewCam.transform.DOLookAt(newState.StateLookAt.position, _transitionDuration);
    }

    public void SwitchToState(CameraMode newMode)
    {
        if (CamMode == newMode) return;

        if (_currentState != null)
            _currentState.ExitState();

        CameraState newState = null;
        switch (newMode)
        {
            case CameraMode.ADS:
                newState = _adsState;
                break;
            case CameraMode.ThirdPerson:
                newState = _tpState;
                break;
            case CameraMode.InspectMode:
                newState = _inspectorCamState;
                break;
        }

        _componentManager.EventManager.OnCameraChanged.Invoke(newMode);

        _currentState = newState;
        CamMode = newMode;
        _currentState.EnterState();
        //AnimateCamera(newState);
    }

    private bool GetEstimatedHitPoint()
    {
        bool data = Physics.Raycast(_raycaster.position, _raycaster.forward,
            out _currentHitData, Mathf.Infinity);

        if (_currentHitData.collider != null)
            _colliderTag = _currentHitData.collider.tag;
        else _colliderTag = "No collision detected";

        return data && !_currentHitData.collider.CompareTag("Shell");
    }

    private void OnDrawGizmos()
    {
        if (_currentHitData.point != Vector3.zero && !_currentHitData.collider.CompareTag("Shell"))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_currentHitData.point, 0.3f);
            Handles.Label(_currentHitData.point + GameManager.HandlesOffset, "Hitpoint");
        }

        Handles.Label(_raycaster.position, $"Collider hit: {_colliderTag}");

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(_currentBarrelCrosshair.position, 0.2f);
        Handles.Label(_currentBarrelCrosshair.position + GameManager.HandlesOffset,
            _currentBarrelCrosshair.name);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(_estimatedTargetCrosshair.position, 0.2f);
        Handles.Label(_estimatedTargetCrosshair.position + GameManager.HandlesOffset,
            _estimatedTargetCrosshair.name);
    }
}