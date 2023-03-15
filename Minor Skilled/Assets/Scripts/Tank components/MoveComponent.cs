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
    private const int REAR_DRIVE_GEAR = -1;
    private const int MIN_RPM = 750;
    private const int FINAL_DRIVE_RATIO = 6;
    
    private MoveDirection _moveDirection;
    private float _hudCurrentUpdateTime;
    private float _hudMaxUpdateTime = 0.2f;
    private float _maxSpeed;
    private int _gearIndex = 0;
    private int _wheelCount;
    private float _rpm;
    private float _motorTorque;
    private Vector2 _textureOffset;
    
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
        _wheelCount = LeftTrackWheelColliders.Count + RightTrackWheelColliders.Count;
    }

    private void OnValidate()
    {
        Debug.Assert(_tankRB != null, $"Tank rigidbody reference in MoveComponent ({gameObject.name}) is null. Drag into the inspector");
        Debug.Assert(_centerOfMass != null, $"Center of mass reference in MoveComponent ({gameObject.name}) is null. Drag into the inspector");
        Debug.Assert(_leftTrackRenderer != null, $"Left track renderer reference in MoveComponent ({gameObject.name}) is null. Drag into the inspector");
        Debug.Assert(_rightTrackRenderer != null, $"Right track renderer reference in MoveComponent ({gameObject.name}) is null. Drag into the inspector");
    }

    private void Update()
    {
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
        _textureOffset.y = offset;
        _leftTrackRenderer.material.mainTextureOffset = _textureOffset;
        _rightTrackRenderer.material.mainTextureOffset = _textureOffset;
    }

    private void SetMotorTorque(float leftTrackTorque, float rightTrackTorque)
    {
        SetTorqueForTracks(LeftTrackWheelColliders, leftTrackTorque);
        SetTorqueForTracks(RightTrackWheelColliders, rightTrackTorque);
    }

    private void SetTorqueForTracks(List<WheelCollider> listToUpdate, float torqueValue)
    {
        foreach (WheelCollider wheel in listToUpdate)
        {
            wheel.motorTorque = torqueValue;
        }
    }

    public void MoveTank(float inputValue)
    {
        //For the AI, input value can be generated as well so should be a reusable class.
        float torquePerWheel = CalculateTorque(inputValue);
        SetMotorTorque(torquePerWheel, torquePerWheel);
    }

    public void RotateTank(float rotateInputValue)
    {
        float torquePerWheel = CalculateTorque(rotateInputValue);

        switch (rotateInputValue)
        {
            //Rotating to left
            case < 0:
                SetMotorTorque(0, torquePerWheel * _properties.SingleTrackTorqueMultiplier);
                break;
            //Rotating to right
            case > 0:
                SetMotorTorque(torquePerWheel * _properties.SingleTrackTorqueMultiplier, 0);
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
        float wheelRPM = GetWheelRPM() / _wheelCount;
        float gearValue = _properties.GearRatios.Evaluate(_gearIndex);
        float motorRPM = MIN_RPM + (Mathf.Abs(wheelRPM) * FINAL_DRIVE_RATIO * gearValue);
        
        //Debugging
        //_componentManager.EntityHUD.UpdateWheelRPMCalculation($"motorRPM = [minRPM]{MIN_RPM} + ([wheelRPM]{wheelRPM} * [FDR]{FINAL_DRIVE_RATIO} * [gear value]{gearValue} = {motorRPM})");

        return motorRPM;
    }

    private float GetWheelRPM()
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
        _rpm = CalculateRPM();

        if (inputValue == 0) return 0;
        
        //Prevents moving forward when in rear gear
        if (_gearIndex < 0)
            inputValue = Mathf.Abs(inputValue);
        //Prevents moving backwards when in any gear above 0
        else if (_gearIndex > 0 && inputValue < 0)
            inputValue = Mathf.Abs(inputValue);

        _rpm = Mathf.Abs(_rpm);
        _motorTorque = _properties.MotorTorque.Evaluate(_rpm) * _properties.GearRatios.Evaluate(_gearIndex) * FINAL_DRIVE_RATIO * inputValue;
        // _componentManager.EntityHUD.UpdateCalculationText($"Final torque: [torque graph - rpm {_rpm}]{_properties.MotorTorque.Evaluate(_rpm)} * " +
        //                                                    $"[gear graph]{_properties.GearRatios.Evaluate(_gearIndex)} * [FDR]{FINAL_DRIVE_RATIO} * [input]{inputValue} = {_motorTorque}" +
        //                                                    $"\nTPW: {_motorTorque} / {_wheelCount} = {_motorTorque / _wheelCount}");
        float torquePerWheel = _motorTorque / _wheelCount;

        return torquePerWheel;
    }

    public void IncreaseGear()
    {
        _gearIndex++;
        if (_gearIndex > _properties.MaxGears)
            _gearIndex = _properties.MaxGears;
    }

    public void DecreaseGear()
    {
        _gearIndex--;
        if (_gearIndex < REAR_DRIVE_GEAR)
            _gearIndex = REAR_DRIVE_GEAR;
    }
}