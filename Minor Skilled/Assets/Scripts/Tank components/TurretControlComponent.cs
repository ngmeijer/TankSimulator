using System;
using UnityEngine;

public class TurretControlComponent : TankComponent
{
    [SerializeField] private bool _lockTurret;
    [SerializeField] private float barrelMinY;
    [SerializeField] private float barrelMaxY;
    private Vector3 currentLerpEuler;
    
    private Vector3 _barrelEulerAngles;      
    private Vector3 _turretEulerAngles;    
    private Transform _turretTransform;       
    private Transform _barrelTransform;    
    
    private void Update()
    {
        HandleTurretRotation();
        OffsetCannonRotationOnTankRotation();
    }
    
    private void LateUpdate()                                        
    {                                                                
        _barrelTransform.localEulerAngles  = _barrelEulerAngles;        
        _turretTransform.localEulerAngles = _turretEulerAngles;        
    }                                                                

    private void HandleTurretRotation()
    {
        float xRotateInput = Input.GetAxis("Mouse X");

        _turretEulerAngles += new Vector3(0, xRotateInput, 0) * (Time.deltaTime * _componentManager.Properties.TurretRotateSpeed);
    }

    private void OffsetCannonRotationOnTankRotation()
    {
        float moveInput = Input.GetAxis("Vertical");
        float hullRotateInput = Input.GetAxis("Horizontal");
        float xRotateInput = Input.GetAxis("Mouse X");

        if (moveInput == 0 && hullRotateInput == 0) return;

        float offsetHullRotation = hullRotateInput * _componentManager.Properties.HullRotateSpeed;
        float turretRotation = xRotateInput * _componentManager.Properties.TurretRotateSpeed;
        _turretEulerAngles += new Vector3(0, turretRotation - offsetHullRotation) * Time.deltaTime;
    }

    public float TiltCannon(float inputValue)
    {
        //Move cannon up and down
        float delta = inputValue * Time.deltaTime * _componentManager.Properties.TurretTiltSpeed;
        _barrelEulerAngles -= new Vector3(delta, 0, 0);
        _barrelEulerAngles.x =
            Mathf.Clamp(_barrelEulerAngles.x, barrelMaxY, barrelMinY);

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