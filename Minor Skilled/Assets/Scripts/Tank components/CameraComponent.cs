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
    private const float THIRD_PERSON_DIST_DAMP = 10f;
    private const float FIRST_PERSON_DIST_DAMP = 100F;
    
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
    [SerializeField] private float _camLowestY = -7;
    [SerializeField] private float _camHighestY = 3;

    private Vector3 _thirdPersonCamOffset;
    private Camera _currentCamera;
    private Transform _currentCameraPivot;
    private CameraMode _camMode;
    private CameraMode _previousCamMode;
    private float _lastMoveValue;
    private Vector3 _lastTankPosition;
    private float _cameraDampOnCannonTilt = 0.1f;

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
        ChangeCameraPerspective(_adsCam, true, false, false);
    }

    private void EnableFirstPerson()
    {
        _camMode = CameraMode.FirstPerson;
        ChangeCameraPerspective(_firstPersonCam, false, true, false);
    }

    private void EnableThirdPerson()
    {
        _camMode = CameraMode.ThirdPerson;
        ChangeCameraPerspective(_thirdPersonCam, false, false, true);
    }

    private void ChangeCameraPerspective(Camera currentCamera, bool adsCamState, bool fpsCamState, bool tpCamState)
    {
        _currentCamera = currentCamera;
        _adsCam.gameObject.SetActive(adsCamState);
        _firstPersonCam.gameObject.SetActive(fpsCamState);
        _thirdPersonCam.gameObject.SetActive(tpCamState);
    }

    public void UpdateCameraPosition()
    {
        if (_camMode == CameraMode.FirstPerson)
        {
            Vector3 targetPosition = _firstPersonPivot.position;
            Vector3 lerpedPosition = Vector3.Lerp(_currentCamera.transform.position, targetPosition, FIRST_PERSON_DIST_DAMP * Time.deltaTime);
            _currentCamera.transform.position = lerpedPosition;

            _currentCamera.transform.LookAt(_firstPersonCameraFocus.position);
        }
        
        if (_camMode == CameraMode.ThirdPerson)
        {
            Vector3 targetPosition =
                _thirdPersonPivot.position + (_thirdPersonCameraFocus.parent.rotation * _cameraOffset) + _thirdPersonCamOffset;
            Vector3 slerpedPosition =
                Vector3.Slerp(_currentCamera.transform.position, targetPosition, THIRD_PERSON_DIST_DAMP * Time.deltaTime);
            _currentCamera.transform.position = slerpedPosition;

            _currentCamera.transform.LookAt(_thirdPersonCameraFocus.position);
        }
    }

    public void OffsetCameraOnCannonTilt(float mouseTiltInput)
    {
        //Inverts the delta for the camera. Cannon moves up, camera moves down.
        _thirdPersonCamOffset.y -= mouseTiltInput * _properties.TurretTiltSpeed * _cameraDampOnCannonTilt * Time.deltaTime;
        _thirdPersonCamOffset.y = Mathf.Clamp(_thirdPersonCamOffset.y, _camLowestY, _camHighestY);
    }
}