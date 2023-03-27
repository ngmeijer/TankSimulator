using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    ADS,
    ThirdPerson
}

public class CameraComponent : TankComponent
{
    private const float THIRD_PERSON_DIST_DAMP = 60f;

    [Header("ADS properties")] [SerializeField]
    private Camera _adsCam;
    [SerializeField] private Transform _adsTargetPos;

    [Header("Third person properties")] [SerializeField]
    private Camera _thirdPersonCam;

    [SerializeField] private Transform _thirdPersonTargetPos;
    [SerializeField] private Transform _thirdPersonOppositeTargetPos;
    [SerializeField] private Transform _thirdPersonCameraFocus;
    [SerializeField] private Transform _thirdPersonRotationAnchor;
    [SerializeField] private float _camLowestY = -7;
    [SerializeField] private float _camHighestY = 3;
    [SerializeField] private Transform _barrelTargetPosition;

    private Vector3 _offsetOnTilt;
    private Camera _currentCamera;
    public CameraMode CamMode { get; private set; }
    private CameraMode _previousCamMode;
    private float _lastMoveValue;
    private Vector3 _lastTankPosition;
    private float _cameraDampOnCannonTilt = 0.1f;
    private int _currentCameraFOVLevel = 0;
    [SerializeField] private int _maxCameraZoomLevel = 3;

    private int[] _FovRanges = new int[4]
    {
        60,
        20,
        10,
        5
    };

    private void Start()
    {
        EnableThirdPerson();
        Cursor.lockState = CursorLockMode.Locked;
        _thirdPersonCam.transform.position = _thirdPersonTargetPos.position;
    }

    private void LateUpdate()
    {
        GameManager.Instance.RotationCrosshairPosition = GetScreenpointRotationCrosshair();
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
            EnableThirdPerson();
        }
    }

    private void CheckCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EnableADS();
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            EnableThirdPerson();
    }

    private Vector3 GetScreenpointRotationCrosshair()
    {
        switch (CamMode)
        {
            case CameraMode.ADS:
                return _currentCamera.WorldToScreenPoint(_adsTargetPos.position);
            case CameraMode.ThirdPerson:
                Vector3 posToConvert = new Vector3(_thirdPersonOppositeTargetPos.position.x, _barrelTargetPosition.position.y,
                    _thirdPersonOppositeTargetPos.position.z);
                return _currentCamera.WorldToScreenPoint(posToConvert);
        }

        return Vector3.zero;
    }

    private void EnableADS()
    {
        CamMode = CameraMode.ADS;
        ChangeCameraPerspective(_adsCam, true, false);
    }

    private void EnableThirdPerson()
    {
        CamMode = CameraMode.ThirdPerson;
        ChangeCameraPerspective(_thirdPersonCam, false, true);
    }

    private void ChangeCameraPerspective(Camera currentCamera, bool adsCamState, bool tpCamState)
    {
        _componentManager.EventManager.OnCameraChanged.Invoke(CamMode);
        _currentCamera = currentCamera;
        _adsCam.gameObject.SetActive(adsCamState);
        _thirdPersonCam.gameObject.SetActive(tpCamState);
    }

    public void UpdateCameraPosition()
    {
        Vector3 positionDelta = Vector3.zero;
        Vector3 slerpedPosition = Vector3.zero;

        if (CamMode == CameraMode.ThirdPerson)
        {
            slerpedPosition =
                Vector3.Slerp(_currentCamera.transform.position, _thirdPersonTargetPos.position + _offsetOnTilt,
                    THIRD_PERSON_DIST_DAMP * Time.deltaTime);
            positionDelta = slerpedPosition - _currentCamera.transform.position;
            _currentCamera.transform.position += positionDelta;
            _currentCamera.transform.LookAt(_thirdPersonCameraFocus.position);
        }
    }

    public void ShakeCamera()
    {
        float shakeIntensity = 50;
        float shakeDuration = 0.42f;
        float dropOffTime = 1.6f;
        LTDescr shakeTweenVertical = LeanTween
            .rotateAroundLocal(_currentCamera.gameObject, Vector3.right, shakeIntensity, shakeDuration)
            .setEase(LeanTweenType.easeShake)
            .setLoopClamp();

        LTDescr shakeTweenHorizontal = LeanTween
            .rotateAroundLocal(_currentCamera.gameObject, Vector3.up, shakeIntensity, shakeDuration)
            .setEase(LeanTweenType.easeShake)
            .setLoopClamp();

        LeanTween.value(_currentCamera.gameObject, shakeIntensity, 0f, dropOffTime).setOnUpdate(
            (float val) => { shakeTweenVertical.setTo(Vector3.right * val); }
        ).setEase(LeanTweenType.easeOutQuad);

        LeanTween.value(_currentCamera.gameObject, shakeIntensity, 0f, dropOffTime).setOnUpdate(
            (float val) => { shakeTweenHorizontal.setTo(Vector3.right * val); }
        ).setEase(LeanTweenType.easeOutQuad);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_thirdPersonCameraFocus.position, 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_thirdPersonTargetPos.position, 0.2f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(_thirdPersonOppositeTargetPos.position, 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_thirdPersonRotationAnchor.position, 0.2f);

        if (_currentCamera != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_currentCamera.transform.position, 0.2f);
        }
    }

    public void ZoomADS()
    {
        if (CamMode != CameraMode.ADS) return;

        if (_currentCameraFOVLevel >= _maxCameraZoomLevel)
            _currentCameraFOVLevel = 0;
        else _currentCameraFOVLevel++;

        float newFOV = _FovRanges[_currentCameraFOVLevel];
        _adsCam.fieldOfView = newFOV;
    }
}