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
    [SerializeField] private float thirdPersonCamOffsetY;

    private TankComponentManager _componentManager;
    private MoveComponent _processor;

    private Camera _currentCamera;
    private Transform _currentCameraPivot;
    private CameraMode _camMode;

    private void Awake()
    {
        _componentManager = GetComponent<TankComponentManager>();
        _processor = GetComponent<MoveComponent>();
    }

    private void Start()
    {
        EnableFirstPerson();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CheckCameraSwitch();
        HandleCameraTransform();
        OffsetCameraOnCannonTilt();
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
                cameraEuler = new Vector3(_componentManager.BarrelEulerAngles.x / 1.5f, 0,0);
                _currentCamera.transform.localEulerAngles = cameraEuler;
                break;
            case CameraMode.ThirdPerson:
                _currentCamera.transform.localPosition = _thirdPersonPivot.localPosition + new Vector3(0, thirdPersonCamOffsetY, 0); 
                cameraEuler = new Vector3(_componentManager.BarrelEulerAngles.x / 2, 0,_currentCamera.transform.localEulerAngles.z);
                _currentCamera.transform.localEulerAngles = cameraEuler;
                break;
        }
    }

    private void OffsetCameraOnCannonTilt()
    {
        float yRotateInput = Input.GetAxis("Mouse Y");
        
        //Inverts the delta for the camera. Cannon moves up, camera moves down.
        thirdPersonCamOffsetY -= yRotateInput * _componentManager.Properties.TurretTiltSpeed * 0.1f * Time.deltaTime;
        thirdPersonCamOffsetY = Mathf.Clamp(thirdPersonCamOffsetY, -2.5f, 2.5f);
    }
}