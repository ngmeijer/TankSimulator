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

public struct MovementData
{
    public float Velocity;
    public int GearIndex;
    public int RPM;
}

public class MoveComponent : TankComponent
{
    private const int REAR_DRIVE_GEAR = -1;
    private const int MIN_RPM = 750;
    private const int FINAL_DRIVE_RATIO = 14;
    
    private MoveDirection _moveDirection;
    private float _hudCurrentUpdateTime;
    private float _hudMaxUpdateTime = 0.2f;
    private float _maxSpeed;
    private MovementData _movementData;
    private Vector2 _textureOffsetLefTrack;
    private Vector2 _textureOffsetRightTrack;
    private int _wheelCount;
    private float _currentTorque;
    
    [SerializeField] private Rigidbody _tankRB;
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private MeshRenderer _leftTrackRenderer;    
    [SerializeField] private MeshRenderer _rightTrackRenderer;

    public List<WheelCollider> LeftTrackWheelColliders = new List<WheelCollider>();  
    public List<WheelCollider> RightTrackWheelColliders = new List<WheelCollider>();

    private void Start()
    {
        _componentManager.EventManager.OnShellFired.AddListener((content) => TankKickbackOnShellFire());
        _tankRB.centerOfMass = _centerOfMass.localPosition;
        _tankRB.mass = _properties.TankMass;

        _movementData = new MovementData();
        _wheelCount = LeftTrackWheelColliders.Count + RightTrackWheelColliders.Count;
    }

    private void OnValidate()
    {
        Debug.Assert(_tankRB != null, $"Tank rigidbody reference in MoveComponent ({gameObject.name}) is null. Drag into the inspector");
        Debug.Assert(_centerOfMass != null, $"Center of mass reference in MoveComponent ({gameObject.name}) is null. Drag into the inspector");
        Debug.Assert(_leftTrackRenderer != null, $"Left track renderer reference in MoveComponent ({gameObject.name}) is null. Drag into the inspector");
        Debug.Assert(_rightTrackRenderer != null, $"Right track renderer reference in MoveComponent ({gameObject.name}) is null. Drag into the inspector");
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

    public void UpdateHUD()
    {
        _hudCurrentUpdateTime += Time.deltaTime;
        if (_hudCurrentUpdateTime >= _hudMaxUpdateTime)
        {
            _hudCurrentUpdateTime = 0;
            HUDManager.Instance.UpdateGearboxData(_movementData);
        }
    }

    public float GetTankVelocity()
    {
        return _tankRB.velocity.magnitude;
    }

    private void AnimateTankTracks(float inputValueLeftTrack, float inputValueRightTrack)
    {
        var offsetLeft = Time.time * inputValueLeftTrack * GetTankVelocity();
        var offsetRight = Time.time * inputValueRightTrack * GetTankVelocity();
        _textureOffsetLefTrack.y = offsetLeft;
        _textureOffsetRightTrack.y = offsetRight;
        _leftTrackRenderer.material.mainTextureOffset = _textureOffsetLefTrack;
        _rightTrackRenderer.material.mainTextureOffset = _textureOffsetRightTrack;
    }

    private void SetMotorTorque(float leftTrackTorque, float rightTrackTorque)
    {
        SetTorqueForTracks(LeftTrackWheelColliders, leftTrackTorque);
        SetTorqueForTracks(RightTrackWheelColliders, rightTrackTorque);
    }

    private void SetTorqueForTracks(List<WheelCollider> listToUpdate, float torqueNewtonMeters)
    {
        foreach (WheelCollider wheel in listToUpdate)
        {
            wheel.motorTorque = torqueNewtonMeters;
        }
    }

    public void MoveTank(float inputValue)
    {
        AnimateTankTracks(inputValue, inputValue);
        //For the AI, input value can be generated as well so should be a reusable class.
        float torquePerWheel = CalculateTorque(inputValue);
        SetMotorTorque(torquePerWheel, torquePerWheel);
        _movementData.Velocity = (float)Math.Round(GetTankVelocity(), 1);
    }

    public void RotateTank(float rotateInputValue)
    {
        float torquePerWheel = CalculateTorque(rotateInputValue);

        switch (rotateInputValue)
        {
            //Rotating to left
            case < 0:
                AnimateTankTracks(-rotateInputValue, rotateInputValue);
                SetMotorTorque(-torquePerWheel * _properties.SingleTrackTorqueMultiplier, torquePerWheel * _properties.SingleTrackTorqueMultiplier);
                break;
            //Rotating to right
            case > 0:
                AnimateTankTracks(rotateInputValue, -rotateInputValue);
                SetMotorTorque(torquePerWheel * _properties.SingleTrackTorqueMultiplier, -torquePerWheel * _properties.SingleTrackTorqueMultiplier);
                break;
        }
    }

    public void SlowTankDown()
    {
        SetMotorTorque(0f,0f);
    }

    private void TankKickbackOnShellFire()
    {
        _tankRB.AddForce(0,0, -_componentManager.GetCurrentBarrelDirection().z * _properties.KickbackForce,ForceMode.Impulse);
    }
    
    public List<WheelCollider> GetLeftWheelColliders() => LeftTrackWheelColliders;       
                                                                                      
    public List<WheelCollider> GetRightWheelColliders() => RightTrackWheelColliders;

    private float CalculateRPM()
    {
        float totalRPM = GetTotalRPM();
        float wheelRPM = totalRPM / _wheelCount;
        _movementData.RPM = (int)totalRPM;
        float gearValue = _properties.GearRatios.Evaluate(_movementData.GearIndex);
        float motorRPM = MIN_RPM + (Mathf.Abs(wheelRPM) * FINAL_DRIVE_RATIO * gearValue);
        
        return motorRPM;
    }

    private float GetTotalRPM()
    {
        float totalRPM = 0;
        
        foreach (var wheel in RightTrackWheelColliders)
        {
            totalRPM += Mathf.Abs(wheel.rpm);
        }

        foreach (var wheel in LeftTrackWheelColliders)
        {
            totalRPM += Mathf.Abs(wheel.rpm);
        }

        return totalRPM;
    }

    private float CalculateTorque(float inputValue)
    {
        _movementData.RPM = (int)CalculateRPM();

        if (inputValue == 0) return 0;
        
        //Prevents moving forward when in rear gear
        if (_movementData.GearIndex < 0)
            inputValue = Mathf.Abs(inputValue);
        //Prevents moving backwards when in any gear above 0
        else if (_movementData.GearIndex > 0 && inputValue < 0)
            inputValue = Mathf.Abs(inputValue);

        _movementData.RPM = Mathf.Abs(_movementData.RPM);
        _currentTorque = _properties.MotorTorque.Evaluate(_movementData.RPM) * _properties.GearRatios.Evaluate(_movementData.GearIndex) * FINAL_DRIVE_RATIO * inputValue;
        float torquePerWheel = _currentTorque / _wheelCount;

        return torquePerWheel;
    }

    public void IncreaseGear()
    {
        _movementData.GearIndex++;
        if (_movementData.GearIndex > _properties.MaxGears)
            _movementData.GearIndex = _properties.MaxGears;
    }

    public void DecreaseGear()
    {
        _movementData.GearIndex--;
        if (_movementData.GearIndex < REAR_DRIVE_GEAR)
            _movementData.GearIndex = REAR_DRIVE_GEAR;
    }
}