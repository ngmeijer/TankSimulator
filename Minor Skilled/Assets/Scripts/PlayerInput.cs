using System;
using UnityEngine;

public class PlayerInput : TankComponent
{
    private float _moveInput;
    private float _hullRotateInput;
    private float _scrollInput;
    private float _mouseHorizontalInput;
    private float _cannonTiltInput;

    [SerializeField] private StateSwitcher _stateSwitcher;
    
    private void Start()
    {
        Debug.Assert(_stateSwitcher != null, $"StateSwitcher in PlayerInput is null. Assign in the inspector.");
        HUDManager.Instance.UpdateAmmoCount(_componentManager.ShootComponent.GetCurrentAmmoCount());
        HUDManager.Instance.UpdateShellTypeUI(_componentManager.ShootComponent.GetCurrentShellType());
    }

    private void Update()
    {
        if (_componentManager.HasDied) return;
        CheckStateSwitch();
    }

    private void CheckStateSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _stateSwitcher.SwitchToTankState(E_TankState.Combat);
            _stateSwitcher.SwitchToCamState(E_CameraState.ADS);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _stateSwitcher.SwitchToTankState(E_TankState.Combat);
            _stateSwitcher.SwitchToCamState(E_CameraState.ThirdPerson);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _stateSwitcher.SwitchToTankState(E_TankState.Inspection);
            _stateSwitcher.SwitchToCamState(E_CameraState.InspectMode);
        }
    }
}