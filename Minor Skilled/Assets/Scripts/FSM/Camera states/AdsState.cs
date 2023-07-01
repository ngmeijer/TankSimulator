using System.Collections;
using FSM.HUDStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM.CameraStates
{
    public class AdsState : CameraState
    {
        private float[] _fovRanges =
        {
            60f,
            20f,
            10f,
            5f,
            2.5f
        };

        private int _currentFOVIndex;

        private PlayerInputActions _inputActions;

        private HUDCombatState _hudCombatState;
        [SerializeField] private float _fovLerpSpeed = 0.5f;


        private void Start()
        {
            _inputActions = new PlayerInputActions();
            _inputActions.TankMovement.ZoomADS.started += ZoomADSWrapper;
            _inputActions.Enable();

            _hudCombatState = HUDStateSwitcher.Instance.HUDCombatState as HUDCombatState;
        }

        public override void Enter()
        {
            base.Enter();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _currentFOVIndex = 0;
            ViewCam.fieldOfView = _fovRanges[_currentFOVIndex];
            _hudCombatState.SetZoomLevelText(_currentFOVIndex + 1, true);
        }

        public override int GetFOVLevel()
        {
            return _currentFOVIndex;
        }

        private void ZoomADSWrapper(InputAction.CallbackContext cb)
        {
            if (!_stateActive)
                return;

            if (_currentFOVIndex >= _fovRanges.Length - 1)
                _currentFOVIndex = 0;
            else _currentFOVIndex++;

            float newFOV = _fovRanges[_currentFOVIndex];
            StartCoroutine(ZoomADS(newFOV));
            _hudCombatState.SetZoomLevelText(_currentFOVIndex + 1, true);
        }

        private IEnumerator ZoomADS(float newFOV)
        {
            float initialFOV = ViewCam.fieldOfView;
            float elapsedTime = 0f;

            while (elapsedTime < _fovLerpSpeed)
            {
                float time = elapsedTime / _fovLerpSpeed;
                float lerpedFOV = Mathf.Lerp(initialFOV, newFOV, time);
                ViewCam.fieldOfView = lerpedFOV;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            ViewCam.fieldOfView = newFOV;
        }

        public override void Exit()
        {
            base.Exit();

            _hudCombatState.SetZoomLevelText(_currentFOVIndex + 1, false);
        }
    }
}