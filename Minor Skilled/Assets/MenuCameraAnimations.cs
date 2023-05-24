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
    [SerializeField] private Transform _cameraLowerLeftBound;
    [SerializeField] private Transform _cameraUpperRightBound;

    private void Start()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.MenuInput.MouseMove.performed += MoveLookAt;
        _inputActions.MenuInput.MouseMove.performed += MoveCannon;
        _inputActions.Enable();
        _camStartRot = transform.eulerAngles;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        transform.LookAt(_cameraLookat);
    }

    private void MoveLookAt(InputAction.CallbackContext cb)
    {
        // Vector2 mouseDelta = cb.ReadValue<Vector2>();
        //
        // _slerpProgress += mouseDelta.x * _sensitivity * Time.deltaTime;
        //
        // Vector3 slerpedPosition = Vector3.Slerp(_cameraLowerLeftBound.position, _cameraUpperRightBound.position, _slerpProgress);
        // slerpedPosition.x = Mathf.Clamp(slerpedPosition.x, _cameraLowerLeftBound.position.x, _cameraUpperRightBound.position.x);
        // slerpedPosition.y = Mathf.Clamp(slerpedPosition.y, _cameraLowerLeftBound.position.y, _cameraUpperRightBound.position.y);
        // slerpedPosition.z = Mathf.Clamp(slerpedPosition.y, _cameraLowerLeftBound.position.z, _cameraUpperRightBound.position.z);
        //
        // _cameraLookat.position = slerpedPosition;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_cameraLookat.position, 0.5f);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_cameraLowerLeftBound.position, 0.5f);
        
        Debug.DrawLine(_cameraLowerLeftBound.position, _cameraLookat.position, Color.yellow);
        Gizmos.DrawSphere(_cameraUpperRightBound.position, 0.5f);
        Debug.DrawLine(_cameraUpperRightBound.position, _cameraLookat.position, Color.yellow);
    }
}
