using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    private const float GRAVITY = 9.81f;
    
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private GameObject _GFX;
    [SerializeField] private Collider _collider;
    [SerializeField] private string _shellType;
    [SerializeField] private float _forceOnImpact = 10000;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private AudioSource _audioSource;
    private bool _vfxHasPlayed;
    public int Damage { get; private set; } = 50;

    private float _speed;
    private Vector3 _startPos;
    private Vector3 _startForward;
    private bool _isInitialized;
    private float _time = -1;

    private void FixedUpdate()
    {
        if (!_isInitialized) return;
        if (_time < 0) _time = Time.time;

        float currentTime = Time.time - _time;
        float nextTime = currentTime + Time.fixedDeltaTime;

        Vector3 currentPoint = FindPointOnParabole(currentTime);
        Vector3 nextPoint = FindPointOnParabole(nextTime);

        if (CastRayBetweenCurrentPoints(currentPoint, nextPoint, out RaycastHit hit))
        {
            OnHitCollider(hit);
        }
    }

    private void Update()
    {
        if (!_isInitialized) return;
        
        float currentTime = Time.time - _time;
        Vector3 currentPoint = FindPointOnParabole(currentTime);
        transform.position = currentPoint;
    }

    public void Initialize(float speed, Transform startPos)
    {
        _startPos = startPos.position;
        _startForward = startPos.forward.normalized;
        _speed = speed;
        _isInitialized = true;
    }

    private Vector3 FindPointOnParabole(float time)
    {
        Vector3 currentPoint = _startPos + (_startForward * (_speed * time));
        Vector3 gravityVec = Vector3.down * (GRAVITY * time * time);

        return currentPoint + gravityVec;
    }

    private bool CastRayBetweenCurrentPoints(Vector3 startPoint, Vector3 endPoint, out RaycastHit hit)
    {
        return Physics.Raycast(startPoint, endPoint, out hit, (endPoint - startPoint).magnitude);
    }

    private void OnHitCollider(RaycastHit hitData)
    {
        if (_explosion != null)
        {
            if (_explosion.isPlaying) return;
            if (_vfxHasPlayed) return;
            _explosion.transform.parent = null;
            _explosion.transform.up = hitData.normal;
            _explosion.transform.position = hitData.point;
            _explosion.Play();
            _vfxHasPlayed = true;
        }

        if (_audioSource != null)
        {
            _audioSource.Play();
        }
        
        _GFX.SetActive(false);
        _collider.enabled = false;
        
        _GFX.transform.localPosition = Vector3.zero;
        _GFX.transform.localRotation = Quaternion.identity;
    }

    public string GetShellType() => _shellType;
}
