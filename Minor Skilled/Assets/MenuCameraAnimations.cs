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
    [SerializeField] private Transform _cannonTransform;

    private void Start()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.MenuInput.MouseMove.performed += MoveCamera;
        _inputActions.MenuInput.MouseMove.performed += MoveCannon;
        _inputActions.Enable();
        _camStartRot = transform.eulerAngles;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void MoveCamera(InputAction.CallbackContext cb)
    {
        Vector2 mouseDelta = cb.ReadValue<Vector2>();
        Vector3 targetRotation = transform.eulerAngles;

        targetRotation.x += mouseDelta.y * _sensitivity * Time.deltaTime;
        targetRotation.y += mouseDelta.x * _sensitivity * Time.deltaTime;  
        Vector3 slerpedRotation = Vector3.Slerp(transform.eulerAngles, targetRotation, _slerpSpeed * Time.time);
        transform.eulerAngles = slerpedRotation;
    }

    private void MoveCannon(InputAction.CallbackContext cb)
    {
        Vector2 mouseDelta = cb.ReadValue<Vector2>();
        Vector3 targetRotation = _cannonTransform.eulerAngles;
        targetRotation.x = targetRotation.x += mouseDelta.y * _sensitivity * Time.deltaTime;
        Vector3 slerpedRotation = Vector3.Slerp(_cannonTransform.eulerAngles, targetRotation, _slerpSpeed * Time.time);
        Debug.Log(slerpedRotation);
        slerpedRotation.x = Mathf.Clamp(slerpedRotation.x, 340f, 370f);
        _cannonTransform.eulerAngles = slerpedRotation;
    }
}
