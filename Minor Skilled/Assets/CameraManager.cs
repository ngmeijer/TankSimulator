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
    [Header("ADS properties")] [SerializeField]
    private Camera _adsCam;

    [SerializeField] private Transform _adsPivot;
    [SerializeField] private Vector3 _ADSCameraOffset;

    [Header("First person properties")] [SerializeField]
    private Camera _firstPersonCam;

    [SerializeField] private Transform _firstPersonPivot;
    [SerializeField] private Vector3 _firstPersonCameraOffset;

    [Header("Third person properties")] [SerializeField]
    private Camera _thirdPersonCam;

    [SerializeField] private float _rotateSpeed = 40f;

    private Camera _currentCamera;
    private CameraMode _camMode;
    [SerializeField] private Transform _turretTransform;
    [SerializeField] private Transform _barrelTransform;
    [SerializeField] private TankProperties _properties;
    
    private void Start()
    {
        EnableFirstPerson();
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
        float yRotateInput = Input.GetAxis("Mouse Y");
        float hullRotateInput = Input.GetAxis("Horizontal");

        switch (_camMode)
        {
            case CameraMode.ADS:
                _currentCamera.transform.position = _adsPivot.position + _ADSCameraOffset;
                _currentCamera.transform.rotation = _adsPivot.parent.parent.rotation;
                break;
            case CameraMode.FirstPerson:
                _currentCamera.transform.position = _firstPersonPivot.position + _firstPersonCameraOffset;
                _firstPersonPivot.parent.Rotate(Vector3.up * (xRotateInput * (_rotateSpeed * Time.deltaTime)));
                _currentCamera.transform.rotation = _firstPersonPivot.parent.rotation;
                break;
            case CameraMode.ThirdPerson:
                
                break;
        }

        float offsetHullRotation = hullRotateInput * _properties._rotateSpeed;
        float turretRotation = xRotateInput * _rotateSpeed;
        _turretTransform.Rotate(Vector3.up * ((turretRotation - offsetHullRotation) * Time.deltaTime));
        
        Quaternion rotQuat = _barrelTransform.rotation;
        Vector3 euler = rotQuat.eulerAngles;
        euler.x -= yRotateInput * _rotateSpeed * Time.deltaTime;
        rotQuat.eulerAngles = euler;
        _barrelTransform.rotation = rotQuat;
    }
}