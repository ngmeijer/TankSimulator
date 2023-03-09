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
    private float currentAcceleration;
    private float currentBrakeTorque;

    private Rigidbody TankRB;
    [SerializeField] private Transform centerOfMass;
    public List<WheelCollider> LeftTrackWheelColliders = new List<WheelCollider>();  
    public List<WheelCollider> RightTrackWheelColliders = new List<WheelCollider>(); 
    [SerializeField] private MeshRenderer leftTrackRenderer;    
    [SerializeField] private MeshRenderer rightTrackRenderer;   
    [SerializeField] private float kickbackForce = 9000f;
    private MoveDirection _moveDirection;
    
    protected override void Awake()
    {
        base.Awake();
        TankRB = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        componentManager.EventManager.OnShellFired.AddListener((content) => TankKickbackOnShellFire());
        TankRB.centerOfMass = centerOfMass.localPosition;
    }

    private void Update()
    {
        TankRB.velocity =
            Vector3.ClampMagnitude(TankRB.velocity, componentManager.Properties.MaxSpeed);
        AnimateTankTracks(GetTankVelocity());
        
        componentManager.EntityHUD.UpdateSpeed((float)Math.Round(GetTankVelocity(), 2));
    }
    
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.yellow;
        
        Handles.Label(transform.position + new Vector3(0, 2, 0), $"Velocity: {TankRB.velocity.magnitude}");

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
        return TankRB.velocity.magnitude;
    }

    private void MultiplyVelocity(float multiplier)
    {
        TankRB.velocity *= multiplier;
    }

    private void AnimateTankTracks(float speed)
    {
        var offset = Time.time * speed;
        leftTrackRenderer.material.mainTextureOffset = new Vector2(0, offset);
        rightTrackRenderer.material.mainTextureOffset = new Vector2(0, offset);
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
                currentAcceleration = componentManager.Properties.SingleTrackSpeed * rotateInputValue;
            else if (rotateInputValue > 0)
                currentAcceleration = -(componentManager.Properties.SingleTrackSpeed * rotateInputValue);
        }
        else
        {
            currentAcceleration = Mathf.Abs(componentManager.Properties.SingleTrackSpeed * rotateInputValue);
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
            currentAcceleration = componentManager.Properties.Acceleration * inputValue;
        if(inputValue < 0)
            currentAcceleration = componentManager.Properties.ReverseAcceleration * inputValue;
        SetMotorTorque(currentAcceleration, currentAcceleration);
    }

    private bool CheckIfAtMaxSpeed()
    {
        return TankRB.velocity.magnitude >= componentManager.Properties.MaxSpeed;
    }

    public void SlowTankDown()
    {
        SetMotorTorque(0f,0f);
        MultiplyVelocity(0.95f);
    }

    private void TankKickbackOnShellFire()
    {
        TankRB.AddForce(0,0, -componentManager.GetCurrentBarrelDirection().z * kickbackForce,ForceMode.Impulse);
    }
    
    public List<WheelCollider> GetLeftWheelColliders() => LeftTrackWheelColliders;       
                                                                                      
    public List<WheelCollider> GetRightWheelColliders() => RightTrackWheelColliders;     
}