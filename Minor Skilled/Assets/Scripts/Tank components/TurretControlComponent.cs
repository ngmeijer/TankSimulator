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
    [SerializeField] private Transform _barrelTargetDestination;

    private Vector3 _turretEulerAngles;
    private float _currentXRotation;

    private Vector3 _handlesOffset = new Vector3(0.5f, 0.5f, 0);

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
        float minY = _barrelLowerBound.localPosition.y;
        float maxY = _barrelUpperBound.localPosition.y;
        float maxLength = maxY - minY;
        float newY = rangePercent * maxLength;
        float yBarrelClamp = Mathf.Clamp(newY, minY, maxY);

        Vector3 currentPosition = _barrelTargetDestination.transform.localPosition;
        Vector3 newPosition = currentPosition;
        newPosition.y = yBarrelClamp;
        _barrelTargetDestination.transform.localPosition = Vector3.MoveTowards(currentPosition,
            newPosition, _properties.TurretTiltSpeed * Time.deltaTime);
        _barrelTransform.LookAt(_barrelTargetDestination);
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
        Handles.Label(_barrelLowerBound.position + _handlesOffset, _barrelLowerBound.name);
        Gizmos.DrawSphere(_barrelUpperBound.position, 0.25f);
        Handles.Label(_barrelUpperBound.position + _handlesOffset, _barrelUpperBound.name);
        Gizmos.DrawLine(_barrelLowerBound.position, _barrelUpperBound.position);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_barrelTargetDestination.position, 0.2f);
        Handles.Label(_barrelTargetDestination.position + _handlesOffset, _barrelTargetDestination.name);
    }
}