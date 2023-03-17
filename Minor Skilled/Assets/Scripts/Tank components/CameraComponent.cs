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
    [SerializeField] private Transform _thirdPersonTargetPos;
    [SerializeField] private Transform _thirdPersonCameraFocus;
    [SerializeField] private Transform _thirdPersonRotationAnchor;
    [SerializeField] private float _camLowestY = -7;
    [SerializeField] private float _camHighestY = 3;

    private Vector3 _offsetOnTilt;
    private Transform _currentCameraTransform;
    public CameraMode CamMode { get; private set; }
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
        if (CamMode != CameraMode.ADS)
        {
            _previousCamMode = CamMode;
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
        CamMode = CameraMode.ADS;
        ChangeCameraPerspective(_adsCam, true, false, false);
    }

    private void EnableFirstPerson()
    {
        CamMode = CameraMode.FirstPerson;
        ChangeCameraPerspective(_firstPersonCam, false, true, false);
    }

    private void EnableThirdPerson()
    {
        CamMode = CameraMode.ThirdPerson;
        ChangeCameraPerspective(_thirdPersonCam, false, false, true);
    }

    private void ChangeCameraPerspective(Camera currentCamera, bool adsCamState, bool fpsCamState, bool tpCamState)
    {
        _currentCameraTransform = currentCamera.transform;
        _adsCam.gameObject.SetActive(adsCamState);
        _firstPersonCam.gameObject.SetActive(fpsCamState);
        _thirdPersonCam.gameObject.SetActive(tpCamState);
    }

    public void UpdateCameraPosition(float rotateInput)
    {
        _thirdPersonRotationAnchor.Rotate(_thirdPersonRotationAnchor.up, rotateInput * _properties.TurretRotateSpeed * Time.deltaTime);

        switch (CamMode)
        {
            case CameraMode.FirstPerson:
            {
                Vector3 lerpedPosition = Vector3.Slerp(_currentCameraTransform.position, _firstPersonPivot.position, FIRST_PERSON_DIST_DAMP * Time.deltaTime);
                _currentCameraTransform.position = lerpedPosition;

                _currentCameraTransform.LookAt(_firstPersonCameraFocus.position);
                break;
            }
            case CameraMode.ThirdPerson:
            {
                Vector3 slerpedPosition =
                    Vector3.Slerp(_currentCameraTransform.position, _thirdPersonTargetPos.position, THIRD_PERSON_DIST_DAMP * Time.deltaTime);
                _currentCameraTransform.position = slerpedPosition;
                _currentCameraTransform.LookAt(_thirdPersonCameraFocus.position);
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_thirdPersonCameraFocus.position, 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_thirdPersonTargetPos.position, 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_thirdPersonRotationAnchor.position, 0.2f);

        if (_currentCameraTransform != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_currentCameraTransform.position, 0.2f);
        }
    }

    public void OffsetCameraOnCannonTilt(float mouseTiltInput)
    {
        //Inverts the delta for the camera. Cannon moves up, camera moves down.
        _offsetOnTilt.y -= mouseTiltInput * _properties.TurretTiltSpeed * _cameraDampOnCannonTilt * Time.deltaTime;
        _offsetOnTilt.y = Mathf.Clamp(_offsetOnTilt.y, _camLowestY, _camHighestY);
    }
}