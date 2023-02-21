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

public class MovementManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private TankProperties _properties;

    [SerializeField] private MeshRenderer _leftTrackRenderer;
    [SerializeField] private MeshRenderer _rightTrackRenderer;
    [SerializeField] private List<WheelCollider> _leftTrackWheelColliders = new List<WheelCollider>();
    [SerializeField] private List<WheelCollider> _rightTrackWheelColliders = new List<WheelCollider>();

    private float currentAcceleration;
    private float currentBrakeTorque;

    [SerializeField] private Transform _turretTransform;
    [SerializeField] private bool _lockTurret;
    
    [SerializeField] private Transform _hullTransform;
    [SerializeField] private Transform _centerOfMass;
    
    private MoveDirection _moveDirection;
    private float moveInput;
    private float rotateInput;
    
    private void Start()
    {
        _rb.centerOfMass = _centerOfMass.localPosition;
    }

    private void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        rotateInput = Input.GetAxis("Horizontal");
        
        if (moveInput > 0 && rotateInput == 0)
            MoveForward();

        if (moveInput < 0 && rotateInput == 0)
            MoveBackward();

        if(rotateInput < 0 || rotateInput > 0)
            RotateTank();
        TurretFollowHullRotation();
        AnimateTankTracks(_rb.velocity.magnitude);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        Handles.Label(transform.position + new Vector3(0, 2, 0), $"Velocity: {_rb.velocity.magnitude}");

        foreach (var wheel in _leftTrackWheelColliders)
        {
            Gizmos.DrawSphere(wheel.transform.position, 0.2f);
            Handles.Label(wheel.transform.position - new Vector3(0, 0.25f, 0), $"Motor torque: {wheel.motorTorque}");
        }
        
        foreach (var wheel in _rightTrackWheelColliders)
        {
            Gizmos.DrawSphere(wheel.transform.position, 0.2f);
            Handles.Label(wheel.transform.position - new Vector3(0, 0.25f, 0), $"Motor torque: {wheel.motorTorque}");
        }
    }

    private void FixedUpdate()
    {
        if (moveInput == 0 && rotateInput == 0 && _rb.velocity.magnitude > 0)
        {
            SetMotorTorque(0f,0f);
            _rb.velocity *= 0.95f;
        }
    }

    private void AnimateTankTracks(float speed)
    {
        var offset = Time.time * speed;
        _leftTrackRenderer.material.mainTextureOffset = new Vector2(0, offset);
        _rightTrackRenderer.material.mainTextureOffset = new Vector2(0, offset);
    }

    private void MoveForward()
    {
        if (_moveDirection != MoveDirection.Forward)
        {
            _moveDirection = MoveDirection.Forward;
            //SetBrakeTorque(0f, 0f);
        }

        MoveTank();
    }

    private void MoveBackward()
    {
        if (_moveDirection != MoveDirection.Backward)
        {
            _moveDirection = MoveDirection.Backward;
            SetMotorTorque(0f, 0f);
            
            // if(_rb.velocity.magnitude > 0)
            //     SetBrakeTorque(_properties.BrakeTorque, _properties.BrakeTorque);
        }

        // if (_moveDirection == MoveDirection.Backward && currentBrakeTorque != 0)
            // SetBrakeTorque(0f,0f);

        MoveTank();
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
            //Debug.Log($"RotateInput: {rotateInput}. Torque: {collider.motorTorque}");
            collider.motorTorque = rightTrackTorque;
        }
    }

    private void SetBrakeTorque(float leftTrackBrakeTorque, float rightTrackBrakeTorque)
    {
        currentBrakeTorque = leftTrackBrakeTorque;
        foreach (var collider in _leftTrackWheelColliders)
        {
            collider.brakeTorque = leftTrackBrakeTorque;
        }
            
        foreach (var collider in _rightTrackWheelColliders)
        {
            collider.brakeTorque = rightTrackBrakeTorque;
        }
    }

    private void RotateTank()
    {
        if (CheckIfAtMaxSpeed()) 
            return;
        
        //Rotating to the right
        if (rotateInput > 0)
        {
            currentAcceleration = _properties.SingleTrackSpeed * rotateInput;

            if (moveInput < 0)
                currentAcceleration *= -1;
            SetMotorTorque(currentAcceleration, 0f);
            //SetBrakeTorque(0f, _properties.BrakeTorque);
        }
        
        //Rotating to the left
        if (rotateInput < 0)
        {
            currentAcceleration = _properties.SingleTrackSpeed * -rotateInput;

            if (moveInput < 0)
                currentAcceleration *= -1;
            SetMotorTorque(0f, currentAcceleration);
            //SetBrakeTorque(_properties.BrakeTorque, 0f);
        }
    }

    private void MoveTank()
    {
        if (CheckIfAtMaxSpeed()) return;
        
        currentAcceleration = _properties.Acceleration * moveInput;
        SetMotorTorque(currentAcceleration, currentAcceleration);
    }

    private bool CheckIfAtMaxSpeed()
    {
        if (_rb.velocity.magnitude >= _properties.MaxSpeed)
        {
            _rb.velocity *= 0.95f;
            return true;
        };

        return false;
    }
}