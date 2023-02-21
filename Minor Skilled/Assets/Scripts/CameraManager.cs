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
    private Transform _currentCameraPivot;
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
        _currentCameraPivot = _adsPivot;
        _adsCam.gameObject.SetActive(true);
        _firstPersonCam.gameObject.SetActive(false);
        _thirdPersonCam.gameObject.SetActive(false);
    }

    private void EnableFirstPerson()
    {
        _camMode = CameraMode.FirstPerson;

        _currentCamera = _firstPersonCam;
        _currentCameraPivot = _firstPersonPivot;
        _firstPersonCam.gameObject.SetActive(true);
        _adsCam.gameObject.SetActive(false);
        _thirdPersonCam.gameObject.SetActive(false);
    }

    private void EnableThirdPerson()
    {
        _camMode = CameraMode.ThirdPerson;

        _currentCamera = _thirdPersonCam;
        _currentCameraPivot = _thirdPersonPivot;
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
                _currentCamera.transform.rotation = _adsPivot.parent.parent.rotation;
                break;
            case CameraMode.FirstPerson:
                _currentCamera.transform.rotation = _firstPersonPivot.parent.rotation;
                break;
            case CameraMode.ThirdPerson:
                _currentCamera.transform.RotateAround(transform.position, Vector3.up,
                    (xRotateInput * (_properties.TurretRotateSpeed * Time.deltaTime)));
                _currentCamera.transform.LookAt(_thirdPersonLookAt);
                break;
        }

        _currentCamera.transform.position = _currentCameraPivot.position;

        //_turretTransform.localRotation *= Quaternion.Euler(0, _properties.TurretRotateSpeed * Time.deltaTime, 0);

        Quaternion localRot = _turretTransform.localRotation;
        Vector3 localEuler = localRot.eulerAngles;
        localEuler.y += _properties.TurretRotateSpeed * Time.deltaTime;
        localRot.eulerAngles = localEuler;
        //_turretTransform.localRotation = localRot;
        
        //OffsetCannonRotationOnMove(xRotateInput);
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