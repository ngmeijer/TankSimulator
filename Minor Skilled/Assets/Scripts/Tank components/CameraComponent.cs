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

public class CameraComponent : MonoBehaviour
{
    [Header("ADS properties")] 
    [SerializeField] private Camera _adsCam;

    [Header("First person properties")] 
    [SerializeField] private Camera _firstPersonCam;

    [Header("Third person properties")] 
    [SerializeField] private Camera _thirdPersonCam;
    [SerializeField] private Transform _thirdPersonPivot;
    [SerializeField] private float _thirdPersonCamOffsetY;

    private TankComponentManager _componentManager;

    private Camera _currentCamera;
    private Transform _currentCameraPivot;
    private CameraMode _camMode;
    private CameraMode _previousCamMode;

    private void Awake()
    {
        _componentManager = GetComponent<TankComponentManager>();
    }

    private void Start()
    {
        EnableFirstPerson();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        CheckCameraSwitch();
        HandleCameraTransform();
        OffsetCameraOnCannonTilt();
    }

    public void RightMBADSActivate()
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

    private void HandleCameraTransform()
    {
        Vector3 barrelEuler = _componentManager.GetBarrelEuler();
        if (_camMode == CameraMode.ThirdPerson)
        {
            _currentCamera.transform.localPosition =
                _thirdPersonPivot.localPosition + new Vector3(0, _thirdPersonCamOffsetY, 0);
            _currentCamera.transform.localEulerAngles = new Vector3(barrelEuler.x, 0, 0);
        }
    }

    private void OffsetCameraOnCannonTilt()
    {
        float yRotateInput = Input.GetAxis("Mouse Y");
        
        //Inverts the delta for the camera. Cannon moves up, camera moves down.
        _thirdPersonCamOffsetY -= yRotateInput * _componentManager.Properties.TurretTiltSpeed * 0.1f * Time.deltaTime;
        _thirdPersonCamOffsetY = Mathf.Clamp(_thirdPersonCamOffsetY, -2.5f, 2.5f);
    }
}