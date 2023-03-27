using System;
using UnityEngine;

public class TurretControlComponent : TankComponent
{
    [SerializeField] private Transform _turretTransform;
    [SerializeField] private Transform _rotationTargetParent;
    [SerializeField] private Transform _tpRotationTarget;
    [SerializeField] private Transform _barrelTransform;
    [SerializeField] private bool _lockTurret;
    [SerializeField] private Transform _barrelLookat;
    [SerializeField] private Transform _barrelLowerY;
    [SerializeField] private Transform _barrelUpperY;
    [SerializeField] private Transform _barrelTargetTransform;
    private float _barrelLookatY;
    
    private Vector3 _barrelEulerAngles;
    private Vector3 _turretEulerAngles;
    private float _currentXRotation;

    private void Start()
    {
        Debug.Assert(_turretTransform != null, $"TurretTransform on '{gameObject.name}' is null.");
        Debug.Assert(_barrelTransform != null, $"BarrelTransform on '{gameObject.name}' is null.");
        _barrelEulerAngles = _barrelTransform.localEulerAngles;
    }

    public void HandleTurretRotation(float rotateInput)
    {
        _rotationTargetParent.Rotate(_rotationTargetParent.up,
            rotateInput * _properties.TurretRotateSpeed * Time.deltaTime);
        _turretTransform.rotation =
            Quaternion.RotateTowards(_turretTransform.rotation, _rotationTargetParent.rotation,
                _properties.TurretRotateSpeed * Time.deltaTime);
    }

    public void OffsetCannonRotationOnTankRotation(float hullRotateInput, float turretRotateInput)
    {
        float offsetHullRotation = hullRotateInput * _componentManager.Properties.HullRotateSpeed;
        float turretRotation = turretRotateInput * _componentManager.Properties.TurretRotateSpeed;
        _turretEulerAngles += new Vector3(0, turretRotation - offsetHullRotation) * Time.deltaTime;
    }

    public void OffsetCannonOnRangeChange(float rangePercent)
    {
        float minY = _barrelLowerY.localPosition.y;
        float maxY = _barrelUpperY.localPosition.y;
        float maxLength = maxY - minY;
        float newY = rangePercent * maxLength;
        float yBarrelClamp = Mathf.Clamp(newY, minY, maxY);

        Vector3 currentPosition = _barrelLookat.transform.localPosition;
        Vector3 newPosition = currentPosition;
        newPosition.y = yBarrelClamp;
        _barrelTargetTransform.localPosition = newPosition;
        _barrelLookat.transform.localPosition = Vector3.MoveTowards(currentPosition, _barrelTargetTransform.localPosition, _properties.TurretTiltSpeed * Time.deltaTime);
        _barrelTransform.LookAt(_barrelLookat);
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
        Gizmos.DrawSphere(_barrelLowerY.position, 0.25f);
        Gizmos.DrawSphere(_barrelUpperY.position, 0.25f);
        
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(_barrelLookat.position, 0.15f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_barrelTargetTransform.position, 0.2f);
    }
}