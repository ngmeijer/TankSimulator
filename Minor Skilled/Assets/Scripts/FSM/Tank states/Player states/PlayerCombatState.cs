using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatState : TankCombatState
{
    private Vector2 _moveForwardInput;
    private Vector2 _rotateTankInput;
    private Vector2 _mouseInput;

    private PlayerInputActions _inputActions;
    private PlayerStateSwitcher _playerStateSwitcher;
    private HUDCombatState _hudCombatState;

    protected override void Awake()
    {
        base.Awake();
        
        _inputActions = new PlayerInputActions();
        _inputActions.TankMovement.Shoot.started += TankFire;
        _inputActions.TankMovement.IncreaseGear.started += IncreaseGear;
        _inputActions.TankMovement.DecreaseGear.started += DecreaseGear;
        _inputActions.TankMovement.ShellSwitch.started += ShellTypeSwitch;
    }
    
    protected override void Start()
    {
        _playerStateSwitcher = _componentManager.StateSwitcher as PlayerStateSwitcher;
        _hudCombatState = HUDStateSwitcher.Instance.HUDCombatState as HUDCombatState;
    }

    public override void Enter()
    {
        base.Enter();
        
        _inputActions.TankMovement.Enable();
    }

    public override void Exit()
    {
        base.Exit();
        
        _inputActions.TankMovement.Disable();
    }

    public override void UpdateState()
    {
        if (_playerStateSwitcher.CurrentCameraState.InTransition) return;
        GetInputValues();
        HandleCrosshair();
    }

    public override void FixedUpdateState()
    {
        _componentManager.MoveComponent.CheckGroundCoverage();
        _componentManager.MoveComponent.MoveForward(_moveForwardInput.y);
        _componentManager.MoveComponent.HandleSteering(_rotateTankInput.x);
    }

    public override void LateUpdateState()
    {
        if (_playerStateSwitcher.CurrentCameraState.InTransition) return;
        float multiplier = 0;
        E_CameraState camState = _playerStateSwitcher.CurrentCameraState.ThisState;
        if (camState == E_CameraState.ThirdPerson)
            multiplier = _componentManager.Properties.TP_HorizontalSensitivity;
        else if (camState == E_CameraState.ADS)
        {
            int currentFOV = _playerStateSwitcher.CurrentCameraState.GetFOVLevel();
            multiplier = _componentManager.Properties.ADS_HorizontalSensitivity[currentFOV];
        }
        
        _componentManager.TurretControlComponent.HandleTurretRotation(_mouseInput.x, multiplier);
    }

    protected override void GetInputValues()
    {
        _moveForwardInput = _inputActions.TankMovement.MoveTank.ReadValue<Vector2>();
        _rotateTankInput = _inputActions.TankMovement.RotateTank.ReadValue<Vector2>();
        
        _mouseInput = _inputActions.TankMovement.TurretRotate.ReadValue<Vector2>();
    }

    private void TankFire(InputAction.CallbackContext cb)
    {
        if (!_componentManager.ShootComponent.CanFire) return;

        _componentManager.ShootComponent.FireShell();
        _hudCombatState.UpdateAmmoCount(_componentManager.ShootComponent.GetCurrentAmmoCount());
        if (_componentManager.ShootComponent.GetCurrentAmmoCount() > 0)
            StartCoroutine(_hudCombatState.UpdateReloadUI(Properties.ReloadTime));
    }

    private void ShellTypeSwitch(InputAction.CallbackContext cb)
    {
        _componentManager.ShootComponent.SwitchShell();
        _hudCombatState.UpdateShellTypeUI(_componentManager.ShootComponent.GetCurrentShellType());
        _hudCombatState.UpdateAmmoCount(_componentManager.ShootComponent.GetCurrentAmmoCount());
    }

    private void IncreaseGear(InputAction.CallbackContext cb)
    {
        //_componentManager.MoveComponent.IncreaseGear();
    }

    private void DecreaseGear(InputAction.CallbackContext cb)
    {
        //_componentManager.MoveComponent.DecreaseGear();
    }

    private void HandleCrosshair()
    {
        if (_componentManager.ShootComponent.CurrentRange < _componentManager.ShootComponent.MinRange) return;
        if (_componentManager.ShootComponent.CurrentRange > _componentManager.ShootComponent.MaxRange) return;

        _componentManager.ShootComponent.UpdateCurrentRange(_mouseInput.y, GetSensitivityMultiplier());
        _componentManager.TurretControlComponent.AdjustCannonRotation(_componentManager.ShootComponent.RangePercent);
        _hudCombatState.UpdateCrosshair();
    }
    
    private float GetSensitivityMultiplier()
    {
        float multiplier = 0;

        E_CameraState camState = _playerStateSwitcher.CurrentCameraState.ThisState;
        if (camState == E_CameraState.ThirdPerson)
            multiplier = Properties.TP_VerticalSensitivity;
        else if (camState == E_CameraState.ADS)
        {
            int currentFOV = _playerStateSwitcher.CurrentCameraState.GetFOVLevel();
            multiplier = _componentManager.Properties.ADS_HorizontalSensitivity[currentFOV];
        }

        Debug.Assert(multiplier != 0, "Sensitivity multiplier is 0. Check Properties SO and if the CamState is ThirdPerson or ADS.");
        
        return multiplier;
    }
}