using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum MoveDirection
{
    Forward,
    Backward
};

public class MoveComponent : MonoBehaviour
{
    private TankComponentManager _componentManager; 
    
    private float currentAcceleration;
    private float currentBrakeTorque;

    [SerializeField] private bool _lockTurret;
    [SerializeField] private float barrelMinY;
    [SerializeField] private float barrelMaxY;
    [SerializeField] private float kickbackForce = 9000f;
    private MoveDirection _moveDirection;

    private void Awake()
    {
        _componentManager = GetComponent<TankComponentManager>();
    }

    private void Update()
    {
        AnimateTankTracks(_componentManager.TankRB.velocity.magnitude);
        HandleTurretRotation();
        OffsetCannonRotationOnTankRotation();
    }
    
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.yellow;
        
        Handles.Label(transform.position + new Vector3(0, 2, 0), $"Velocity: {_componentManager.TankRB.velocity.magnitude}");

        foreach (var wheel in _componentManager.LeftTrackWheelColliders)
        {
            Gizmos.DrawSphere(wheel.transform.position, 0.2f);
            Handles.Label(wheel.transform.position - new Vector3(0, 0.25f, 0), $"Motor torque: {wheel.motorTorque}");
        }
        
        foreach (var wheel in _componentManager.RightTrackWheelColliders)
        {
            Gizmos.DrawSphere(wheel.transform.position, 0.2f);
            Handles.Label(wheel.transform.position - new Vector3(0, 0.25f, 0), $"Motor torque: {wheel.motorTorque}");
        }
    }

    public bool IsTankMoving()
    {
        return _componentManager.TankRB.velocity.magnitude > 0;
    }

    public void MultiplyVelocity(float multiplier)
    {
        _componentManager.TankRB.velocity *= multiplier;
    }

    private void AnimateTankTracks(float speed)
    {
        var offset = Time.time * speed;
        _componentManager.LeftTrackRenderer.material.mainTextureOffset = new Vector2(0, offset);
        _componentManager.RightTrackRenderer.material.mainTextureOffset = new Vector2(0, offset);
    }

    public void MoveForward(float inputValue)
    {
        if (_moveDirection != MoveDirection.Forward)
        {
            _moveDirection = MoveDirection.Forward;
        }

        MoveTank(inputValue);
    }

    public void MoveBackward(float inputValue)
    {
        if (_moveDirection != MoveDirection.Backward)
        {
            _moveDirection = MoveDirection.Backward;
            SetMotorTorque(0f, 0f);
        }

        MoveTank(inputValue);
    }

    public void SetMotorTorque(float leftTrackTorque, float rightTrackTorque)
    {
        foreach (var collider in _componentManager.LeftTrackWheelColliders)
        {
            collider.motorTorque = leftTrackTorque;
        }
            
        foreach (var collider in _componentManager.RightTrackWheelColliders)
        {
            collider.motorTorque = rightTrackTorque;
        }
    }

    public void RotateTank(float rotateInputValue, float moveInputValue)
    {
        if (rotateInputValue == 0) return;
        
        if (CheckIfAtMaxSpeed()) 
            return;
        
        currentAcceleration = _componentManager.Properties.SingleTrackSpeed * rotateInputValue;

        if (moveInputValue < 0)
            currentAcceleration *= -1;
        SetMotorTorque(currentAcceleration, 0f);
    }

    private void MoveTank(float inputValue)
    {
        if (CheckIfAtMaxSpeed()) return;
        
        currentAcceleration = _componentManager.Properties.Acceleration * inputValue;
        SetMotorTorque(currentAcceleration, currentAcceleration);
    }

    private bool CheckIfAtMaxSpeed()
    {
        if (_componentManager.TankRB.velocity.magnitude >= _componentManager.Properties.MaxSpeed)
        {
            _componentManager.TankRB.velocity *= 0.95f;
            return true;
        };

        return false;
    }
    
    private void HandleTurretRotation()
    {
        float xRotateInput = Input.GetAxis("Mouse X");

        _componentManager.TurretEulerAngles += new Vector3(0, xRotateInput, 0) * Time.deltaTime * _componentManager.Properties.TurretRotateSpeed;
    }

    private void OffsetCannonRotationOnTankRotation()
    {
        float moveInput = Input.GetAxis("Vertical");
        float hullRotateInput = Input.GetAxis("Horizontal");
        float xRotateInput = Input.GetAxis("Mouse X");

        if (moveInput == 0 && hullRotateInput == 0) return;

        float offsetHullRotation = hullRotateInput * _componentManager.Properties.HullRotateSpeed;
        float turretRotation = xRotateInput * _componentManager.Properties.TurretRotateSpeed;
        _componentManager.TurretEulerAngles += new Vector3(0, turretRotation - offsetHullRotation) * Time.deltaTime;
    }

    public void TiltCannon(float inputValue)
    {
        //Move cannon up and down
        _componentManager.BarrelEulerAngles -= new Vector3(inputValue, 0, 0) * Time.deltaTime * _componentManager.Properties.TurretTiltSpeed;
        _componentManager.BarrelEulerAngles.x = Mathf.Clamp(_componentManager.BarrelEulerAngles.x, barrelMaxY, barrelMinY);
    }

    public void SlowTankDown()
    {
        SetMotorTorque(0f,0f);
        MultiplyVelocity(0.95f);
    }

    public void TankKickback()
    {
        _componentManager.TankRB.AddForce(0,0, -_componentManager.GetCurrentBarrelDirection().z * kickbackForce,ForceMode.Impulse);
    }
}