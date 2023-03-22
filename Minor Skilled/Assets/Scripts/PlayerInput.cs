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
    private ShootComponent _shootComponent;

    protected override void Awake()
    {
        base.Awake();

        _cameraComponent = GetComponent<CameraComponent>();
    }

    private void Start()
    {
        _shootComponent = _componentManager.ShootComponent;
        HUDManager.Instance.UpdateAmmoCount(_componentManager.ShootComponent.GetCurrentAmmoCount());
        HUDManager.Instance.UpdateShellTypeUI(_componentManager.ShootComponent.GetCurrentShellType());
    }

    private void Update()
    {
        if (_componentManager.HasDied) return;

        GetInputValues();
        TankFire();
        ShellTypeSwitch();
        IncreaseGear();
        DecreaseGear();
        _componentManager.TurretControlComponent.HandleTurretRotation();
        //_componentManager.TurretControlComponent.TiltCannon(_tiltInput);
        _cameraComponent.OffsetCameraOnCannonTilt(_tiltInput);

        HandleCrosshair();
    }

    private void LateUpdate()
    {
        _cameraComponent.UpdateCameraPosition(_turretRotateInput);
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
        _scrollInput = Input.GetAxis("Mouse ScrollWheel");
    }

    private void HandleCrosshair()
    {
        //HUDManager.Instance.UpdateCurrentRange(_componentManager.ShootComponent.CurrentRange);
        
        if (_componentManager.ShootComponent.CurrentRange < _componentManager.ShootComponent.MinRange) return;
        if (_componentManager.ShootComponent.CurrentRange > _componentManager.ShootComponent.MaxRange) return;

        _componentManager.TurretControlComponent.OffsetCannonOnRangeChange(_scrollInput);
        _componentManager.ShootComponent.UpdateCurrentRange(_scrollInput);
        HUDManager.Instance.UpdateCrosshair(_scrollInput, _shootComponent.CurrentDistanceToTarget, _shootComponent.CurrentRange);
    }

    private void TankMove()
    {
        _componentManager.MoveComponent.MoveTank(_moveInput);

        if (_hullRotateInput < 0 || _hullRotateInput > 0)
            _componentManager.MoveComponent.RotateTank(_hullRotateInput);

        _componentManager.MoveComponent.UpdateHUD();
    }

    private void TankFire()
    {
        if (Input.GetAxis("Fire1") == 0) return;
        if (!_componentManager.ShootComponent.CanFire) return;
        
        //_cameraComponent.ShakeCamera();
        _componentManager.ShootComponent.FireShell();
        HUDManager.Instance.UpdateAmmoCount(_componentManager.ShootComponent.GetCurrentAmmoCount());
        if (_componentManager.ShootComponent.GetCurrentAmmoCount() > 0)
            StartCoroutine(HUDManager.Instance.UpdateReloadUI(_properties.ReloadTime));
    }

    private void ShellTypeSwitch()
    {
        if (!Input.GetKeyDown(KeyCode.Tab)) return;

        _componentManager.ShootComponent.SwitchShell();
        HUDManager.Instance.UpdateShellTypeUI(_componentManager.ShootComponent.GetCurrentShellType());
        HUDManager.Instance.UpdateAmmoCount(_componentManager.ShootComponent.GetCurrentAmmoCount());
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