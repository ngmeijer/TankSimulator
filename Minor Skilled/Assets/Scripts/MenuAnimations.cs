using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuAnimations : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    [SerializeField] private float _sensitivity = 10f;
    [SerializeField] private float _slerpSpeed = 1f;
    private float _slerpProgress;

    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _mouseTransform;
    [SerializeField] private float _distFromCam = 10f;

    [SerializeField] private Transform _credits_cameraTargetPosition;
    [SerializeField] private Transform _credits_cameraLookAtPosition;
    [SerializeField] private float _credits_tweenSpeed;
    
    [Header("Cannon animations")]
    [SerializeField] private Transform _minPosition;
    [SerializeField] private Transform _maxPosition;
    [SerializeField] private Transform _cannonTransform;
    [SerializeField] private Transform _cannonLookAt;
    [SerializeField] private Transform _lookAtTargetPos;

    [SerializeField] private ParticleSystem _cannonParticles;
    [SerializeField] private Transform _particlesSpawnpoint;

    private void Start()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.MenuInput.Click.started += FireCannon;
        _inputActions.Enable();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        RotateCannon();
    }

    private void RotateCannon()
    {
        Vector3 currentPos = _cannonLookAt.position;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _distFromCam));
        _mouseTransform.position = mouseWorldPosition;
        Vector3 newPos = currentPos;
        newPos.y = mouseWorldPosition.y;
        Vector3 slerpedPos = Vector3.Slerp(currentPos, newPos, Time.time * _slerpSpeed);
        _lookAtTargetPos.position = slerpedPos;
        _cannonTransform.LookAt(_lookAtTargetPos);
    }

    private void FireCannon(InputAction.CallbackContext cb)
    {
        if (_cannonParticles.isPlaying)
            return;

        _cannonParticles.transform.position = _particlesSpawnpoint.position;
        _cannonParticles.transform.rotation = _particlesSpawnpoint.rotation;
        _cannonParticles.Play();
    }

    public void MoveCameraToCredits()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_cam.transform.DOMove(_credits_cameraTargetPosition.position, _credits_tweenSpeed));
        seq.Append(_cam.transform.DOLookAt(_credits_cameraLookAtPosition.position, _credits_tweenSpeed));
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_cannonLookAt.transform.position, 0.25f);
        Handles.Label(_cannonLookAt.transform.position, "Cannon LookAt");
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_mouseTransform.transform.position, 0.25f);
        Handles.Label(_mouseTransform.transform.position, "Mouse position");
    }
    #endif
}
