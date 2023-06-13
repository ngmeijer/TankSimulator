using System;
using UnityEditor;
using UnityEngine;

public class TurretControlComponent : TankComponent
{
    [Header("Tank part transforms")] [SerializeField]
    private Transform _turretTransform;
    public Transform TurretTransform
    {
        get { return _turretTransform; }
    }
    
    [SerializeField] private Transform _barrelTransform;
    [SerializeField] private Transform _rotationTargetParent;
    public Transform RotationTarget
    {
        get { return _rotationTargetParent; }
    }
    
    [Header("Barrel rotation")] 
    [SerializeField] private Transform _barrelLowerBound;
    [SerializeField] private Transform _barrelUpperBound;
    [SerializeField] private Transform _barrelLookAt;

    public Transform BarrelLookAt
    {
        get { return _barrelLookAt; }
    }
    
    private float _currentXRotation;
    
    private PlayerStateSwitcher _playerStateSwitcher;

    protected void Start()
    {
        _playerStateSwitcher = _componentManager.StateSwitcher as PlayerStateSwitcher;
        Debug.Assert(_turretTransform != null, $"TurretTransform on '{gameObject.transform.root.name}' is null.");
        Debug.Assert(_barrelTransform != null, $"BarrelTransform on '{gameObject.transform.root.name}' is null.");
    }

    public void HandleTurretRotation(float rotateInput, float multiplier)
    {
        Vector3 newRotation = new Vector3(0,
            _rotationTargetParent.localEulerAngles.y + (rotateInput * multiplier * Time.deltaTime), 0);
        _rotationTargetParent.localEulerAngles = newRotation;
        
        _turretTransform.rotation = Quaternion.RotateTowards(_turretTransform.rotation, _rotationTargetParent.rotation,
                multiplier * Time.deltaTime);
    }

    public void AdjustCannonRotation(float currentRotationPercent)
    {
        float minY = _barrelLowerBound.position.y;
        float maxY = _barrelUpperBound.position.y;
        float maxLength = maxY - minY;
        float yDelta = currentRotationPercent * maxLength;
        
        Vector3 currentPosition = _barrelLookAt.transform.position;
        Vector3 newPosition = currentPosition;
        newPosition.y = minY + yDelta;
        
        _barrelLookAt.transform.position = Vector3.MoveTowards(currentPosition,
            newPosition, DetermineSensitivityMultiplier() * Time.deltaTime);

        Vector3 lookatPosition = new Vector3(_barrelLowerBound.position.x, _barrelLookAt.position.y, _barrelLowerBound.position.z);
        _barrelTransform.LookAt(lookatPosition);
    }

    private float DetermineSensitivityMultiplier()
    {
        E_CameraState camState = _playerStateSwitcher.CurrentCameraState.ThisState;
        float multiplier = 0;
        if (camState == E_CameraState.ThirdPerson)
            multiplier = _componentManager.Properties.TP_VerticalSensitivity;
        else if (camState == E_CameraState.ADS)
        {
            int currentFOV = _playerStateSwitcher.CurrentCameraState.GetFOVLevel();
            multiplier = _componentManager.Properties.ADS_VerticalSensitivity[currentFOV];
        }

        return multiplier;
    }

    public Vector3 GetCurrentBarrelDirection()
    {
        return _barrelTransform.forward;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_barrelLowerBound.position, 0.25f);
        Handles.Label(_barrelLowerBound.position + GameManager.HandlesOffset, _barrelLowerBound.name);
        Gizmos.DrawSphere(_barrelUpperBound.position, 0.25f);
        Handles.Label(_barrelUpperBound.position + GameManager.HandlesOffset, _barrelUpperBound.name);
        Gizmos.DrawLine(_barrelLowerBound.position, _barrelUpperBound.position);

        if (_barrelLookAt != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_barrelLookAt.position, 0.2f);
            Handles.Label(_barrelLookAt.position + GameManager.HandlesOffset, _barrelLookAt.name);
        }

        if (_rotationTargetParent != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(_rotationTargetParent.position, 0.2f);
            Handles.Label(_rotationTargetParent.position + GameManager.HandlesOffset, _rotationTargetParent.name);
        }
    }
}