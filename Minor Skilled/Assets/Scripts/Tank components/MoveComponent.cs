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
    private int _gearIndex = 0;
    private int _wheelCount;
    private float _rpm;
    private float _motorTorque;
    
    public List<WheelCollider> LeftTrackWheelColliders = new List<WheelCollider>();  
    public List<WheelCollider> RightTrackWheelColliders = new List<WheelCollider>();

    private float _hudCurrentUpdateTime;
    private float _hudMaxUpdateTime = 0.2f;
    private float _maxSpeed;
    
    protected override void Awake()
    {
        base.Awake();
        _tankRB = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        _componentManager.EventManager.OnShellFired.AddListener((content) => TankKickbackOnShellFire());
        _tankRB.centerOfMass = _centerOfMass.localPosition;
        _tankRB.mass = _properties.TankMass;
        _maxSpeed = _properties.MaxSpeed;
        _wheelCount = LeftTrackWheelColliders.Count + RightTrackWheelColliders.Count;
    }

    private void Update()
    {
        _tankRB.velocity =
            Vector3.ClampMagnitude(_tankRB.velocity, _maxSpeed);
        AnimateTankTracks(GetTankVelocity());

        _hudCurrentUpdateTime += Time.deltaTime;
        if (_hudCurrentUpdateTime >= _hudMaxUpdateTime)
        {
            _hudCurrentUpdateTime = 0;
            _componentManager.EntityHUD.UpdateSpeed((float)Math.Round(GetTankVelocity(), 1));
            _componentManager.EntityHUD.UpdateGearData(_gearIndex, (int)_rpm, (int)_motorTorque);
        }
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

    public void MoveTank(float inputValue)
    {
        if (CheckIfAtMaxSpeed()) return;

        if (_gearIndex == 0) return;

        if (inputValue > 0)
        {
            
        }
        
        // if(inputValue > 0)
        //     _currentAcceleration = _componentManager.Properties.Acceleration * inputValue;
        // if(inputValue < 0)
        //     _currentAcceleration = _componentManager.Properties.ReverseAcceleration * inputValue;
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

    private float CalculateRPM()
    {
        float minRPM = 750f;
        float wheelRPM = GetWheelRPM() / _wheelCount;
        float gearValue = _properties.GearRatios.Evaluate(_gearIndex);
        float motorRPM = minRPM + (wheelRPM * _properties.FinalDriveRatio * gearValue);
        //Debug.Log($"Dynamic RPM: {dynamicRPM}");
        //Debug.Log($"Final drive ratio: {_properties.FinalDriveRatio}");
        //Debug.Log($"Calculation: {minRPM} + ({wheelRPM} * {_properties.FinalDriveRatio} * {gearValue}) = {motorRPM}");

        return motorRPM;
    }

    private float GetWheelRPM()
    {
        float totalRPM = 0;
        
        foreach (var wheel in RightTrackWheelColliders)
        {
            totalRPM += (Mathf.Abs(wheel.rpm));
        }

        foreach (var wheel in LeftTrackWheelColliders)
        {
            totalRPM += (Mathf.Abs(wheel.rpm));
        }

        return totalRPM;
    }

    public void CalculateTorque(float inputValue)
    {
        _rpm = CalculateRPM();
        _motorTorque = _properties.MotorTorque.Evaluate(_rpm) * _properties.GearRatios.Evaluate(_gearIndex) * _properties.FinalDriveRatio * inputValue;
        float torquePerWheel = _motorTorque / _wheelCount;
        
        Debug.Log(torquePerWheel);
        
        SetMotorTorque(torquePerWheel, torquePerWheel);
    }

    public void IncreaseGear()
    {
        _gearIndex++;
        if (_gearIndex > 4)
            _gearIndex = 4;
    }

    public void DecreaseGear()
    {
        _gearIndex--;
        if (_gearIndex < -1)
            _gearIndex = -1;
    }
}