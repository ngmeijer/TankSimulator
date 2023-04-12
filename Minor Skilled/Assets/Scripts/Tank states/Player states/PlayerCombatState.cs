using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatState : TankCombatState
{
    private Vector2 _moveInput;
    private Vector2 _mouseInput;

    private PlayerInputActions _inputActions;
    private PlayerStateSwitcher _playerStateSwitcher;

    protected override void Start()
    {
        _playerStateSwitcher = _componentManager.StateSwitcher as PlayerStateSwitcher;

        _inputActions = new PlayerInputActions();
        _inputActions.Tankmovement.Shoot.started += TankFire;
        _inputActions.Tankmovement.IncreaseGear.started += IncreaseGear;
        _inputActions.Tankmovement.DecreaseGear.started += DecreaseGear;
        _inputActions.Tankmovement.ShellSwitch.started += ShellTypeSwitch;
        _inputActions.Tankmovement.Enable();
    }
    
    public override void EnterState()
    {
        base.EnterState();
        
        HandleCombatUI(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        
        HandleCombatUI(false);
    }

    public override void UpdateState()
    {
        GetInputValues();
        TankMove();
        HandleCrosshair();
        _componentManager.MoveComponent.UpdateHUD();
    }

    public override void FixedUpdateState()
    {
        if (_moveInput.y == 0 && _moveInput.x == 0 && _componentManager.MoveComponent.GetTankVelocity() > 0)
        {
            _componentManager.MoveComponent.SlowTankDown();
        }
    }

    public override void LateUpdateState()
    {
        _componentManager.TurretControlComponent.HandleTurretRotation(_mouseInput.x);
    }

    protected override void GetInputValues()
    {
        _moveInput = _inputActions.Tankmovement.Move.ReadValue<Vector2>();
        _mouseInput = _inputActions.Tankmovement.TurretRotate.ReadValue<Vector2>();
    }
    
    private void TankMove()
    {
        _componentManager.MoveComponent.MoveTank(_moveInput.y);

        if (_moveInput.x < 0 || _moveInput.x > 0)
            _componentManager.MoveComponent.RotateTank(_moveInput.x);
    }

    private void TankFire(InputAction.CallbackContext cb)
    {
        if (!_componentManager.ShootComponent.CanFire) return;

        _componentManager.ShootComponent.FireShell();
        HUDManager.Instance.UpdateAmmoCount(_componentManager.ShootComponent.GetCurrentAmmoCount());
        if (_componentManager.ShootComponent.GetCurrentAmmoCount() > 0)
            StartCoroutine(HUDManager.Instance.UpdateReloadUI(Properties.ReloadTime));
    }

    private void ShellTypeSwitch(InputAction.CallbackContext cb)
    {
        _componentManager.ShootComponent.SwitchShell();
        HUDManager.Instance.UpdateShellTypeUI(_componentManager.ShootComponent.GetCurrentShellType());
        HUDManager.Instance.UpdateAmmoCount(_componentManager.ShootComponent.GetCurrentAmmoCount());
    }

    private void IncreaseGear(InputAction.CallbackContext cb)
    {
        _componentManager.MoveComponent.IncreaseGear();
    }

    private void DecreaseGear(InputAction.CallbackContext cb)
    {
        _componentManager.MoveComponent.DecreaseGear();
    }

    private void HandleCrosshair()
    {
        if (_componentManager.ShootComponent.CurrentRange < _componentManager.ShootComponent.MinRange) return;
        if (_componentManager.ShootComponent.CurrentRange > _componentManager.ShootComponent.MaxRange) return;

        _componentManager.ShootComponent.UpdateCurrentRange(_mouseInput.y, GetSensitivityMultiplier());
        _componentManager.TurretControlComponent.OffsetCannonOnRangeChange(_componentManager.ShootComponent.RangePercent);
        HUDManager.Instance.UpdateCrosshair();
    }
    
    private float GetSensitivityMultiplier()
    {
        float multiplier = 0;

        E_CameraState camState = _playerStateSwitcher.CurrentCameraState.ThisState;
        if (camState == E_CameraState.ThirdPerson)
            multiplier = Properties.TP_VerticalSensitivity;
        else if (camState == E_CameraState.ADS)
            multiplier = Properties.ADS_VerticalSensitivity;

        Debug.Assert(multiplier != 0, "Sensitivity multiplier is 0. Check Properties SO and if the CamState is ThirdPerson or ADS.");
        
        return multiplier;
    }
    
    private void HandleCombatUI(bool enabled)
    {
        HUDManager.Instance.EnableCombatUI(enabled);
    }
}