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
    
    private PlayerInputActions _inputActions;
    protected bool _canMove;

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

        ZoomInspectView(_scrollInput.y);

        if (!_canMove) return;
        
        GetInputValues();
        RotateAroundTank();
        MoveVertically();
    }

    public override void LateUpdateState()
    {
        Vector3 newPos = ViewCam.transform.position + _posDelta;
        newPos.y = Mathf.Clamp(newPos.y, _innerBorder.position.y, _outerBorder.position.y);
        ViewCam.transform.position = newPos;
    }

    protected override void GetInputValues()
    {
        _scrollInput = _inputActions.Tankinspection.Zoom.ReadValue<Vector2>();
        _mouseInput = _inputActions.Tankinspection.InspectTank.ReadValue<Vector2>();
    }

    private void EnableRotate(InputAction.CallbackContext cb)
    {
        _canMove = true;
    }

    private void DisableRotate(InputAction.CallbackContext cb)
    {
        _canMove = false;
    }

    private void RotateAroundTank()
    {
        ViewCam.transform.LookAt(StateLookAt);
        StateLookAt.eulerAngles += new Vector3(0, _mouseInput.x * _horizontalSpeed * Time.deltaTime, 0);
    }

    private void MoveVertically()
    {
        Vector3 yDelta = transform.up * (_mouseInput.y * _verticalSpeed * Time.deltaTime);
        _posDelta += yDelta;
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