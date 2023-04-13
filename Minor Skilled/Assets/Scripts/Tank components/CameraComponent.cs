using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public enum E_CameraState
{
    None,
    ADS,
    ThirdPerson,
    InspectMode,
    HostileInspection
}

public class CameraComponent : TankComponent
{
    [Header("Targets")] 
    [SerializeField] private Transform _raycaster;
    [SerializeField] private Transform _currentBarrelCrosshair;
    [SerializeField] private Transform _estimatedTargetCrosshair;
    [SerializeField] private Transform _hostileInspectPivot;
    
    private RaycastHit _currentHitData;
    private string _colliderTag;
    private bool _inTransition;
    [SerializeField] private float _transitionDuration = 1f;

    private PlayerStateSwitcher _playerStateSwitcher;
    private PlayerInputActions _inputActions;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _playerStateSwitcher = _componentManager.StateSwitcher as PlayerStateSwitcher;
        _inputActions = new PlayerInputActions();
        _inputActions.StateSwitcher.EnableHostileInspectionView.started += SelectEnemyToInspect;
        _inputActions.Enable();
    }

    private void LateUpdate()
    {
        CameraState camState = _playerStateSwitcher.CurrentCameraState;
        if (camState.ThisState == E_CameraState.InspectMode) return;

        GameManager.Instance.CurrentBarrelCrosshairPos = ConvertCurrentBarrelCrosshair();
        GameManager.Instance.TargetBarrelCrosshairPos = ConvertTargetBarrelCrosshair();

        RaycastToAllEnemies();

        if (RaycastForwardFromBarrelTip())
        {
            if (_currentHitData.collider.CompareTag("Enemy"))
            {
                GameManager.Instance.ValidTargetInSight = true;
                HUDManager.Instance.EnableInspectHostileText(true);
            }
            else
            {
                GameManager.Instance.ValidTargetInSight = false;
                HUDManager.Instance.EnableInspectHostileText(false);
            }
            
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
            Camera currentCam = _playerStateSwitcher.CurrentCameraState.ViewCam;
            
            //Bugfix for double screenpoints (indicator would appear on the mirrored position of enemy as well)
            Vector3 inversePos = currentCam.transform.InverseTransformPoint(entity.Value);
            inversePos.z = Mathf.Max(inversePos.z, 1.0f);
            Vector2 screenPos = currentCam.WorldToScreenPoint(currentCam.transform.TransformPoint(inversePos));
            Vector3 direction = entity.Value - currentCam.transform.position;

            bool rayCollision = Physics.Raycast(currentCam.transform.position, direction, out RaycastHit hitInfo);
            bool enemyCollision = hitInfo.collider.CompareTag("Enemy");
            bool enemyOnScreen = CheckIfVectorIsOnScreen(screenPos);
            
            if(rayCollision && enemyCollision && enemyOnScreen)
            {
                HUDManager.Instance.SetEnemyIndicator(entity.Key, screenPos, true);
                Debug.DrawLine(currentCam.transform.position, entity.Value, Color.green);
            }
            else
            {
                HUDManager.Instance.SetEnemyIndicator(entity.Key, screenPos, false); 
                Debug.DrawLine(currentCam.transform.position, entity.Value, Color.red);
            }
        }
    }

    private bool CheckIfVectorIsOnScreen(Vector2 screenPos)
    {
        bool xOnScreen = (screenPos.x >= 0 && screenPos.x <= Screen.width);
        bool yOnScreen = (screenPos.y >= 0 && screenPos.y <= Screen.height);

        return xOnScreen && yOnScreen;
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
        Vector3 currentPos = _estimatedTargetCrosshair.position;
        currentPos.x = _currentBarrelCrosshair.position.x;
        currentPos.y = _currentBarrelCrosshair.position.y;
        currentPos.z = _currentBarrelCrosshair.position.z;
        _estimatedTargetCrosshair.position = currentPos;
        Vector3 currentLocalPos = _estimatedTargetCrosshair.localPosition;
        currentLocalPos.x = 0;
        _estimatedTargetCrosshair.localPosition = currentLocalPos;

        Vector3 convertedPos = _playerStateSwitcher.CurrentCameraState.ViewCam.WorldToScreenPoint(_estimatedTargetCrosshair.position);
        convertedPos.y = GameManager.Instance.CurrentBarrelCrosshairPos.y;
        return convertedPos;
    }

    private void SelectEnemyToInspect(InputAction.CallbackContext cb)
    {
        GameManager.Instance.HostileTargetTransform = _currentHitData.collider.transform.root;
    }

    private bool RaycastForwardFromBarrelTip()
    {
        bool data = Physics.Raycast(_raycaster.position, _raycaster.forward,
            out _currentHitData, Mathf.Infinity);

        if (_currentHitData.collider != null)
            _colliderTag = _currentHitData.collider.tag;
        else _colliderTag = "No collision detected";

        return data && !_currentHitData.collider.CompareTag("Shell");
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