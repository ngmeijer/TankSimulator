using System;
using FSM.HUDStates;
using TankComponents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM.TankStates
{
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
            _playerStateSwitcher = _componentManager.ThisStateSwitcher as PlayerStateSwitcher;
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
            _componentManager.MoveComp.CheckGroundCoverage();
            _componentManager.MoveComp.MoveForward(_moveForwardInput.y);
            _componentManager.MoveComp.HandleSteering(_rotateTankInput.x);
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

            _componentManager.TurretControlComp.HandleTurretRotation(_mouseInput.x, multiplier);
        }

        protected override void GetInputValues()
        {
            _moveForwardInput = _inputActions.TankMovement.MoveTank.ReadValue<Vector2>();
            _rotateTankInput = _inputActions.TankMovement.RotateTank.ReadValue<Vector2>();

            _mouseInput = _inputActions.TankMovement.TurretRotate.ReadValue<Vector2>();
        }

        private void TankFire(InputAction.CallbackContext cb)
        {
            if (!_componentManager.ShootComp.CanFire) return;

            _componentManager.ShootComp.FireShell();
            _hudCombatState.UpdateAmmoCount(_componentManager.ShootComp.GetCurrentAmmoCount());
            if (_componentManager.ShootComp.GetCurrentAmmoCount() > 0)
                StartCoroutine(_hudCombatState.UpdateReloadUI(_properties.ReloadTime));
        }

        private void ShellTypeSwitch(InputAction.CallbackContext cb)
        {
            _componentManager.ShootComp.SwitchShell();
            _hudCombatState.UpdateShellTypeUI(_componentManager.ShootComp.GetCurrentShellType());
            _hudCombatState.UpdateAmmoCount(_componentManager.ShootComp.GetCurrentAmmoCount());
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
            if (_componentManager.ShootComp.CurrentRange < _componentManager.ShootComp.MinRange) return;
            if (_componentManager.ShootComp.CurrentRange > _componentManager.ShootComp.MaxRange) return;

            _componentManager.ShootComp.UpdateCurrentRange(_mouseInput.y, GetSensitivityMultiplier());
            _componentManager.TurretControlComp.AdjustCannonRotation(_componentManager.ShootComp.RangePercent);
            _hudCombatState.UpdateCrosshair();
        }

        private float GetSensitivityMultiplier()
        {
            float multiplier = 0;

            E_CameraState camState = _playerStateSwitcher.CurrentCameraState.ThisState;
            if (camState == E_CameraState.ThirdPerson)
                multiplier = _properties.TP_VerticalSensitivity;
            else if (camState == E_CameraState.ADS)
            {
                int currentFOV = _playerStateSwitcher.CurrentCameraState.GetFOVLevel();
                multiplier = _componentManager.Properties.ADS_HorizontalSensitivity[currentFOV];
            }

            Debug.Assert(multiplier != 0,
                "Sensitivity multiplier is 0. Check Properties SO and if the CamState is ThirdPerson or ADS.");

            return multiplier;
        }
    }
}