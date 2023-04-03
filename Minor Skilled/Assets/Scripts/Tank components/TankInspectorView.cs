using System;
using UnityEditor;
using UnityEngine;

public class TankInspectorView : CameraView
{
    [SerializeField] private Transform _inspectModePivot;
    [SerializeField] private float _inspectSpeed = 25f;
    [SerializeField] private float _zoomSpeed = 20f;
    [SerializeField] private Transform _maxZoomOut;
    [SerializeField] private Transform _maxZoomIn;
    private Transform _camTrans;

    private void Start()
    {
        _camTrans = ViewCam.transform;
    }

    public void RotateAroundTank(float mouseInput)
    {
        ViewCam.transform.LookAt(transform);

        if (Input.GetMouseButton(1))
        {
            _inspectModePivot.rotation *= Quaternion.AngleAxis(mouseInput * _inspectSpeed * Time.deltaTime, _inspectModePivot.up);
        }
    }

    public void ZoomInspectView(float scrollInput)
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_maxZoomOut.position, 0.2f);
        Handles.Label(_maxZoomOut.position + GameManager.HandlesOffset, _maxZoomOut.name);
        
        Gizmos.DrawSphere(_maxZoomIn.position, 0.2f);
        Handles.Label(_maxZoomIn.position + GameManager.HandlesOffset, _maxZoomIn.name);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_maxZoomOut.position, _maxZoomIn.position);
    }
}