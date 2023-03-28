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
    public Rigidbody RB;
    private bool _vfxHasPlayed;
    public int Damage { get; private set; } = 50;

    private float _speed;
    private Vector3 _startPos;
    private Vector3 _startForward;
    private bool _isInitialized;
    private float _time = -1;

    private void OnCollisionEnter(Collision collision)
    {
        if (_explosion != null)
        {
            if (_explosion.isPlaying) return;
            if (_vfxHasPlayed) return;
            _explosion.transform.parent = null;
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

    private void OnHitCollider(RaycastHit hitData)
    {
        
    }

    public string GetShellType() => _shellType;
}
