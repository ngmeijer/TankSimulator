using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuCameraAnimations : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    private Vector3 _camStartRot;
    [SerializeField] private float _sensitivity = 10f;
    [SerializeField] private float _slerpSpeed = 1f;
    private float _slerpProgress;
    [SerializeField] private Transform _cannonTransform;
    [SerializeField] private Transform _cameraLookat;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Transform _cameraLowerLeftBound;
    [SerializeField] private Transform _cameraUpperRightBound;

    [SerializeField] private ParticleSystem _cannonParticles;

    private void Start()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.MenuInput.MouseMove.performed += MoveLookAt;
        _inputActions.MenuInput.MouseMove.performed += MoveCannon;
        _inputActions.MenuInput.Click.started += FireCannon;
        _inputActions.Enable();
        _camStartRot = transform.eulerAngles;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void LateUpdate()
    {
        transform.LookAt(_cameraLookat);
    }

    private void MoveLookAt(InputAction.CallbackContext cb)
    {
        Vector2 mouseDelta = cb.ReadValue<Vector2>();
        Vector3 targetPos = _targetTransform.position;
        
        //Move targetTransform instantly to target position
        targetPos.z += mouseDelta.x * _sensitivity * Time.deltaTime;
        //targetPos.y += mouseDelta.y * _sensitivity * Time.deltaTime;
        targetPos.z = Mathf.Clamp(targetPos.z, _cameraLowerLeftBound.position.z, _cameraUpperRightBound.position.z);
        _targetTransform.position = targetPos;
        
        //Slerp lookAtTransform to target position
        _cameraLookat.position =
            Vector3.Slerp(_cameraLookat.position, _targetTransform.position, Time.time * _slerpSpeed);
    }

    private void MoveCannon(InputAction.CallbackContext cb)
    {
        Vector2 mouseDelta = cb.ReadValue<Vector2>();
        Vector3 targetRotation = _cannonTransform.eulerAngles;
        targetRotation.x = targetRotation.x += mouseDelta.y * _sensitivity * Time.deltaTime;
        Vector3 slerpedRotation = Vector3.Slerp(_cannonTransform.eulerAngles, targetRotation, _slerpSpeed * Time.time);
        slerpedRotation.x = Mathf.Clamp(slerpedRotation.x, 340f, 359);
        _cannonTransform.eulerAngles = slerpedRotation;
    }

    private void FireCannon(InputAction.CallbackContext cb)
    {
        _cannonParticles.Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_cameraLookat.position, 0.5f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(_targetTransform.position, 0.6f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_cameraLowerLeftBound.position, 0.5f);
        
        Debug.DrawLine(_cameraLowerLeftBound.position, _cameraLookat.position, Color.yellow);
        Gizmos.DrawSphere(_cameraUpperRightBound.position, 0.5f);
        Debug.DrawLine(_cameraUpperRightBound.position, _cameraLookat.position, Color.yellow);
    }
}
