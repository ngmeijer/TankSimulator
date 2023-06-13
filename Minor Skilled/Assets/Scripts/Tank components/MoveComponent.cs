using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
    private const int FINAL_DRIVE_RATIO = 1;
    
    private MoveDirection _moveDirection;
    private float _hudCurrentUpdateTime;
    private float _hudMaxUpdateTime = 0.2f;
    private float _maxSpeed;
    private MovementData _movementData;
    private Vector2 _textureOffsetLefTrack;
    private Vector2 _textureOffsetRightTrack;
    private int _wheelCount;
    private float _currentTorque;
    private HUDCombatState _hudCombatState;
    
    [SerializeField] private Rigidbody _tankRB;
    
    [Header("Center of Mass (CoM)")]
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private Transform _leftBound;
    [SerializeField] private Transform _rightBound;
    [SerializeField] private Transform _frontBound;
    [SerializeField] private Transform _backBound;
    
    [SerializeField] private MeshRenderer _leftTrackRenderer;    
    [SerializeField] private MeshRenderer _rightTrackRenderer;

    public List<WheelCollider> LeftTrackWheelColliders = new List<WheelCollider>();  
    public List<WheelCollider> RightTrackWheelColliders = new List<WheelCollider>();
    public List<WheelCollider> GetLeftWheelColliders() => LeftTrackWheelColliders;
    public List<WheelCollider> GetRightWheelColliders() => RightTrackWheelColliders;
    public float GetTankVelocity() => _tankRB.velocity.magnitude;
    
    private void Start()
    {
        _hudCombatState = HUDStateSwitcher.Instance.HUDCombatState as HUDCombatState;
        
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

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_centerOfMass.position, 0.25f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_frontBound.position, 0.3f);
        Gizmos.DrawLine(_frontBound.position, _backBound.position);
        Gizmos.DrawSphere(_backBound.position, 0.3f);
        Gizmos.DrawSphere(_leftBound.position, 0.3f);
        Gizmos.DrawLine(_leftBound.position, _rightBound.position);
        Gizmos.DrawSphere(_rightBound.position, 0.3f);
    }

    public void UpdateHUD()
    {
        _hudCurrentUpdateTime += Time.deltaTime;
        if (_hudCurrentUpdateTime >= _hudMaxUpdateTime)
        {
            _hudCurrentUpdateTime = 0;
            _hudCombatState.UpdateGearboxData(_movementData);
        }
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

    public void MoveForward(float inputValue)
    {
        _movementData.Velocity = (float)Math.Round(GetTankVelocity(), 1);
        AnimateTankTracks(inputValue, inputValue);

        if (inputValue == 0)
            return;

        float torquePerWheel = CalculateTorque(inputValue);
        Debug.Log("Setting torque for MoveForward");
        SetTorqueForTracks(LeftTrackWheelColliders, torquePerWheel);
        SetTorqueForTracks(RightTrackWheelColliders, torquePerWheel);
    }

    public void RotateTank(float rotateInputValue)
    {
        float torquePerWheel = CalculateTorque(rotateInputValue);
        torquePerWheel /= 2;

        if (rotateInputValue == 0)
            return;

        switch (rotateInputValue)
        {
            //Rotating to left
            case < 0:
                AnimateTankTracks(0, rotateInputValue);
                SetTorqueForTracks(LeftTrackWheelColliders, -torquePerWheel * (_properties.SingleTrackTorqueMultiplier / 2));
                SetTorqueForTracks(RightTrackWheelColliders, torquePerWheel * _properties.SingleTrackTorqueMultiplier);
                break;
            //Rotating to right
            case > 0:
                AnimateTankTracks(rotateInputValue, 0);
                SetTorqueForTracks(LeftTrackWheelColliders, torquePerWheel * _properties.SingleTrackTorqueMultiplier);
                SetTorqueForTracks(RightTrackWheelColliders, -torquePerWheel * (_properties.SingleTrackTorqueMultiplier / 2));
                break;
        }
    }

    private void TankKickbackOnShellFire()
    {
        _tankRB.AddForce(0,0, -_componentManager.GetCurrentBarrelDirection().z * _properties.KickbackForce,ForceMode.Impulse);
    }

    private void SetTorqueForTracks(List<WheelCollider> listToUpdate, float torqueNewtonMeters)
    {
        foreach (WheelCollider wheel in listToUpdate)
        {
            wheel.motorTorque = torqueNewtonMeters;
        }
    }

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
        
        if (_movementData.GearIndex is > 0 or < 0 && inputValue < 0)
            inputValue = Mathf.Abs(inputValue);
        
        inputValue = Mathf.Abs(inputValue);
        _movementData.RPM = Mathf.Abs(_movementData.RPM);
        _currentTorque = _properties.MotorTorque.Evaluate(_movementData.RPM) * _properties.GearRatios.Evaluate(_movementData.GearIndex) * FINAL_DRIVE_RATIO * inputValue;
        float torquePerWheel = _currentTorque / _wheelCount;

        return torquePerWheel;
    }

    private int elapsedFrames = 0;
    private const int maxFrames = 60;
    
    public void ChangeCenterOfMass(Vector2 moveDir)
    {
        Vector3 newCoMPosition = _componentManager.EntityOrigin.localPosition;

        elapsedFrames++;
        if (elapsedFrames >= maxFrames && moveDir == Vector2.zero)
            elapsedFrames = 0;
        
        float interpolationRatio = (float)elapsedFrames / maxFrames;

        if (moveDir.x < 0)
            newCoMPosition = _leftBound.localPosition;
        else if (moveDir.x > 0)
            newCoMPosition = _rightBound.localPosition;
        
        if (moveDir.y < 0)
            newCoMPosition = _backBound.localPosition;
        else if (moveDir.y > 0)
            newCoMPosition = _frontBound.localPosition;
            
        Vector3 lerpedPos = Vector3.Lerp(_centerOfMass.localPosition, newCoMPosition, interpolationRatio);
        _centerOfMass.localPosition = lerpedPos;
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

    public void SlowTankDown()
    {
        SetTorqueForTracks(LeftTrackWheelColliders, 0);
        SetTorqueForTracks(RightTrackWheelColliders, 0);
    }
}