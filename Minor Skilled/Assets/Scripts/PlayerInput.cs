using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MoveComponent))]
public class PlayerInput : TankComponent
{
    private float _moveInput;
    private float _hullRotateInput;
    private float _tiltInput;
    private float _scrollInput;
    private float _turretRotateInput;

    private CameraComponent _cameraComponent;

    protected override void Awake()
    {
        base.Awake();

        _cameraComponent = GetComponent<CameraComponent>();
    }

    private void Update()
    {
        if (_componentManager.HasDied) return;

        GetInputValues();
        TankFire();
        ShellTypeSwitch();
        IncreaseGear();
        DecreaseGear();
        //_componentManager.TurretControlComponent.OffsetCannonRotationOnTankRotation(_hullRotateInput, _turretRotateInput);
        _componentManager.TurretControlComponent.HandleTurretRotation(_turretRotateInput);
        _componentManager.TurretControlComponent.TiltCannon(_tiltInput);
        _cameraComponent.OffsetCameraOnCannonTilt(_tiltInput);
        //componentManager.HUDManager.UpdateCrosshairYPosition(turretYDelta);
        //playerHUD.SetTurretRotationUI(componentManager.TurretEulerAngles);
    }

    private void LateUpdate()
    {
        _cameraComponent.UpdateCameraPosition();
    }

    private void FixedUpdate()
    {
        TankMove();

        if (_moveInput == 0 && _hullRotateInput == 0 && _componentManager.MoveComponent.GetTankVelocity() > 0)
        {
            _componentManager.MoveComponent.SlowTankDown();
        }
    }

    private void GetInputValues()
    {
        _moveInput = Input.GetAxis("Vertical");
        _hullRotateInput = Input.GetAxis("Horizontal");
        _tiltInput = Input.GetAxis("Mouse Y");
        _turretRotateInput = Input.GetAxis("Mouse X");
    }

    private void TankMove()
    {
        _componentManager.MoveComponent.MoveTank(_moveInput);

        if (_hullRotateInput < 0 || _hullRotateInput > 0)
            _componentManager.MoveComponent.RotateTank(_hullRotateInput);
    }

    private void TankFire()
    {
        if (Input.GetAxis("Fire1") == 0) return;
        if (!_componentManager.ShootComponent.CanFire) return;

        _componentManager.ShootComponent.FireShell();
    }

    private void ShellTypeSwitch()
    {
        if (!Input.GetKeyDown(KeyCode.Tab)) return;

        _componentManager.ShootComponent.SwitchShell();
    }

    private void IncreaseGear()
    {
        if (!Input.GetKeyDown(KeyCode.LeftShift)) return;

        _componentManager.MoveComponent.IncreaseGear();
    }

    private void DecreaseGear()
    {
        if (!Input.GetKeyDown(KeyCode.LeftAlt)) return;

        _componentManager.MoveComponent.DecreaseGear();
    }
}