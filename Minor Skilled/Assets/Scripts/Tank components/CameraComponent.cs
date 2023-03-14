using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    ADS,
    FirstPerson,
    ThirdPerson
}

public class CameraComponent : TankComponent
{
    [Header("ADS properties")] 
    [SerializeField] private Camera _adsCam;

    [Header("First person properties")] 
    [SerializeField] private Camera _firstPersonCam;
    [SerializeField] private Transform _firstPersonPivot;
    [SerializeField] private Transform _firstPersonCameraFocus;

    [Header("Third person properties")] 
    [SerializeField] private Camera _thirdPersonCam;
    [SerializeField] private Transform _thirdPersonPivot;
    [SerializeField] private Transform _thirdPersonCameraFocus;
    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private float _distanceDamp = 10f;

    private float _thirdPersonCamOffsetY;
    private Camera _currentCamera;
    private Transform _currentCameraPivot;
    private CameraMode _camMode;
    private CameraMode _previousCamMode;

    private float _lastMoveValue;
    private Vector3 _lastTankPosition;

    private void Start()
    {
        EnableFirstPerson();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        CheckCameraSwitch();
    }

    public void ForceADSView()
    {
        if (_camMode != CameraMode.ADS)
        {
            _previousCamMode = _camMode;
            EnableADS();
        }
        else
        {
            switch (_previousCamMode)
            {
                case CameraMode.FirstPerson:
                    EnableFirstPerson();
                    break;
                case CameraMode.ThirdPerson:
                    EnableThirdPerson();
                    break;
            }
        }
    }

    private void CheckCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EnableADS();
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            EnableFirstPerson();
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            EnableThirdPerson();
    }

    private void EnableADS()
    {
        _camMode = CameraMode.ADS;

        _currentCamera = _adsCam;
        _adsCam.gameObject.SetActive(true);
        _firstPersonCam.gameObject.SetActive(false);
        _thirdPersonCam.gameObject.SetActive(false);
    }

    private void EnableFirstPerson()
    {
        _camMode = CameraMode.FirstPerson;

        _currentCamera = _firstPersonCam;
        _firstPersonCam.gameObject.SetActive(true);
        _adsCam.gameObject.SetActive(false);
        _thirdPersonCam.gameObject.SetActive(false);
    }

    private void EnableThirdPerson()
    {
        _camMode = CameraMode.ThirdPerson;

        _currentCamera = _thirdPersonCam;
        _thirdPersonCam.gameObject.SetActive(true);
        _firstPersonCam.gameObject.SetActive(false);
        _adsCam.gameObject.SetActive(false);
    }

    public void UpdateCameraPosition()
    {
        if (_camMode == CameraMode.FirstPerson)
        {
            Vector3 targetPosition = _firstPersonPivot.position;
            Vector3 lerpedPosition = Vector3.Lerp(_currentCamera.transform.position, targetPosition, 100f * Time.deltaTime);
            _currentCamera.transform.position = lerpedPosition;

            _currentCamera.transform.LookAt(_firstPersonCameraFocus.position);
        }
        
        if (_camMode == CameraMode.ThirdPerson)
        {
            Vector3 targetPosition = _thirdPersonPivot.position + (_thirdPersonCameraFocus.parent.rotation * _cameraOffset) + new Vector3(0, _thirdPersonCamOffsetY, 0);
            Vector3 slerpedPosition =
                Vector3.Slerp(_currentCamera.transform.position, targetPosition, _distanceDamp * Time.deltaTime);
            _currentCamera.transform.position = slerpedPosition;

            _currentCamera.transform.LookAt(_thirdPersonCameraFocus.position);
        }
    }

    public void OffsetCameraOnCannonTilt(float mouseTiltInput)
    {
        //Inverts the delta for the camera. Cannon moves up, camera moves down.
        _thirdPersonCamOffsetY -= mouseTiltInput * _properties.TurretTiltSpeed * 0.1f * Time.deltaTime;
        _thirdPersonCamOffsetY = Mathf.Clamp(_thirdPersonCamOffsetY, -7f, 3f);
    }
}