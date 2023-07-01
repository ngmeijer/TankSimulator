using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM.CameraStates
{
    public class InspectorCamState : CameraState
    {
        [SerializeField] private float _horizontalSpeed = 50f;
        [SerializeField] private float _verticalSpeed = 25f;
        [SerializeField] private float _zoomSpeed = 50f;
        [SerializeField] private float _cameraFollowSpeed = 10f;
        [SerializeField] private Transform _lowerBound;
        [SerializeField] private Transform _upperBound;
        [SerializeField] private Transform _rotationTarget;
        private Vector2 _scrollInput;
        private Vector2 _mouseInput;
        private float _zoomLevel;
        private float _zoomPosition;
        private Vector3 _currentPosDelta;

        private Vector3 _totalPosDelta;
        private Vector3 _maxPosDelta;
        private Vector3 _camStartPos;

        private PlayerInputActions _inputActions;
        protected bool _canMove;

        public override void Enter()
        {
            base.Enter();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            _inputActions = new PlayerInputActions();
            _inputActions.TankInspection.AllowInspection.started += EnableRotate;
            _inputActions.TankInspection.AllowInspection.canceled += DisableRotate;
            _inputActions.TankInspection.Enable();

            _maxPosDelta = _lowerBound.localPosition - _upperBound.localPosition;
            GameManager.Instance.InspectionCameraTrans = ViewCam.transform;
        }

        public override void UpdateState()
        {
            if (InTransition) return;

            _currentPosDelta = Vector3.zero;
            GetInputValues();

            ZoomInspectView();

            if (!_canMove) return;

            MoveCameraHorizontally();
            MoveCameraVertically();
        }

        public override void LateUpdateState()
        {
            _totalPosDelta += _currentPosDelta;
            _totalPosDelta.x = 0;
            _totalPosDelta.y = Mathf.Clamp(_totalPosDelta.y, _maxPosDelta.y, 0);
            _totalPosDelta.z = Mathf.Clamp(_totalPosDelta.z, 0, _maxPosDelta.z);
            CameraTargetDestination.localPosition = _upperBound.localPosition + _totalPosDelta;

            ViewCam.transform.localPosition = Vector3.Slerp(ViewCam.transform.localPosition,
                CameraTargetDestination.localPosition, _cameraFollowSpeed * Time.deltaTime);

            ViewCam.transform.LookAt(StateLookAt);
            CameraTargetDestination.LookAt(StateLookAt);
        }

        private void MoveCameraHorizontally()
        {
            _rotationTarget.eulerAngles += new Vector3(0, _mouseInput.x * _horizontalSpeed * Time.deltaTime, 0);

            StateLookAt.rotation = Quaternion.RotateTowards(StateLookAt.rotation, _rotationTarget.rotation,
                _horizontalSpeed * Time.deltaTime);
        }

        protected override void GetInputValues()
        {
            _scrollInput = _inputActions.TankInspection.Zoom.ReadValue<Vector2>();
            _mouseInput = _inputActions.TankInspection.InspectTank.ReadValue<Vector2>();
        }

        private void EnableRotate(InputAction.CallbackContext cb)
        {
            _canMove = true;
        }

        private void DisableRotate(InputAction.CallbackContext cb)
        {
            _canMove = false;
            _rotationTarget.rotation = StateLookAt.rotation;
        }

        private void MoveCameraVertically()
        {
            _currentPosDelta += Vector3.up * (_mouseInput.y * _verticalSpeed * Time.deltaTime);
        }

        private void ZoomInspectView()
        {
            _currentPosDelta += Vector3.forward * (_scrollInput.y * _zoomSpeed * Time.deltaTime);
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_upperBound.position, 0.2f);
            Handles.Label(_upperBound.position + GameManager.HandlesOffset, _upperBound.name);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_upperBound.position, _lowerBound.position);
            Handles.color = Color.red;
            Vector3[] lines =
            {
                _upperBound.position,
                CameraTargetDestination.position,
                CameraTargetDestination.position,
                _lowerBound.position
            };
            Handles.DrawLines(lines);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_lowerBound.position, 0.2f);
            Handles.Label(_lowerBound.position + GameManager.HandlesOffset, _lowerBound.name);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(CameraTargetDestination.position, 0.15f);
            Handles.Label(CameraTargetDestination.position + GameManager.HandlesOffset, CameraTargetDestination.name);

            if (_rotationTarget != null && StateLookAt != null)
            {
                Handles.color = Color.red;
                Handles.DrawSolidArc(StateLookAt.position, StateLookAt.up, StateLookAt.right,
                    _rotationTarget.eulerAngles.y - StateLookAt.eulerAngles.y, 1f);
            }
        }
    }
}