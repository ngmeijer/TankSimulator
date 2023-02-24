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
    private TurretControlComponent turretControlComponent;
    
    private void Start()
    {
        moveComponent = GetComponent<MoveComponent>();
        shootComponent = GetComponent<ShootComponent>();
        turretControlComponent = GetComponent<TurretControlComponent>();
    }

    private void Update()
    {
        if (moveComponent != null)
            TankTransformation();

        if (shootComponent != null)
        {
            TankFire();
        }

        if (turretControlComponent != null)
        {
            turretControlComponent.TiltCannon(tiltInput);
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

        if(rotateInput < 0 || rotateInput > 0)
            moveComponent.RotateTank(rotateInput, moveInput);
    }

    private void TankFire()
    {
        if (Input.GetMouseButtonDown(0) && shootComponent.readyToFire)
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
