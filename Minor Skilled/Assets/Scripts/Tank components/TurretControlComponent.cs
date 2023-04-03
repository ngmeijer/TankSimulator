using System;
using UnityEditor;
using UnityEngine;

public class TurretControlComponent : TankComponent
{
    [Header("Tank part transforms")] [SerializeField]
    private Transform _turretTransform;

    [SerializeField] private Transform _barrelTransform;
    [SerializeField] private Transform _rotationTargetParent;

    [Header("Barrel rotation")] 
    [SerializeField] private Transform _barrelLowerBound;
    [SerializeField] private Transform _barrelUpperBound;
    [SerializeField] private Transform _barrelLookAt;

    private Vector3 _turretEulerAngles;
    private float _currentXRotation;
    
    private void Start()
    {
        Debug.Assert(_turretTransform != null, $"TurretTransform on '{gameObject.name}' is null.");
        Debug.Assert(_barrelTransform != null, $"BarrelTransform on '{gameObject.name}' is null.");
    }

    public void HandleTurretRotation(float rotateInput)
    {
        Vector3 newRotation = new Vector3(0,
            _rotationTargetParent.localEulerAngles.y + (rotateInput * _properties.TurretRotateSpeed * Time.deltaTime), 0);
        _rotationTargetParent.localEulerAngles = newRotation;
        
        _turretTransform.rotation = Quaternion.RotateTowards(_turretTransform.rotation, _rotationTargetParent.rotation,
                _properties.TurretRotateSpeed * Time.deltaTime);
    }

    public void OffsetCannonOnRangeChange(float rangePercent)
    {
        float minY = _barrelLowerBound.position.y;
        float maxY = _barrelUpperBound.position.y;
        float maxLength = maxY - minY;
        float yDelta = rangePercent * maxLength;

        Vector3 currentPosition = _barrelLookAt.transform.position;
        Vector3 newPosition = currentPosition;
        newPosition.y = minY + yDelta;
        _barrelLookAt.transform.position = Vector3.MoveTowards(currentPosition,
            newPosition, _properties.TurretTiltSpeed * Time.deltaTime);

        Vector3 lookatPosition = new Vector3(_barrelLowerBound.position.x, _barrelLookAt.position.y, _barrelLowerBound.position.z);
        _barrelTransform.LookAt(lookatPosition);
    }

    public Vector3 GetCurrentBarrelDirection()
    {
        return _barrelTransform.forward;
    }

    public Vector3 GetBarrelEuler()
    {
        return _barrelTransform.rotation.eulerAngles;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_barrelLowerBound.position, 0.25f);
        Handles.Label(_barrelLowerBound.position + GameManager.HandlesOffset, _barrelLowerBound.name);
        Gizmos.DrawSphere(_barrelUpperBound.position, 0.25f);
        Handles.Label(_barrelUpperBound.position + GameManager.HandlesOffset, _barrelUpperBound.name);
        Gizmos.DrawLine(_barrelLowerBound.position, _barrelUpperBound.position);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_barrelLookAt.position, 0.2f);
        Handles.Label(_barrelLookAt.position + GameManager.HandlesOffset, _barrelLookAt.name);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(_rotationTargetParent.position,  0.2f);
        Handles.Label(_rotationTargetParent.position + GameManager.HandlesOffset, _rotationTargetParent.name);
    }
}