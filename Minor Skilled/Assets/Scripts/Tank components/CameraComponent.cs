using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum CameraMode
{
    ADS,
    ThirdPerson
}

public class CameraComponent : TankComponent
{
    [Tooltip("The lower the value, the lower the move speed!")][SerializeField] private float _cameraRotationDamp = 1f;

    [Header("ADS properties")] 
    [SerializeField] private Camera _adsCam;
    [SerializeField] private Transform _adsTargetPos;

    [Header("Third person properties")] 
    [SerializeField] private Camera _thirdPersonCam;
    [SerializeField] private Transform _cameraTargetDestination;
    [SerializeField] private Transform _lookAtPosition;
    [SerializeField] private Transform _thirdPersonRotationAnchor;

    [Header("Y Ranges")]
    [SerializeField] private Transform _cameraLowerBound;
    [SerializeField] private Transform _cameraUpperBound;

    private Vector3 _offsetOnTilt;
    private Camera _currentCamera;
    public CameraMode CamMode { get; private set; }
    private CameraMode _previousCamMode;
    private float _lastMoveValue;
    private Vector3 _lastTankPosition;
    private float _cameraDampOnCannonTilt = 0.1f;
    private int _currentCameraFOVLevel = 0;
    private Vector3 _handlesOffset = new Vector3(0.5f, 0.5f, 0);

    private int[] _fovRanges = new int[4]
    {
        60,
        20,
        10,
        5
    };

    private void Start()
    {
        EnableThirdPerson();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _thirdPersonCam.transform.position = _cameraTargetDestination.position;
    }

    private void LateUpdate()
    {
        GameManager.Instance.RotationCrosshairPosition = GetScreenpointRotationCrosshair();
        CheckCameraSwitch();
    }

    private void CheckCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EnableADS();
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            EnableThirdPerson();
    }

    private Vector3 GetScreenpointRotationCrosshair()
    {
        switch (CamMode)
        {
            case CameraMode.ADS:
                return _currentCamera.WorldToScreenPoint(_adsTargetPos.position);
            case CameraMode.ThirdPerson:
                Vector3 posToConvert = new Vector3(_adsTargetPos.position.x, _lookAtPosition.position.y,
                    _adsTargetPos.position.z);
                return _currentCamera.WorldToScreenPoint(posToConvert);
        }

        return Vector3.zero;
    }

    private void EnableADS()
    {
        CamMode = CameraMode.ADS;
        ChangeCameraPerspective(_adsCam, true, false);
    }

    private void EnableThirdPerson()
    {
        CamMode = CameraMode.ThirdPerson;
        ChangeCameraPerspective(_thirdPersonCam, false, true);
    }

    private void ChangeCameraPerspective(Camera currentCamera, bool adsCamState, bool tpCamState)
    {
        _componentManager.EventManager.OnCameraChanged.Invoke(CamMode);
        _currentCamera = currentCamera;
        _adsCam.gameObject.SetActive(adsCamState);
        _thirdPersonCam.gameObject.SetActive(tpCamState);
    }

    public void UpdateCameraPosition()
    {
        if (CamMode != CameraMode.ThirdPerson) return;
        
        //Getting min & max values & total moving range
        Vector3 minY = _cameraLowerBound.position;
        Vector3 maxY = _cameraUpperBound.position;
        Vector3 maxLength = maxY - minY;
        
        //Calculate current Y Position
        float inverseValue = 1 - _componentManager.RotationValue;
        Vector3 yDelta = inverseValue * maxLength;
        Vector3 newPosition = minY + yDelta;
        
        Vector3 currentPosition = _currentCamera.transform.position;
        _cameraTargetDestination.position = newPosition;
        _currentCamera.transform.position = Vector3.MoveTowards(currentPosition, _cameraTargetDestination.position, _cameraRotationDamp * Time.deltaTime);
        _currentCamera.transform.LookAt(_lookAtPosition.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_lookAtPosition.position, 0.2f);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(_thirdPersonRotationAnchor.position, 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_cameraLowerBound.position, 0.25f);
        Handles.Label(_cameraLowerBound.position + _handlesOffset, _cameraLowerBound.name);
        Gizmos.DrawSphere(_cameraUpperBound.position, 0.25f);
        Handles.Label(_cameraUpperBound.position + _handlesOffset, _cameraUpperBound.name);
        Gizmos.DrawLine(_cameraLowerBound.position, _cameraUpperBound.position);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_cameraTargetDestination.position, 0.2f);
        Handles.Label(_cameraTargetDestination.position + _handlesOffset, _cameraTargetDestination.name);

        if (_currentCamera != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_currentCamera.transform.position, 0.15f);
            Handles.Label(_currentCamera.transform.position + _handlesOffset, _currentCamera.name);
        }
    }

    public void ZoomADS()
    {
        if (CamMode != CameraMode.ADS) return;

        if (_currentCameraFOVLevel >= _fovRanges.Length - 1)
            _currentCameraFOVLevel = 0;
        else _currentCameraFOVLevel++;

        float newFOV = _fovRanges[_currentCameraFOVLevel];
        _adsCam.fieldOfView = newFOV;
    }
}