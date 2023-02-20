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
    [SerializeField] private Transform _adsPivot;

    [Header("First person properties")] 
    [SerializeField] private Camera _firstPersonCam;
    [SerializeField] private Transform _firstPersonPivot;

    [Header("Third person properties")] 
    [SerializeField] private Camera _thirdPersonCam;
    [SerializeField] private Transform _thirdPersonPivot;
    [SerializeField] private Transform _thirdPersonLookAt;
    
    [Header("Tank components")]
    [SerializeField] private Transform _turretTransform;
    [SerializeField] private Transform _barrelTransform;
    [SerializeField] private TankProperties _properties;
    
    private Camera _currentCamera;
    private CameraMode _camMode;
    
    private void Start()
    {
        EnableFirstPerson();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CheckCameraSwitch();
        HandleCameraTransform();
        // Quaternion turretQuat = _turretTransform.rotation;
        // turretQuat.eulerAngles = new Vector3(0, turretQuat.eulerAngles.y, 0);
        // _turretTransform.rotation = turretQuat;
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
        float xRotateInput = Input.GetAxis("Mouse X");

        switch (_camMode)
        {
            case CameraMode.ADS:
                _currentCamera.transform.position = _adsPivot.position;
                _currentCamera.transform.rotation = _adsPivot.parent.parent.rotation;
                _turretTransform.Rotate(_turretTransform.up * (xRotateInput * (_properties.TurretRotateSpeed * Time.deltaTime)));
                break;
            case CameraMode.FirstPerson:
                _currentCamera.transform.position = _firstPersonPivot.position;
                _firstPersonPivot.parent.Rotate(_firstPersonPivot.parent.transform.up * (xRotateInput * (_properties.TurretRotateSpeed * Time.deltaTime)));
                _currentCamera.transform.rotation = _firstPersonPivot.parent.rotation;
                break;
            case CameraMode.ThirdPerson:
                _currentCamera.transform.position = _thirdPersonPivot.position;
                _currentCamera.transform.RotateAround(transform.position, Vector3.up,
                    (xRotateInput * (_properties.TurretRotateSpeed * Time.deltaTime)));
                _currentCamera.transform.LookAt(_thirdPersonLookAt);
                _turretTransform.Rotate(_turretTransform.up * (xRotateInput * (_properties.TurretRotateSpeed * Time.deltaTime)));
                break;
        }

        OffsetCannonRotationOnMove(xRotateInput);
        TiltCannon();
    }

    private void OffsetCannonRotationOnMove(float xRotateInputValue)
    {
        float moveInput = Input.GetAxis("Vertical");
        float hullRotateInput = Input.GetAxis("Horizontal");

        if (moveInput == 0) return;

        float offsetHullRotation = hullRotateInput * _properties.HullRotateSpeed;
        float turretRotation = xRotateInputValue * _properties.TurretRotateSpeed;
        _turretTransform.Rotate(_turretTransform.up * ((turretRotation - offsetHullRotation) * Time.deltaTime));
    }

    private void TiltCannon()
    {
        float yRotateInput = Input.GetAxis("Mouse Y");

        Quaternion rotQuat = _barrelTransform.rotation;
        Vector3 euler = rotQuat.eulerAngles;
        
        euler.x -= yRotateInput * _properties.TurretRotateSpeed * Time.deltaTime;
        euler.x = Mathf.Clamp(euler.x, 305f, 359f);
        rotQuat.eulerAngles = euler;
        _barrelTransform.rotation = rotQuat;
    }
}