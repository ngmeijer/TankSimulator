using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputChecker : TankComponent
{
    private float _moveInput;
    private float _hullRotateInput;
    private float _scrollInput;
    private float _mouseHorizontalInput;
    private float _cannonTiltInput;
    private PlayerInputActions _inputActions;

    [SerializeField] private PlayerStateSwitcher _playerStateSwitcher;
    
    private void Start()
    {
        Debug.Assert(_playerStateSwitcher != null, $"StateSwitcher in PlayerInput is null. Assign in the inspector.");

        _inputActions = new PlayerInputActions();
        _inputActions.StateSwitcher.EnableInspectionView.started += EnableInspection;
        _inputActions.StateSwitcher.EnableADSView.started += EnableADS;
        _inputActions.StateSwitcher.EnableThirdPersonView.started += EnableTP;
        _inputActions.StateSwitcher.EnableHostileInspectionView.started += EnableHostileInspection;
        _inputActions.Enable();
    }

    private void EnableADS(InputAction.CallbackContext cb)
    {
        _playerStateSwitcher.SwitchToTankState(E_TankState.Combat);
        _playerStateSwitcher.SwitchToCamState(E_CameraState.ADS);
    }

    private void EnableTP(InputAction.CallbackContext cb)
    {
        _playerStateSwitcher.SwitchToTankState(E_TankState.Combat);
        _playerStateSwitcher.SwitchToCamState(E_CameraState.ThirdPerson);
    }

    private void EnableInspection(InputAction.CallbackContext cb)
    {
        _playerStateSwitcher.SwitchToTankState(E_TankState.Inspection);
        _playerStateSwitcher.SwitchToCamState(E_CameraState.InspectMode);
    }

    private void EnableHostileInspection(InputAction.CallbackContext cb)
    {
        if (!GameManager.Instance.ValidTargetInSight) return;
        _playerStateSwitcher.SwitchToTankState(E_TankState.HostileInspection);
        _playerStateSwitcher.SwitchToCamState(E_CameraState.HostileInspection);
    }
}