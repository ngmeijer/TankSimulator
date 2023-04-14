using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private GameObject _GFX;
    [SerializeField] private Collider _collider;
    [SerializeField] private string _shellType;
    [SerializeField] private float _forceOnImpact = 10000;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private AudioSource _audioSource;
    public Rigidbody RB;
    private bool _vfxHasPlayed;
    public int Damage { get; private set; } = 120;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (_explosion != null)
        {
            if (_explosion.isPlaying) return;
            if (_vfxHasPlayed) return;
            _explosion.Play();
            if (collision.collider.transform.TryGetComponent(out TankPart tankPart))
            {
                tankPart.ReceiveCollisionData(this);
            }
            _explosion.transform.parent = null;
            
            RB.AddExplosionForce(_forceOnImpact, transform.position, _explosionRadius);
            _vfxHasPlayed = true;
            _GFX.SetActive(false);
            _collider.enabled = false;
        }

        if (_audioSource != null)
        {
            _audioSource.Play();
        }
        
        //_GFX.SetActive(false);
        
        _GFX.transform.localPosition = Vector3.zero;
        _GFX.transform.localRotation = Quaternion.identity;
    }

    public string GetShellType() => _shellType;
}
