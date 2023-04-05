using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;


public class InspectorCamState : CameraState
{
    [SerializeField] private Transform _inspectModePivot;
    [SerializeField] private Transform _rotationTarget;
    [SerializeField] private float _horizontalSpeed = 50f;
    [SerializeField] private float _verticalSpeed = 25f;
    [SerializeField] private float _zoomSpeed = 50f;
    [SerializeField] private Transform _innerBorder;
    [SerializeField] private Transform _outerBorder;
    private float _scrollInput;
    private float _mouseHorizontalInput;
    private float _mouseVerticalInput;
    private float _zoomLevel;
    private float _zoomPosition;

    private bool _inTransition;
    private Vector3 _posDelta;
    private Vector3 _tempCamRot;
    private Vector3 _tempCamPos;

    public override void EnterState()
    {
        base.EnterState();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        _tempCamRot = ViewCam.transform.eulerAngles;

        StateLookAt = _inspectModePivot;
    }

    public override void UpdateState()
    {
        _posDelta = Vector3.zero;
        _tempCamPos = ViewCam.transform.position;

        GameManager.Instance.InspectCameraPosition = ViewCam.transform.position;
        GetInputValues();
        RotateAroundTank(_mouseHorizontalInput);
        MoveVertically(_mouseVerticalInput);
        ZoomInspectView(_scrollInput);
        
        _tempCamPos += _posDelta;
        _tempCamPos.y = Mathf.Clamp(_tempCamPos.y, _innerBorder.position.y, _outerBorder.position.y);
        _tempCamPos.z = Mathf.Clamp(_tempCamPos.z, _innerBorder.position.z, _outerBorder.position.z);
    }

    public override void LateUpdateState()
    {
        if (!_inTransition)
        {
            //ViewCam.transform.position = _tempCamPos;
            //ViewCam.transform.eulerAngles = _tempCamRot;
        }
    }
    
    protected override void GetInputValues()
    {
        _scrollInput = Input.GetAxis("Mouse ScrollWheel");
        _mouseHorizontalInput = Input.GetAxis("Mouse X");
        _mouseVerticalInput = Input.GetAxis("Mouse Y");
    }

    private void RotateAroundTank(float xMouseInput)
    {
        ViewCam.transform.LookAt(StateLookAt);

        if (Input.GetMouseButton(1))
        {
            _rotationTarget.eulerAngles += new Vector3(0, xMouseInput * _horizontalSpeed * Time.deltaTime, 0);
        }
    }

    private void MoveVertically(float yMouseInput)
    {
        if (Input.GetMouseButton(1))
        {
            float yDelta = yMouseInput * _verticalSpeed * Time.deltaTime;
            _posDelta.y -= yDelta;
        }
    }

    private void ZoomInspectView(float scrollInput)
    {
        Vector3 zoomDelta = transform.forward * (scrollInput * _zoomSpeed * Time.deltaTime);
        _posDelta += zoomDelta;
    }

    //OnClick function
    public void LerpToClickedCanvas(TankPart data)
    {
        _inTransition = true;

        // ViewCam.transform.DOLookAt(data.RotationTarget.position, 1f).OnComplete(() =>
        // {
        //     _tempCamRot = ViewCam.transform.eulerAngles;
        // });
        // ViewCam.transform.DOMove(data.PositionTarget.position, 1f).OnComplete(() =>
        // {
        //     _inTransition = false;
        //     //_posDelta = ViewCam.transform.position;
        // });
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