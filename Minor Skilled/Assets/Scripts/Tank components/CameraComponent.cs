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
    private const float THIRD_PERSON_DIST_DAMP = 10f;

    [Header("ADS properties")] [SerializeField]
    private Camera _adsCam;

    [Header("Third person properties")] [SerializeField]
    private Camera _thirdPersonCam;

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
        EnableThirdPerson();
        Cursor.lockState = CursorLockMode.Locked;
        _thirdPersonCam.transform.position = _thirdPersonTargetPos.position;
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
        _currentCameraTransform = currentCamera.transform;
        _adsCam.gameObject.SetActive(adsCamState);
        _thirdPersonCam.gameObject.SetActive(tpCamState);
    }

    public void UpdateCameraPosition(float rotateInput)
    {
        _thirdPersonRotationAnchor.Rotate(_thirdPersonRotationAnchor.up,
            rotateInput * _properties.TurretRotateSpeed * Time.deltaTime);

        Vector3 positionDelta = Vector3.zero;
        Vector3 slerpedPosition = Vector3.zero;

        if (CamMode == CameraMode.ThirdPerson)
        {
            slerpedPosition =
                Vector3.Slerp(_currentCameraTransform.position, _thirdPersonTargetPos.position,
                    THIRD_PERSON_DIST_DAMP * Time.deltaTime);
            positionDelta = slerpedPosition - _currentCameraTransform.position;
            _currentCameraTransform.LookAt(_thirdPersonCameraFocus.position);
        }

        _currentCameraTransform.position += positionDelta;
    }

    public void ShakeCamera()
    {
        float shakeIntensity = 50;
        float shakeDuration = 0.42f;
        float dropOffTime = 1.6f;
        LTDescr shakeTweenVertical = LeanTween
            .rotateAroundLocal(_currentCameraTransform.gameObject, Vector3.right, shakeIntensity, shakeDuration)
            .setEase(LeanTweenType.easeShake)
            .setLoopClamp();

        LTDescr shakeTweenHorizontal = LeanTween
            .rotateAroundLocal(_currentCameraTransform.gameObject, Vector3.up, shakeIntensity, shakeDuration)
            .setEase(LeanTweenType.easeShake)
            .setLoopClamp();

        LeanTween.value(_currentCameraTransform.gameObject, shakeIntensity, 0f, dropOffTime).setOnUpdate(
            (float val) => { shakeTweenVertical.setTo(Vector3.right * val); }
        ).setEase(LeanTweenType.easeOutQuad);

        LeanTween.value(_currentCameraTransform.gameObject, shakeIntensity, 0f, dropOffTime).setOnUpdate(
            (float val) => { shakeTweenHorizontal.setTo(Vector3.right * val); }
        ).setEase(LeanTweenType.easeOutQuad);
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