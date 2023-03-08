using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MoveComponent))]
public class PlayerInput : MonoBehaviour
{
    private float moveInput;
    private float rotateInput;
    private float tiltInput;
    private float scrollInput;
    
    private MoveComponent moveComponent;
    private ShootComponent shootComponent;
    private TurretControlComponent turretControlComponent;
    private CameraComponent cameraComponent;

    private TankComponentManager componentManager;

    public GameEvent OnShellFired = new GameEvent();

    private PlayerHUD playerHUD;

    private void Awake()
    {
        moveComponent = GetComponent<MoveComponent>();
        shootComponent = GetComponent<ShootComponent>();
        turretControlComponent = GetComponent<TurretControlComponent>();
        cameraComponent = GetComponent<CameraComponent>();
        componentManager = GetComponent<TankComponentManager>();

        playerHUD = componentManager.entityHUD as PlayerHUD;
    }

    private void Start()
    {
        playerHUD.UpdateAmmoCountUI(shootComponent.CurrentAmmoCount);
        playerHUD.UpdateShellTypeUI(shootComponent.CurrentShellType);
    }

    private void Update()
    {
        if (componentManager.HasDied) return;
        
        if (moveComponent != null)
            TankTransformation();

        if (shootComponent != null)
        {
            TankFire();
            ShellTypeSwitch();
            playerHUD.UpdateDistanceUI(shootComponent.TrackDistance());
        }

        if (turretControlComponent != null)
        {
            float turretYDelta = turretControlComponent.TiltCannon(tiltInput);
            //componentManager.HUDManager.UpdateCrosshairYPosition(turretYDelta);
            playerHUD.SetTurretRotationUI(componentManager.TurretEulerAngles);
        }

        if (cameraComponent != null)
        {
            CheckZoom();
        }
    }

    private void FixedUpdate()
    {
        if (moveInput == 0 && rotateInput == 0 && moveComponent.IsTankMoving())
        {
            moveComponent.SlowTankDown();
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
        
        playerHUD.UpdateTankSpeedUI((float)Math.Round(componentManager.TankRB.velocity.magnitude, 2));
    }

    private void TankFire()
    {
        if (Input.GetMouseButtonDown(0) && shootComponent.CanFire && shootComponent.HasAmmo())
        {
            componentManager.eventManager.OnShellFired?.Invoke("Shell fired. Reloading!");
            shootComponent.FireShell();
            playerHUD.UpdateAmmoCountUI(shootComponent.CurrentAmmoCount);
            moveComponent.TankKickback();
            if (shootComponent.CurrentAmmoCount > 0)
                StartCoroutine(playerHUD.UpdateReloadUI(componentManager.Properties.ReloadTime));
        }
    }
    
    private void ShellTypeSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            shootComponent.SwitchShell();
            playerHUD.UpdateShellTypeUI(shootComponent.CurrentShellType);
        }
    }
    
    private void CheckZoom()
    {
        if (Input.GetMouseButtonDown(1))
        {
            cameraComponent.RightMBADSActivate();
        }
        scrollInput = Input.GetAxis("Mouse ScrollWheel");
        //cameraComponent.ZoomADSCamera(scrollInput);
    }
}
