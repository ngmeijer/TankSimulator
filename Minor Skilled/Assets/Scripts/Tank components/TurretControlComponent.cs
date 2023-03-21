using System;
using UnityEngine;

public class TurretControlComponent : TankComponent
{
    [SerializeField] private Transform _turretTransform;
    [SerializeField] private Transform _rotationTarget;
    [SerializeField] private Transform _barrelTransform;
    [SerializeField] private bool _lockTurret;
    [SerializeField] private float _barrelHighestY;
    public float BarrelLowestY = -30;
    
    private Vector3 _barrelEulerAngles;      
    private Vector3 _turretEulerAngles;

    private void Start()
    {
        Debug.Assert(_turretTransform != null, $"TurretTransform on '{gameObject.name}' is null.");
        Debug.Assert(_barrelTransform != null, $"BarrelTransform on '{gameObject.name}' is null.");
    }

    private void LateUpdate()                                        
    {                                                          
        _barrelEulerAngles.x = Mathf.Clamp(_barrelEulerAngles.x, BarrelLowestY, _barrelHighestY);
        _barrelTransform.localEulerAngles = _barrelEulerAngles;
    }

    public void HandleTurretRotation()
    {
        _turretTransform.rotation =
            Quaternion.RotateTowards(_turretTransform.rotation, _rotationTarget.rotation, _properties.TurretRotateSpeed * Time.deltaTime);
    }

    public void OffsetCannonRotationOnTankRotation(float hullRotateInput, float turretRotateInput)
    {
        float offsetHullRotation = hullRotateInput * _componentManager.Properties.HullRotateSpeed;
        float turretRotation = turretRotateInput * _componentManager.Properties.TurretRotateSpeed;
        _turretEulerAngles += new Vector3(0, turretRotation - offsetHullRotation) * Time.deltaTime;
    }

    public void OffsetCannonOnRangeChange(float scrollInput)
    {
        _barrelEulerAngles.x -= scrollInput * _componentManager.Properties.TurretTiltSpeed * Time.deltaTime;
    }

    public Vector3 GetCurrentBarrelDirection()  
    {                                           
        return _barrelTransform.forward;         
    }

    public Vector3 GetBarrelEuler()
    {
        return _barrelTransform.rotation.eulerAngles;
    }
}