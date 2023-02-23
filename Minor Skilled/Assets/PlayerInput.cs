using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveComponent))]
public class PlayerInput : MonoBehaviour
{
    private float moveInput;
    private float rotateInput;
    private float tiltInput;
    private MoveComponent processor;
    
    private void Start()
    {
        processor = GetComponent<MoveComponent>();
    }

    private void Update()
    {
        TankTransformation();
    }

    private void TankTransformation()
    {
        moveInput = Input.GetAxis("Vertical");
        rotateInput = Input.GetAxis("Horizontal");
        tiltInput = Input.GetAxis("Mouse Y");
        
        if (moveInput > 0 && rotateInput == 0)
            processor.MoveForward(moveInput);

        if (moveInput < 0 && rotateInput == 0)
            processor.MoveBackward(moveInput);

        processor.RotateTank(rotateInput, moveInput);
        processor.TiltCannon(tiltInput);
    }

    private void FixedUpdate()
    {
        if (moveInput == 0 && rotateInput == 0 && processor.IsTankMoving())
        {
            processor.SlowTankDown();
        }
    }
}
