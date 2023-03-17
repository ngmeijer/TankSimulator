using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ShootComponent : TankComponent
{
    [SerializeField] protected Transform _shellSpawnpoint;
    [SerializeField] protected Transform _VFXPivot;
    [SerializeField] protected ParticleSystem _fireExplosion;
    [SerializeField] protected List<Shell> _shellPrefabs;
    private int _maxShellTypes;
    private int _currentShellIndex;

    public bool CanFire { get; private set; } = true;
    private int _currentAmmoCountForShell;
    public int GetCurrentAmmoCount() => _currentAmmoCountForShell;
    
    private string _currentShellType;
    public string GetCurrentShellType() => _currentShellType;

    private Dictionary<string, int> _ammoCountsPerShellType = new Dictionary<string, int>();

    protected override void Awake()
    {
        base.Awake();
        
        _maxShellTypes = _shellPrefabs.Count;
        _currentShellType = _shellPrefabs[_currentShellIndex].GetShellType();

        foreach (Shell shell in _shellPrefabs)
        {
            string shellType = shell.GetShellType();
            int ammoCount = 0;
            switch (shellType)
            {
                case "AP (Armor Penetration)":
                    ammoCount = _properties.APAmmo;
                    break;
                case "HE (High Explosive)":
                    ammoCount = _properties.HEAmmo;
                    break;
                case "HEAT (High Explosive Anti Tank)":
                    ammoCount = _properties.HEATAmmo;
                    break;
            }
            _ammoCountsPerShellType.Add(shell.GetShellType(), ammoCount);   
        }

        _ammoCountsPerShellType.TryGetValue(_currentShellType, out _currentAmmoCountForShell);
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
            _shellSpawnpoint.position, _shellSpawnpoint.rotation, GameManager.Instance.GetShellParent());
        Rigidbody rb = shellInstance.GetComponent<Rigidbody>();
        rb.AddForce(rb.transform.forward * _componentManager.Properties.FireForce);
    }

    private void HandleExplosionFX()
    {
        if (_fireExplosion == null) return;
        
        _fireExplosion.transform.SetPositionAndRotation(_VFXPivot.position, _VFXPivot.rotation);
        _fireExplosion.Play();
    }

    private void InitiateReloadSequence()
    {
        CanFire = false;
        _currentAmmoCountForShell--;
        _ammoCountsPerShellType[_currentShellType] = _currentAmmoCountForShell;
        if (_currentAmmoCountForShell > 0)
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

        _currentAmmoCountForShell = _ammoCountsPerShellType[_currentShellType];
    }
}