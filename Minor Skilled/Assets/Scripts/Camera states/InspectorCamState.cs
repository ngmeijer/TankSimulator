using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InspectorCamState : CameraState
{
    [SerializeField] private float _horizontalSpeed = 50f;
    [SerializeField] private float _verticalSpeed = 25f;
    [SerializeField] private float _zoomSpeed = 50f;
    [SerializeField] private Transform _innerBorder;
    [SerializeField] private Transform _outerBorder;
    private Vector2 _scrollInput;
    private Vector2 _mouseInput;
    private float _zoomLevel;
    private float _zoomPosition;

    private Vector3 _posDelta;
    private Vector3 _tempCamPos;
    
    private PlayerInputActions _inputActions;
    protected bool _canRotateAround;

    public override void EnterState()
    {
        base.EnterState();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        
        _inputActions = new PlayerInputActions();
        _inputActions.Tankinspection.AllowInspection.started += EnableRotate;
        _inputActions.Tankinspection.AllowInspection.canceled += DisableRotate;
        _inputActions.Tankinspection.Enable();
    }

    public override void UpdateState()
    {
        _posDelta = Vector3.zero;
        _tempCamPos = ViewCam.transform.position;

        GetInputValues();
        RotateAroundTank();
        MoveVertically();
        ZoomInspectView(_scrollInput.y);
        
        _tempCamPos += _posDelta;
        _tempCamPos.y = Mathf.Clamp(_tempCamPos.y, _innerBorder.position.y, _outerBorder.position.y);
        _tempCamPos.z = Mathf.Clamp(_tempCamPos.z, _innerBorder.position.z, _outerBorder.position.z);
    }

    protected override void GetInputValues()
    {
        _scrollInput = _inputActions.Tankinspection.Zoom.ReadValue<Vector2>();
        _mouseInput = _inputActions.Tankinspection.InspectTank.ReadValue<Vector2>();
    }

    private void EnableRotate(InputAction.CallbackContext cb)
    {
        _canRotateAround = true;
    }

    private void DisableRotate(InputAction.CallbackContext cb)
    {
        _canRotateAround = false;
    }

    private void RotateAroundTank()
    {
        ViewCam.transform.LookAt(StateLookAt);

        if (_canRotateAround)
        {
            StateLookAt.eulerAngles += new Vector3(0, _mouseInput.x * _horizontalSpeed * Time.deltaTime, 0);
        }
    }

    private void MoveVertically()
    {
        if (_canRotateAround)
        {
            float yDelta = _mouseInput.y * _verticalSpeed * Time.deltaTime;
            _posDelta.y -= yDelta;
        }
    }

    private void ZoomInspectView(float scrollInput)
    {
        Vector3 zoomDelta = transform.forward * (scrollInput * _zoomSpeed * Time.deltaTime);
        _posDelta += zoomDelta;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_outerBorder.position, 0.2f);
        Handles.Label(_outerBorder.position + GameManager.HandlesOffset, _outerBorder.name);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_outerBorder.position, _innerBorder.position);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_innerBorder.position, 0.2f);
        Handles.Label(_innerBorder.position + GameManager.HandlesOffset, _innerBorder.name);
    }
}