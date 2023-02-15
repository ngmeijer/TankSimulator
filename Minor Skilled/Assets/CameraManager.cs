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

    [SerializeField] private float _rotateSpeed = 60f;

    private Camera _currentCamera;
    private CameraMode _camMode;
    [SerializeField] private Transform _turretTransform;
    [SerializeField] private Transform _barrelTransform;
    [SerializeField] private float _highestCannonY = 15f;
    [SerializeField] private float _lowestCannonY = -45f;
    
    private void Start()
    {
        _camMode = CameraMode.FirstPerson;
        OnFirstPerson();
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
            OnADS();
            _camMode = CameraMode.ADS;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnFirstPerson();
            _camMode = CameraMode.FirstPerson;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnThirdPerson();
            _camMode = CameraMode.ThirdPerson;
        }
    }

    private void OnADS()
    {
        _currentCamera = _adsCam;
        _adsCam.gameObject.SetActive(true);
        _firstPersonCam.gameObject.SetActive(false);
        _thirdPersonCam.gameObject.SetActive(false);
    }

    private void OnFirstPerson()
    {
        _currentCamera = _firstPersonCam;
        _firstPersonCam.gameObject.SetActive(true);
        _adsCam.gameObject.SetActive(false);
        _thirdPersonCam.gameObject.SetActive(false);
    }

    private void OnThirdPerson()
    {
        _currentCamera = _thirdPersonCam;
        _thirdPersonCam.gameObject.SetActive(true);
        _firstPersonCam.gameObject.SetActive(false);
        _adsCam.gameObject.SetActive(false);
    }

    private void HandleCameraTransform()
    {
        float xRotateInput = Input.GetAxis("Mouse X");
        float yRotateInput = Input.GetAxis("Mouse Y");

        switch (_camMode)
        {
            case CameraMode.ADS:
                _currentCamera.transform.position = _adsPivot.position + _ADSCameraOffset;
                _currentCamera.transform.rotation = _adsPivot.parent.parent.rotation;
                _turretTransform.Rotate(Vector3.up * (xRotateInput * (_rotateSpeed * Time.deltaTime)));

                Quaternion quatRot = _barrelTransform.rotation;
                Vector3 barrelRotation = quatRot.eulerAngles;
                barrelRotation.x += yRotateInput * (_rotateSpeed * Time.deltaTime);
                float clampedX = Mathf.Clamp(barrelRotation.x, _highestCannonY, _lowestCannonY);
                quatRot.eulerAngles = barrelRotation;
                _barrelTransform.rotation = quatRot;
                break;
            case CameraMode.FirstPerson:
                _currentCamera.transform.position = _firstPersonPivot.position + _firstPersonCameraOffset;
                _firstPersonPivot.parent.Rotate(Vector3.up * (xRotateInput * (_rotateSpeed * Time.deltaTime)));
                _currentCamera.transform.rotation = _firstPersonPivot.parent.rotation;
                break;
            case CameraMode.ThirdPerson:
                _turretTransform.Rotate(Vector3.up * (xRotateInput * (_rotateSpeed * Time.deltaTime)));
                // _currentCamera.transform.RotateAround(transform.position, _currentCamera.transform.up,
                //     (xRotateInput * (_rotateSpeed * Time.deltaTime)));
                break;
        }
    }
}