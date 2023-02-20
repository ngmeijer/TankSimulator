using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MoveDirection
{
    Forward,
    Backward
};

public class MovementManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private TankProperties _properties;

    [SerializeField] private MeshRenderer _leftTrackRenderer;
    [SerializeField] private MeshRenderer _rightTrackRenderer;
    [SerializeField] private List<WheelCollider> _leftTrackWheelColliders = new List<WheelCollider>();
    [SerializeField] private List<WheelCollider> _rightTrackWheelColliders = new List<WheelCollider>();

    private float currentAcceleration;

    [SerializeField] private Transform _turretTransform;
    [SerializeField] private Transform _hullTransform;
    
    private MoveDirection _moveDirection;

    private void Update()
    {
        float moveInput = Input.GetAxis("Vertical");
        float rotateInput = Input.GetAxis("Horizontal");
        
        if (moveInput > 0)
            MoveForward(moveInput);

        if (moveInput < 0)
            MoveBackward(moveInput);

        RotateTank(rotateInput);
        TurretFollowHullRotation();
    }

    private void AnimateTankTracks(float leftTrackSpeed, float rightTrackSpeed)
    {
        Debug.Log("animating tank tracks");
        var offsetLeft = Time.time * leftTrackSpeed;
        var offsetRight = Time.time * rightTrackSpeed;
        _leftTrackRenderer.material.mainTextureOffset = new Vector2(0, offsetLeft);
        _rightTrackRenderer.material.mainTextureOffset = new Vector2(0, offsetRight);
    }

    private void MoveForward(float inputValue)
    {
        if (_moveDirection != MoveDirection.Forward)
        {
            _moveDirection = MoveDirection.Forward;
            SetBrakeTorque(0f, 0f);
        }

        MoveTank(inputValue);
        AnimateTankTracks(currentAcceleration, currentAcceleration);
    }

    private void MoveBackward(float inputValue)
    {
        if (_moveDirection != MoveDirection.Backward)
        {
            _moveDirection = MoveDirection.Backward;
            SetMotorTorque(0f, 0f);
            
            // if(_rb.velocity.magnitude > 0)
            //     SetBrakeTorque(_properties.BrakeTorque, _properties.BrakeTorque);
        }

        MoveTank(inputValue);
        AnimateTankTracks(currentAcceleration, currentAcceleration);
    }

    private void TurretFollowHullRotation()
    {
        Quaternion turretRotQuat = _turretTransform.rotation;
        Vector3 turretRotEuler = turretRotQuat.eulerAngles;
        Vector3 hullRot = _hullTransform.rotation.eulerAngles;
        turretRotQuat.eulerAngles = new Vector3(hullRot.x, turretRotEuler.y, hullRot.z);
        _turretTransform.rotation = turretRotQuat;
    }

    private void SetMotorTorque(float leftTrackTorque, float rightTrackTorque)
    {
        foreach (var collider in _leftTrackWheelColliders)
        {
            collider.motorTorque = leftTrackTorque;
        }
            
        foreach (var collider in _rightTrackWheelColliders)
        {
            collider.motorTorque = rightTrackTorque;
        }
    }

    private void SetBrakeTorque(float leftTrackBrakeTorque, float rightTrackBrakeTorque)
    {
        foreach (var collider in _leftTrackWheelColliders)
        {
            collider.brakeTorque = leftTrackBrakeTorque;
        }
            
        foreach (var collider in _rightTrackWheelColliders)
        {
            collider.brakeTorque = rightTrackBrakeTorque;
        }
    }

    private void RotateTank(float rotateInput)
    {
        //Rotating to the right
        if (rotateInput > 0)
        {
            currentAcceleration = _properties.LeftTrackSpeed * rotateInput;
            AnimateTankTracks(currentAcceleration, 0f);
            SetMotorTorque(currentAcceleration, 0f);
        }
        
        //Rotating to the left
        if (rotateInput < 0)
        {
            currentAcceleration = _properties.RightTrackSpeed * -rotateInput;
            AnimateTankTracks(0f, currentAcceleration);
            SetMotorTorque(0f, currentAcceleration);
        }
    }

    private void MoveTank(float inputValue)
    {
        if (_rb.velocity.magnitude > _properties.MaxSpeed) return;
        currentAcceleration = _properties.Acceleration * inputValue;
        Debug.Log(currentAcceleration);
        SetMotorTorque(currentAcceleration, currentAcceleration);
    }
}