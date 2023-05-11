using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public enum E_CameraState
{
    None,
    ADS,
    ThirdPerson,
    InspectMode,
    HostileInspection,
    Death,
}

public class CameraComponent : TankComponent
{
    [Header("Targets")] 
    [SerializeField] private Transform _raycaster;
    [SerializeField] private Transform _currentBarrelCrosshair;
    [SerializeField] private Transform _estimatedTargetCrosshair;
    
    private RaycastHit _currentHitData;
    private string _colliderTag;
    private bool _inTransition;

    private PlayerStateSwitcher _playerStateSwitcher;
    private HUDCombatState _hudCombatState;
    private PlayerInputActions _inputActions;

    private Camera _currentCamera;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _playerStateSwitcher = _componentManager.StateSwitcher as PlayerStateSwitcher;

        _hudCombatState = HUDStateSwitcher.Instance.HUDCombatState as HUDCombatState;
        
        _inputActions = new PlayerInputActions();
        _inputActions.StateSwitcher.EnableHostileInspectionView.started += SelectEnemyToInspect;
        _inputActions.Enable();
    }

    private void LateUpdate()
    {
        CameraState camState = _playerStateSwitcher.CurrentCameraState;
        _currentCamera = camState.ViewCam;
        if (camState.ThisState == E_CameraState.InspectMode) return;

        GameManager.Instance.CurrentBarrelCrosshairPos = ConvertCurrentBarrelCrosshair();
        GameManager.Instance.TargetBarrelCrosshairPos = ConvertTargetBarrelCrosshair();

        RaycastToAllEnemies();

        if (RaycastForwardFromBarrelTip())
        {
            _hudCombatState.EnableInspectHostileText(_currentHitData.collider.CompareTag("Enemy"));
            _currentBarrelCrosshair.position = _currentHitData.point;
            Debug.DrawLine(_raycaster.position, _currentHitData.point, Color.green);
        }
        else Debug.DrawLine(_raycaster.position, _raycaster.forward * 1000f, Color.red);
    }

    private void RaycastToAllEnemies()
    {
        Dictionary<int, Vector3> entityPositions = GameManager.Instance.EntityWorldPositions;
        foreach (var entity in entityPositions)
        {
            Vector3 direction = entity.Value - _currentCamera.transform.position;
            Vector3 screenPos = ConvertWorldIntoScreenPos(entity.Value);

            if (!Physics.Raycast(_currentCamera.transform.position, direction, out RaycastHit hitInfo))
            {
                HandleEnemyIndicator(entity, false, screenPos);
                return;
            }

            if (!hitInfo.collider.CompareTag("Enemy"))
            {
                HandleEnemyIndicator(entity, false, screenPos);
                return;
            }
            
            HandleEnemyIndicator(entity, true, screenPos);
        }
    }

    private void HandleEnemyIndicator(KeyValuePair<int, Vector3> entity, bool canSeeTarget, Vector2 screenPos)
    {
        _hudCombatState.SetEnemyIndicator(entity.Key, screenPos, canSeeTarget);
        Debug.DrawLine(_currentCamera.transform.position, entity.Value, canSeeTarget ? Color.green : Color.red);
    }

    private Vector3 ConvertWorldIntoScreenPos(Vector3 worldPos)
    {
        //Bugfix for double screenpoints (indicator would appear on the mirrored position of enemy as well)
        Vector3 inversePos = _currentCamera.transform.InverseTransformPoint(worldPos);
        inversePos.z = Mathf.Max(inversePos.z, 1.0f);
        return _currentCamera.WorldToScreenPoint(_currentCamera.transform.TransformPoint(inversePos));
    }

    private Vector3 ConvertCurrentBarrelCrosshair()
    {
        Vector3 posToConvert = _currentHitData.point == Vector3.zero || _colliderTag == "Shell"
            ? _currentBarrelCrosshair.position
            : _currentHitData.point;

        return _playerStateSwitcher.CurrentCameraState.ViewCam.WorldToScreenPoint(posToConvert);
    }

    private Vector3 ConvertTargetBarrelCrosshair()
    {
        Vector3 convertedPos = _playerStateSwitcher.CurrentCameraState.ViewCam.WorldToScreenPoint(_estimatedTargetCrosshair.position);
        convertedPos.y = GameManager.Instance.CurrentBarrelCrosshairPos.y;
        convertedPos.z = GameManager.Instance.CurrentBarrelCrosshairPos.z;
        return convertedPos;
    }

    private void SelectEnemyToInspect(InputAction.CallbackContext cb)
    {
        GameManager.Instance.HostileTargetTransform = _currentHitData.collider.transform.root;
    }

    private bool RaycastForwardFromBarrelTip()
    {
        bool hasCollision = Physics.Raycast(_raycaster.position, _raycaster.forward,
            out _currentHitData, Mathf.Infinity);

        if (_currentHitData.collider != null)
            _colliderTag = _currentHitData.collider.tag;
        else _colliderTag = "No collision detected";

        return hasCollision && !_currentHitData.collider.CompareTag("Shell");
    }

    private void OnDrawGizmos()
    {
        if (_currentHitData.point != Vector3.zero && !_currentHitData.collider.CompareTag("Shell"))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_currentHitData.point, 0.3f);
            Handles.Label(_currentHitData.point + GameManager.HandlesOffset, "Hitpoint");
        }

        Handles.Label(_raycaster.position, $"Collider hit: {_colliderTag}");

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(_currentBarrelCrosshair.position, 0.2f);
        Handles.Label(_currentBarrelCrosshair.position + GameManager.HandlesOffset,
            _currentBarrelCrosshair.name);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(_estimatedTargetCrosshair.position, 0.2f);
        Handles.Label(_estimatedTargetCrosshair.position + GameManager.HandlesOffset,
            _estimatedTargetCrosshair.name);

        if (GameManager.Instance != null)
        {
            Gizmos.color = Color.red;
            Dictionary<int, Vector3> entityPositions = GameManager.Instance.EntityWorldPositions;
            foreach (var entityPos in entityPositions)
            {
                Gizmos.DrawWireSphere(entityPos.Value, 3f);
            }
        }
    }
}