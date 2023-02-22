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

public class CameraManager : MonoBehaviour
{
    [Header("ADS properties")] 
    [SerializeField] private Camera _adsCam;

    [Header("First person properties")] 
    [SerializeField] private Camera _firstPersonCam;

    [Header("Third person properties")] 
    [SerializeField] private Camera _thirdPersonCam;
    [SerializeField] private Transform _thirdPersonPivot;
    [SerializeField] private float thirdPersonCamOffsetY;

    [Header("Tank components")]
    [SerializeField] private Transform _turretTransform;
    [SerializeField] private Transform _barrelTransform;
    [SerializeField] private TankProperties _properties;
    
    private Camera _currentCamera;
    private Transform _currentCameraPivot;
    private CameraMode _camMode;
    private Vector3 turretCurrentEulerAngles;
    private Vector3 barrelCurrentEulerAngles;
    [SerializeField] private float barrelMinY;
    [SerializeField] private float barrelMaxY;

    private void Start()
    {
        EnableFirstPerson();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CheckCameraSwitch();
        HandleCameraTransform();
        HandleTurretRotation();
        OffsetCannonRotationOnTankRotation();
        TiltCannon();
        _turretTransform.localEulerAngles = turretCurrentEulerAngles;
    }

    private void CheckCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EnableADS();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EnableFirstPerson();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EnableThirdPerson();
        }
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
        Vector3 cameraEuler;
        switch (_camMode)
        {
            case CameraMode.ADS:
                break;
            case CameraMode.FirstPerson:
                cameraEuler = new Vector3(barrelCurrentEulerAngles.x / 1.5f, 0,0);
                _currentCamera.transform.localEulerAngles = cameraEuler;
                break;
            case CameraMode.ThirdPerson:
                _currentCamera.transform.localPosition = _thirdPersonPivot.localPosition + new Vector3(0, thirdPersonCamOffsetY, 0); 
                cameraEuler = new Vector3(barrelCurrentEulerAngles.x / 2, 0,_currentCamera.transform.localEulerAngles.z);
                _currentCamera.transform.localEulerAngles = cameraEuler;
                break;
        }
    }

    private void HandleTurretRotation()
    {
        float xRotateInput = Input.GetAxis("Mouse X");

        turretCurrentEulerAngles += new Vector3(0, xRotateInput, 0) * Time.deltaTime * _properties.TurretRotateSpeed;
    }

    private void OffsetCannonRotationOnTankRotation()
    {
        float moveInput = Input.GetAxis("Vertical");
        float hullRotateInput = Input.GetAxis("Horizontal");
        float xRotateInput = Input.GetAxis("Mouse X");

        if (moveInput == 0 && hullRotateInput == 0) return;

        float offsetHullRotation = hullRotateInput * _properties.HullRotateSpeed;
        float turretRotation = xRotateInput * _properties.TurretRotateSpeed;
        turretCurrentEulerAngles += new Vector3(0, turretRotation - offsetHullRotation) * Time.deltaTime;
    }

    private void TiltCannon()
    {
        float yRotateInput = Input.GetAxis("Mouse Y");

        //Move cannon up and down
        barrelCurrentEulerAngles -= new Vector3(yRotateInput, 0, 0) * Time.deltaTime * _properties.TurretTiltSpeed;
        barrelCurrentEulerAngles.x = Mathf.Clamp(barrelCurrentEulerAngles.x, barrelMaxY, barrelMinY);
        _barrelTransform.localEulerAngles = barrelCurrentEulerAngles;
        
        //Inverts the delta for the camera. Cannon moves up, camera moves down.
        thirdPersonCamOffsetY -= yRotateInput * _properties.TurretTiltSpeed * 0.1f * Time.deltaTime;
        thirdPersonCamOffsetY = Mathf.Clamp(thirdPersonCamOffsetY, -2.5f, 2.5f);
    }
}