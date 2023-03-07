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
        _componentManager.TankRB.velocity =
            Vector3.ClampMagnitude(_componentManager.TankRB.velocity, _componentManager.Properties.MaxSpeed);
        AnimateTankTracks(_componentManager.TankRB.velocity.magnitude);
        _componentManager.HUDManager.UpdateTankSpeedUI((float)Math.Round(_componentManager.TankRB.velocity.magnitude, 2));
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
        if (CheckIfAtMaxSpeed()) 
            return;

        if (moveInputValue < 0)
        {
            if(rotateInputValue < 0)
                currentAcceleration = _componentManager.Properties.SingleTrackSpeed * rotateInputValue;
            else if (rotateInputValue > 0)
                currentAcceleration = -(_componentManager.Properties.SingleTrackSpeed * rotateInputValue);
        }
        else
        {
            currentAcceleration = Mathf.Abs(_componentManager.Properties.SingleTrackSpeed * rotateInputValue);
        }
        
        //Rotating to left
        if (rotateInputValue < 0)
            SetMotorTorque(0, currentAcceleration);
        
        //Rotating to right
        if(rotateInputValue > 0)
            SetMotorTorque(currentAcceleration, 0);
    }

    private void MoveTank(float inputValue)
    {
        if (CheckIfAtMaxSpeed()) return;
        
        if(inputValue > 0)
            currentAcceleration = _componentManager.Properties.Acceleration * inputValue;
        if(inputValue < 0)
            currentAcceleration = _componentManager.Properties.ReverseAcceleration * inputValue;
        SetMotorTorque(currentAcceleration, currentAcceleration);
    }

    private bool CheckIfAtMaxSpeed()
    {
        return _componentManager.TankRB.velocity.magnitude >= _componentManager.Properties.MaxSpeed;
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