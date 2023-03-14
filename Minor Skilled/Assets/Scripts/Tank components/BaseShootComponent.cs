using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BaseShootComponent : TankComponent
{
    [SerializeField] protected Transform _shellSpawnpoint;
    [SerializeField] protected Transform _VFXPivot;
    [SerializeField] protected ParticleSystem _fireExplosion;
    [SerializeField] protected List<Shell> _shellPrefabs;
    protected int _maxShellTypes;
    protected int _currentShellIndex;
    protected float _reloadTime;

    public bool CanFire { get; private set; } = true;
    protected int _currentAmmoCount;
    protected string _currentShellType;
    
    protected virtual void Start()
    {
        _currentAmmoCount = _properties.AmmoCount;
        _reloadTime = _properties.ReloadTime;
        _maxShellTypes = _shellPrefabs.Count;
        _currentShellType = _shellPrefabs[_currentShellIndex].GetShellType();
    }

    public virtual void FireShell()
    {
        InstantiateShell();
        HandleExplosionFX();
        InitiateReloadSequence();

        _componentManager.EventManager.OnShellFired.Invoke(_properties.OnShellFired);
    }

    private void InstantiateShell()
    {
        GameObject shellInstance = Instantiate(_shellPrefabs[_currentShellIndex].gameObject,
            _shellSpawnpoint.position, _shellSpawnpoint.rotation);
        Rigidbody rb = shellInstance.GetComponent<Rigidbody>();
        rb.AddForce(rb.transform.forward * _componentManager.Properties.FireForce);
    }

    private void HandleExplosionFX()
    {
        if (_fireExplosion == null) return;
        
        _fireExplosion.transform.position = _VFXPivot.position;
        _fireExplosion.transform.rotation = _VFXPivot.rotation;
        _fireExplosion.Play();
    }

    private void InitiateReloadSequence()
    {
        CanFire = false;
        _currentAmmoCount--;
        if (_currentAmmoCount > 0)
            StartCoroutine(ReloadCannon());
        else CanFire = false;
    }

    private IEnumerator ReloadCannon()
    {
        yield return new WaitForSeconds(_componentManager.Properties.ReloadTime);

        CanFire = true;
    }
    
    public float TrackDistance()
    {
        RaycastHit hit;
        Debug.DrawRay(_shellSpawnpoint.position, _shellSpawnpoint.forward * 1000,
            Color.red);
        if (Physics.Raycast(_shellSpawnpoint.position, _shellSpawnpoint.forward,
                out hit, Mathf.Infinity))
        {
            return (float)Math.Round(hit.distance, 2);
        }

        return 0;
    }

    public virtual void SwitchShell()
    {
        _currentShellIndex++;
        if (_currentShellIndex > _maxShellTypes - 1)
            _currentShellIndex = 0;
        _currentShellType = _shellPrefabs[_currentShellIndex].GetShellType();
    }
}