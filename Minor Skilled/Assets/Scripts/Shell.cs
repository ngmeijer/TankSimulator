using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private GameObject _GFX;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _collider;
    [SerializeField] private string _shellType;
    [SerializeField] private float _forceOnImpact = 10000;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private Transform _centerOfMass;
    private bool _vfxHasPlayed;
    private int _damage;
    private Transform _explosionVFXTransform;
    
    private void Start()
    {
        _rb.centerOfMass = _centerOfMass.position;
        if(_explosion != null)
            _explosionVFXTransform = _explosion.transform;
    }

    private void OnCollisionEnter(Collision other)
    {
        _rb.velocity = Vector3.zero;
        if (_explosion != null)
        {
            if (_explosion.isPlaying) return;
            if (_vfxHasPlayed) return;
            _explosion.transform.parent = null;
            _explosionVFXTransform.up = other.contacts[0].normal;
            _explosionVFXTransform.position = other.contacts[0].point;
            _explosion.Play();
            _vfxHasPlayed = true;
        }
        _rb.AddExplosionForce(_forceOnImpact, transform.position, _explosionRadius);
        _GFX.SetActive(false);
        _rb.isKinematic = true;
        _collider.enabled = false;
        
        _GFX.transform.localPosition = Vector3.zero;
        _GFX.transform.localRotation = Quaternion.identity;
    }

    public string GetShellType() => _shellType;

    public int GetDamage() => _damage;
}
