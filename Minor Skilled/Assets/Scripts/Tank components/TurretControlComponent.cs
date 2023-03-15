using System;
using UnityEngine;

public class TurretControlComponent : TankComponent
{
    [SerializeField] private Transform _turretTransform;       
    [SerializeField] private Transform _barrelTransform;
    [SerializeField] private bool _lockTurret;
    [SerializeField] private float _barrelMinY;
    [SerializeField] private float _barrelMaxY;
    
    private Vector3 _barrelEulerAngles;      
    private Vector3 _turretEulerAngles;

    private void Start()
    {
        Debug.Assert(_turretTransform != null, $"TurretTransform on '{gameObject.name}' is null.");
        Debug.Assert(_barrelTransform != null, $"BarrelTransform on '{gameObject.name}' is null.");
    }

    private void LateUpdate()                                        
    {                                                                
        _barrelTransform.localEulerAngles  = _barrelEulerAngles;        
        _turretTransform.localEulerAngles = _turretEulerAngles;        
    }                                                                

    public void HandleTurretRotation(float turretRotationInput)
    {
        _turretEulerAngles += new Vector3(0, turretRotationInput, 0) * (Time.deltaTime * _componentManager.Properties.TurretRotateSpeed);
    }

    public void OffsetCannonRotationOnTankRotation(float hullRotateInput, float turretRotateInput)
    {
        float offsetHullRotation = hullRotateInput * _componentManager.Properties.HullRotateSpeed;
        float turretRotation = turretRotateInput * _componentManager.Properties.TurretRotateSpeed;
        _turretEulerAngles += new Vector3(0, turretRotation - offsetHullRotation) * Time.deltaTime;
    }

    public float TiltCannon(float inputValue)
    {
        //Move cannon up and down
        float delta = inputValue * Time.deltaTime * _componentManager.Properties.TurretTiltSpeed;
        _barrelEulerAngles -= new Vector3(delta, 0, 0);
        _barrelEulerAngles.x =
            Mathf.Clamp(_barrelEulerAngles.x, _barrelMaxY, _barrelMinY);

        return delta;
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