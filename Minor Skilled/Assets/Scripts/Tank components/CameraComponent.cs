using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public enum CameraMode
{
    ADS,
    ThirdPerson,
    InspectMode
}

public class CameraComponent : TankComponent
{
    [Header("ADS properties")] [SerializeField]
    private Camera _adsCam;

    [SerializeField] private Transform _adsTargetPos;

    [Header("Targets")] [SerializeField] private Transform _raycaster;
    [SerializeField] private Transform _currentBarrelCrosshair;
    [SerializeField] private Transform _estimatedTargetCrosshair;

    private Vector3 _offsetOnTilt;
    public Camera CurrentCamera { get; private set; }
    public CameraMode CamMode { get; private set; }
    private CameraMode _previousCamMode;
    private float _lastMoveValue;
    private Vector3 _lastTankPosition;
    private float _cameraDampOnCannonTilt = 0.1f;
    private RaycastHit _currentHitData;
    private string _colliderTag;

    [SerializeField] private ThirdPersonView _tpView;
    [SerializeField] private AdsView _adsView;
    [SerializeField] private TankInspectorView _inspectorView;

    private void Start()
    {
        EnableThirdPerson();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        Debug.Assert(_tpView != null, "ThirdPersonView reference is null. Drag it into the inspector slot.");
        Debug.Assert(_adsView != null, "ADSView reference is null. Drag it into the inspector slot.");
        Debug.Assert(_inspectorView != null, "InspectorView reference is null. Drag it into the inspector slot.");
    }

    private void LateUpdate()
    {
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
        _adsView.ZoomADS();
    }

    public void RotateAroundTank(float mouseInput, float scrollInput)
    {
        _inspectorView.RotateAroundTank(mouseInput);
        _inspectorView.ZoomInspectView(scrollInput);
    }

    public void UpdateThirdPersonCameraPosition()
    {
        _tpView.UpdateThirdPersonCameraPosition();
    }

    private Vector3 ConvertCurrentBarrelCrosshair()
    {
        Vector3 posToConvert = _currentHitData.point == Vector3.zero || _colliderTag == "Shell"
            ? _currentBarrelCrosshair.position
            : _currentHitData.point;
        
        return CurrentCamera.WorldToScreenPoint(posToConvert);
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
        
        Vector3 convertedPos = CurrentCamera.WorldToScreenPoint(_estimatedTargetCrosshair.position);
        convertedPos.y = GameManager.Instance.CurrentBarrelCrosshairPos.y;
        return convertedPos;
    }

    public void EnableADS()
    {
        CamMode = CameraMode.ADS;
        
        HUDManager.Instance.EnableCombatUI(true);
        HUDManager.Instance.EnableDamageUI(false, null);
        ChangeCameraPerspective(_adsView, true, false, false);
    }

    public void EnableThirdPerson()
    {
        CamMode = CameraMode.ThirdPerson;
        
        HUDManager.Instance.EnableCombatUI(true);
        HUDManager.Instance.EnableDamageUI(false, null);
        ChangeCameraPerspective(_tpView, false, true,false);
    }
    
    public void EnableInspectCamera()
    {
        CamMode = CameraMode.InspectMode;
        
        ChangeCameraPerspective(_inspectorView, false, false, true);
    }

    private void ChangeCameraPerspective(CameraView currentView, bool adsCamState, bool tpCamState, bool inspectCamState)
    {
        _componentManager.EventManager.OnCameraChanged.Invoke(CamMode);
        CurrentCamera = currentView.ViewCam;
        _adsCam.gameObject.SetActive(adsCamState);

        _adsView.SetView(adsCamState);
        _tpView.SetView(tpCamState);
        _inspectorView.SetView(inspectCamState);
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