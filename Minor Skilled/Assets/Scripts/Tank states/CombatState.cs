using UnityEngine;

public class CombatState : TankState
{
    private float _mouseVerticalInput;
    private float _moveInput;
    private float _hullRotateInput;
    private float _mouseHorizontalInput;

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
        TankFire();
        ShellTypeSwitch();
        IncreaseGear();
        DecreaseGear();
        Debug.Log("Update state combat");
        HandleCrosshair();
    }

    public override void FixedUpdateState()
    {
        TankMove();
        if (_moveInput == 0 && _hullRotateInput == 0 && ComponentManager.MoveComponent.GetTankVelocity() > 0)
        {
            ComponentManager.MoveComponent.SlowTankDown();
        }
    }

    public override void LateUpdateState()
    {
        Debug.Log("Late update combat state");
        ComponentManager.TurretControlComponent.HandleTurretRotation(_mouseHorizontalInput);
    }

    protected override void GetInputValues()
    {
        _mouseVerticalInput = Input.GetAxis("Mouse Y");
        _moveInput = Input.GetAxis("Vertical");
        _hullRotateInput = Input.GetAxis("Horizontal");
        _mouseHorizontalInput = Input.GetAxis("Mouse X");
    }
    
    private void TankMove()
    {
        ComponentManager.MoveComponent.MoveTank(_moveInput);

        if (_hullRotateInput < 0 || _hullRotateInput > 0)
            ComponentManager.MoveComponent.RotateTank(_hullRotateInput);

        ComponentManager.MoveComponent.UpdateHUD();
    }

    private void TankFire()
    {
        if (Input.GetAxis("Fire1") == 0) return;
        if (!ComponentManager.ShootComponent.CanFire) return;

        ComponentManager.ShootComponent.FireShell();
        HUDManager.Instance.UpdateAmmoCount(ComponentManager.ShootComponent.GetCurrentAmmoCount());
        if (ComponentManager.ShootComponent.GetCurrentAmmoCount() > 0)
            StartCoroutine(HUDManager.Instance.UpdateReloadUI(Properties.ReloadTime));
    }

    private void ShellTypeSwitch()
    {
        if (!Input.GetKeyDown(KeyCode.Tab)) return;

        ComponentManager.ShootComponent.SwitchShell();
        HUDManager.Instance.UpdateShellTypeUI(ComponentManager.ShootComponent.GetCurrentShellType());
        HUDManager.Instance.UpdateAmmoCount(ComponentManager.ShootComponent.GetCurrentAmmoCount());
    }

    private void IncreaseGear()
    {
        if (!Input.GetKeyDown(KeyCode.LeftShift)) return;

        ComponentManager.MoveComponent.IncreaseGear();
    }

    private void DecreaseGear()
    {
        if (!Input.GetKeyDown(KeyCode.LeftAlt)) return;

        ComponentManager.MoveComponent.DecreaseGear();
    }

    private void HandleCrosshair()
    {
        if (ComponentManager.ShootComponent.CurrentRange < ComponentManager.ShootComponent.MinRange) return;
        if (ComponentManager.ShootComponent.CurrentRange > ComponentManager.ShootComponent.MaxRange) return;

        ComponentManager.ShootComponent.UpdateCurrentRange(_mouseVerticalInput, GetSensitivityMultiplier());
        ComponentManager.TurretControlComponent.OffsetCannonOnRangeChange(ComponentManager.ShootComponent.RangePercent);
        HUDManager.Instance.UpdateCrosshair();
    }

    private float GetSensitivityMultiplier()
    {
        float multiplier = 0;

        E_CameraState camState = ComponentManager.StateSwitcher.CurrentCameraState.ThisState;
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