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

    private void Update()
    {
        if (_componentManager.HasDied) return;

        try
        {
            TankTransformation();
        }
        catch (Exception error)
        {
          Debug.LogError($"PlayerInput: {error}");  
        }
        
        try
        {
            TankFire();
            ShellTypeSwitch();
        }
        catch (Exception error)
        {
            Debug.LogError($"PlayerInput: {error}");  
        }

        try
        {
            float turretYDelta = _componentManager.TurretControlComponent.TiltCannon(_tiltInput);
            //componentManager.HUDManager.UpdateCrosshairYPosition(turretYDelta);
            //playerHUD.SetTurretRotationUI(componentManager.TurretEulerAngles);
        }
        catch (Exception error)
        {
            Debug.LogError($"PlayerInput: {error}");  
        }
    }

    private void FixedUpdate()
    {
        if (_moveInput == 0 && _rotateInput == 0 && _componentManager.MoveComponent.GetTankVelocity() > 0)
        {
            _componentManager.MoveComponent.SlowTankDown();
        }
    }

    private void TankTransformation()
    {
        _moveInput = Input.GetAxis("Vertical");
        _rotateInput = Input.GetAxis("Horizontal");
        _tiltInput = Input.GetAxis("Mouse Y");
        
        if (_moveInput > 0 && _rotateInput == 0)
            _componentManager.MoveComponent.MoveForward(_moveInput);

        if (_moveInput < 0 && _rotateInput == 0)
            _componentManager.MoveComponent.MoveBackward(_moveInput);

        if(_rotateInput < 0 || _rotateInput > 0)
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
