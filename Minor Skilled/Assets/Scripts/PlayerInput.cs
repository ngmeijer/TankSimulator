using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MoveComponent))]
public class PlayerInput : TankComponent
{
    private float _moveInput;
    private float _rotateInput;
    private float _tiltInput;
    private float _scrollInput;

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
        TankTransformation();
        // CameraTranslation();
        TankFire();
        ShellTypeSwitch();

        float turretYDelta = _componentManager.TurretControlComponent.TiltCannon(_tiltInput);
        //componentManager.HUDManager.UpdateCrosshairYPosition(turretYDelta);
        //playerHUD.SetTurretRotationUI(componentManager.TurretEulerAngles);
    }

    private void LateUpdate()
    {
        CameraTranslation();
    }

    private void FixedUpdate()
    {
        if (_moveInput == 0 && _rotateInput == 0 && _componentManager.MoveComponent.GetTankVelocity() > 0)
        {
            _componentManager.MoveComponent.SlowTankDown();
        }
    }

    private void GetInputValues()
    {
        _moveInput = Input.GetAxis("Vertical");
        _rotateInput = Input.GetAxis("Horizontal");
        _tiltInput = Input.GetAxis("Mouse Y");
    }

    private void CameraTranslation()
    {
        _cameraComponent.UpdateCameraPosition(_moveInput);
    }

    private void TankTransformation()
    {
        _componentManager.MoveComponent.MoveTank(_moveInput);

        if (_rotateInput < 0 || _rotateInput > 0)
            _componentManager.MoveComponent.RotateTank(_rotateInput, _moveInput);
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
}