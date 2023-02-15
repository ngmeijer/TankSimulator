using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private float _maxSpeed = 20f;
    [SerializeField] private float _rotateSpeed = 15f;
    private float _currentSpeed;
    
    private void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float rotateInput = Input.GetAxis("Horizontal");
        
        if (forwardInput != 0)
            MoveForward(forwardInput);
        else _currentSpeed = 0;

        if (rotateInput != 0)
            RotateBody(rotateInput);
    }

    private void RotateBody(float pInputValue)
    {
        transform.Rotate(Vector3.up * (_rotateSpeed * pInputValue * Time.deltaTime));
    }

    private void MoveForward(float pInputValue)
    {
        _currentSpeed = _rb.velocity.magnitude;
        if (_currentSpeed < _maxSpeed)
            _currentSpeed += _acceleration;
        float force = _currentSpeed * pInputValue * Time.deltaTime;

        _rb.AddForce(transform.forward * force, ForceMode.Acceleration);
    }
}
