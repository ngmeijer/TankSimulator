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
    private MoveComponent moveComponent;
    private ShootComponent shootComponent;
    
    private void Start()
    {
        moveComponent = GetComponent<MoveComponent>();
        shootComponent = GetComponent<ShootComponent>();
    }

    private void Update()
    {
        if (moveComponent != null)
            TankTransformation();

        if (shootComponent != null)
        {
            TankFire();
            shootComponent.TrackDistance();
        }
    }

    private void TankTransformation()
    {
        moveInput = Input.GetAxis("Vertical");
        rotateInput = Input.GetAxis("Horizontal");
        tiltInput = Input.GetAxis("Mouse Y");
        
        if (moveInput > 0 && rotateInput == 0)
            moveComponent.MoveForward(moveInput);

        if (moveInput < 0 && rotateInput == 0)
            moveComponent.MoveBackward(moveInput);

        moveComponent.RotateTank(rotateInput, moveInput);
        moveComponent.TiltCannon(tiltInput);
    }

    private void TankFire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shootComponent.FireShell();
            moveComponent.TankKickback();
        }
    }

    private void FixedUpdate()
    {
        if (moveInput == 0 && rotateInput == 0 && moveComponent.IsTankMoving())
        {
            moveComponent.SlowTankDown();
        }
    }
}
