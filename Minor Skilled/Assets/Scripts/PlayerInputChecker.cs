using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputChecker : SingletonMonobehaviour<PlayerInputChecker>
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
        _inputActions.StateSwitcher.ExitState.started += DisableInspection;
        _inputActions.StateSwitcher.EnableMenu.started += EnableMenuState;
        _inputActions.Enable();
    }

    private void EnableMenuState(InputAction.CallbackContext callbackContext)
    {
        _playerStateSwitcher.SwitchToTankState(E_TankState.Pause);
        HUDStateSwitcher.Instance.SwitchToHUDState(E_TankState.Pause);
    }

    private void EnableADS(InputAction.CallbackContext cb)
    {
        _playerStateSwitcher.SwitchToTankState(E_TankState.Combat);
        _playerStateSwitcher.SwitchToCamState(E_CameraState.ADS);
        HUDStateSwitcher.Instance.SwitchToHUDState(E_TankState.Combat);
    }

    private void EnableTP(InputAction.CallbackContext cb)
    {
        _playerStateSwitcher.SwitchToTankState(E_TankState.Combat);
        _playerStateSwitcher.SwitchToCamState(E_CameraState.ThirdPerson);
        HUDStateSwitcher.Instance.SwitchToHUDState(E_TankState.Combat);
    }

    private void EnableInspection(InputAction.CallbackContext cb)
    {
        _playerStateSwitcher.SwitchToTankState(E_TankState.Inspection);
        _playerStateSwitcher.SwitchToCamState(E_CameraState.InspectMode);
        HUDStateSwitcher.Instance.SwitchToHUDState(E_TankState.Inspection);
    }

    private void EnableHostileInspection(InputAction.CallbackContext cb)
    {
        if (GameManager.Instance.HostileTargetTransform == null) return;
        _playerStateSwitcher.SwitchToTankState(E_TankState.HostileInspection);
        _playerStateSwitcher.SwitchToCamState(E_CameraState.HostileInspection);
        HUDStateSwitcher.Instance.SwitchToHUDState(E_TankState.HostileInspection);
    }

    private void DisableInspection(InputAction.CallbackContext cb)
    {
        E_TankState currentState = _playerStateSwitcher.CurrentTankState.ThisState;
        
        TankState lastTankState = _playerStateSwitcher.LastTankState;
        CameraState lastCamState = _playerStateSwitcher.LastCameraState;
        if (currentState is E_TankState.Inspection or E_TankState.HostileInspection)
        {
            _playerStateSwitcher.SwitchToTankState(lastTankState.ThisState);
            HUDStateSwitcher.Instance.SwitchToHUDState(lastTankState.ThisState);
            _playerStateSwitcher.SwitchToCamState(lastCamState.ThisState);
        }
    }
}