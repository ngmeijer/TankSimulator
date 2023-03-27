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
    private float _scrollInput;
    private float _turretRotateInput;
    private float _cannonTiltInput;

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
        HandleADSZoom();
        HandleCrosshair();
    }

    private void LateUpdate()
    {
        _componentManager.TurretControlComponent.HandleTurretRotation(_turretRotateInput);
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
        _turretRotateInput = Input.GetAxis("Mouse X");
        _cannonTiltInput = Input.GetAxis("Mouse Y");
        _scrollInput = Input.GetAxis("Mouse ScrollWheel");
    }

    private void HandleCrosshair()
    {
        if (_componentManager.ShootComponent.CurrentRange < _componentManager.ShootComponent.MinRange) return;
        if (_componentManager.ShootComponent.CurrentRange > _componentManager.ShootComponent.MaxRange) return;

        float barrelRotationInput = _cameraComponent.CamMode == CameraMode.ThirdPerson ? _cannonTiltInput : _scrollInput;
        float multiplier = _cameraComponent.CamMode == CameraMode.ThirdPerson ? _properties.VerticalCrosshairSpeed : _properties.TurretTiltSpeed;
        _componentManager.ShootComponent.UpdateCurrentRange(barrelRotationInput, multiplier);
        
        _componentManager.TurretControlComponent.OffsetCannonOnRangeChange(_shootComponent.RangePercent);
        HUDManager.Instance.UpdateCrosshair(_shootComponent.CurrentDistanceToTarget, _shootComponent.CurrentRange,
            _shootComponent.RangePercent);
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

    private void HandleADSZoom()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        _cameraComponent.ZoomADS();
    }
}