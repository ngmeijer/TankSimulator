using System.Collections.Generic;
using TankComponents;
using UnityEngine;

namespace Tank_components
{
    public class NewMoveComponent : TankComponent
    {
        [SerializeField] private List<Collider> _wheels;
        [SerializeField] private Rigidbody _tankRB;
        [SerializeField] private float _moveForce;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField, Range(0.1f, 1f)]  private float _coverageCheckLength = 1f;
        [SerializeField] private MeshRenderer _leftTrackRenderer;    
        [SerializeField] private MeshRenderer _rightTrackRenderer;
        [SerializeField, Range(0.01f, 1f)] private float _animationSpeedRegulator = 1f;
        private Vector2 _textureOffset;
        private Vector2 _textureOffsetRightTrack;
        private float _groundCoverage;
        private float _coverageIncrement;
        private float GetTankVelocity() => _tankRB.velocity.magnitude;

        private void Start()
        {
            _coverageIncrement = 1f / _wheels.Count;
        }

        public void CheckGroundCoverage()
        {
            _groundCoverage = 0f;
            Vector3 averagedUpVec = Vector3.zero;
            foreach (Collider wheel in _wheels)
            {
                Vector3 raycastPosition = wheel.transform.position - new Vector3(0, wheel.bounds.extents.y, 0);
                bool hasCollision = Physics.Raycast(raycastPosition, -wheel.transform.up * _coverageCheckLength, out RaycastHit hit);
                if (!hasCollision)
                {
                    Debug.DrawRay(raycastPosition, -wheel.transform.up * _coverageCheckLength, Color.green);
                    continue;
                }

                _groundCoverage += _coverageIncrement;
                Debug.DrawLine(hit.point, raycastPosition, Color.yellow);
                averagedUpVec += hit.normal;
            }
        
            averagedUpVec /= _wheels.Count;
            transform.up = averagedUpVec;
            Debug.DrawRay(_componentManager.EntityOrigin.position, averagedUpVec, Color.yellow);
        }

        public void MoveForward(float inputValue, float multiplier = 1)
        {
            if (inputValue == 0f)
                return;

            Vector3 force = Vector3.forward * (_moveForce * inputValue * multiplier) + Vector3.up;
            force *= _groundCoverage;
            _tankRB.AddRelativeForce(force, ForceMode.Acceleration);
            _tankRB.velocity = Vector3.ClampMagnitude(_tankRB.velocity, _maxSpeed);
            AnimateTankTracks(inputValue);
        }

        public void HandleSteering(float inputValue)
        {
            if (inputValue == 0)
                return;

            Vector3 rotationDelta = Vector3.zero;
            rotationDelta.y += inputValue * _rotationSpeed * Time.deltaTime;
            transform.parent.eulerAngles += rotationDelta;
        
            MoveForward(Mathf.Abs(inputValue), 0.3f);
        }
    
        private void AnimateTankTracks(float inputValue)
        {
            float animationValue = (Time.deltaTime * inputValue * GetTankVelocity()) * _animationSpeedRegulator;
            _textureOffset.y = animationValue;
            _leftTrackRenderer.material.mainTextureOffset = _textureOffset;
            _rightTrackRenderer.material.mainTextureOffset = _textureOffset;

            //TODO Make collider the parent, a child object for the graphics and rotate the GFX only.
            foreach (Collider wheel in _wheels)
            {
                Vector3 newEuler = wheel.transform.localEulerAngles;
                newEuler.x += animationValue;
                newEuler.y = 0f;
                newEuler.z = 0f;
                wheel.transform.localEulerAngles += newEuler;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            foreach (var wheel in _wheels)
            {
                Vector3 raycastPosition = wheel.transform.position - new Vector3(0, wheel.bounds.extents.y, 0);
                Gizmos.DrawSphere(raycastPosition, 0.1f);
            }
        }
    }
}