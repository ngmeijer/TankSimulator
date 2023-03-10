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

public class MoveComponent : TankComponent
{
    private float _currentAcceleration;
    private float _currentBrakeTorque;
    private Rigidbody _tankRB;
    private MoveDirection _moveDirection;
    
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private MeshRenderer _leftTrackRenderer;    
    [SerializeField] private MeshRenderer _rightTrackRenderer;
    [SerializeField] private float _kickbackForce = 9000f;

    public List<WheelCollider> LeftTrackWheelColliders = new List<WheelCollider>();  
    public List<WheelCollider> RightTrackWheelColliders = new List<WheelCollider>();
    
    protected override void Awake()
    {
        base.Awake();
        _tankRB = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        _componentManager.EventManager.OnShellFired.AddListener((content) => TankKickbackOnShellFire());
        _tankRB.centerOfMass = _centerOfMass.localPosition;
    }

    private void Update()
    {
        _tankRB.velocity =
            Vector3.ClampMagnitude(_tankRB.velocity, _componentManager.Properties.MaxSpeed);
        AnimateTankTracks(GetTankVelocity());
        
        _componentManager.EntityHUD.UpdateSpeed((float)Math.Round(GetTankVelocity(), 2));
    }
    
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.yellow;
        
        Handles.Label(transform.position + new Vector3(0, 2, 0), $"Velocity: {_tankRB.velocity.magnitude}");

        foreach (var wheel in LeftTrackWheelColliders)
        {
            Gizmos.DrawSphere(wheel.transform.position, 0.2f);
            Handles.Label(wheel.transform.position - new Vector3(0, 0.25f, 0), $"Motor torque: {wheel.motorTorque}");
        }
        
        foreach (var wheel in RightTrackWheelColliders)
        {
            Gizmos.DrawSphere(wheel.transform.position, 0.2f);
            Handles.Label(wheel.transform.position - new Vector3(0, 0.25f, 0), $"Motor torque: {wheel.motorTorque}");
        }
    }

    public float GetTankVelocity()
    {
        return _tankRB.velocity.magnitude;
    }

    private void AnimateTankTracks(float speed)
    {
        var offset = Time.time * speed;
        _leftTrackRenderer.material.mainTextureOffset = new Vector2(0, offset);
        _rightTrackRenderer.material.mainTextureOffset = new Vector2(0, offset);
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
        foreach (var collider in LeftTrackWheelColliders)
        {
            collider.motorTorque = leftTrackTorque;
        }
            
        foreach (var collider in RightTrackWheelColliders)
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
                _currentAcceleration = _componentManager.Properties.SingleTrackSpeed * rotateInputValue;
            else if (rotateInputValue > 0)
                _currentAcceleration = -(_componentManager.Properties.SingleTrackSpeed * rotateInputValue);
        }
        else
        {
            _currentAcceleration = Mathf.Abs(_componentManager.Properties.SingleTrackSpeed * rotateInputValue);
        }
        
        //Rotating to left
        if (rotateInputValue < 0)
            SetMotorTorque(0, _currentAcceleration);
        
        //Rotating to right
        if(rotateInputValue > 0)
            SetMotorTorque(_currentAcceleration, 0);
    }

    private void MoveTank(float inputValue)
    {
        if (CheckIfAtMaxSpeed()) return;
        
        if(inputValue > 0)
            _currentAcceleration = _componentManager.Properties.Acceleration * inputValue;
        if(inputValue < 0)
            _currentAcceleration = _componentManager.Properties.ReverseAcceleration * inputValue;
        SetMotorTorque(_currentAcceleration, _currentAcceleration);
    }

    private bool CheckIfAtMaxSpeed()
    {
        return _tankRB.velocity.magnitude >= _componentManager.Properties.MaxSpeed;
    }

    public void SlowTankDown()
    {
        SetMotorTorque(0f,0f);
        //TankRB.AddForce(-transform.forward * _properties.SlowDownForce, ForceMode.VelocityChange);
    }

    private void TankKickbackOnShellFire()
    {
        _tankRB.AddForce(0,0, -_componentManager.GetCurrentBarrelDirection().z * _kickbackForce,ForceMode.Impulse);
    }
    
    public List<WheelCollider> GetLeftWheelColliders() => LeftTrackWheelColliders;       
                                                                                      
    public List<WheelCollider> GetRightWheelColliders() => RightTrackWheelColliders;     
}