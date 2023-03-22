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
    [SerializeField] private AudioSource _audioSource;
    private int _maxShellTypes;
    private int _currentShellIndex;

    public float CurrentDistanceToTarget;
    public float CurrentRange { get; private set; } = 100f;
    public float MinRange { get; } = 0f;

    public bool CanFire { get; private set; } = true;
    public float MaxRange { get; private set; } = 1000f;

    private int _currentAmmoCountForShell;
    public int GetCurrentAmmoCount() => _currentAmmoCountForShell;
    
    private string _currentShellType;
    public string GetCurrentShellType() => _currentShellType;

    private Dictionary<string, int> _ammoCountsPerShellType = new Dictionary<string, int>();
    private RaycastHit currentEstimatedHitpoint;

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

    private void Update()
    {
        CurrentDistanceToTarget = TrackDistance();
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
        Shell shell = shellInstance.GetComponent<Shell>();
        shell.Initialize(_properties.ShellSpeed, _shellSpawnpoint);
        //rb.AddForce(rb.transform.forward * _componentManager.Properties.FireForce);
    }

    private void HandleExplosionFX()
    {
        if (_fireExplosion != null)
        {
            _fireExplosion.transform.SetPositionAndRotation(_VFXPivot.position, _VFXPivot.rotation);
            _fireExplosion.Play();
        }

        if (_audioSource != null)
        {
            _audioSource.Play();
        }
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
    
    private float TrackDistance()
    {
        RaycastHit hit;
        Debug.DrawRay(_shellSpawnpoint.position, _shellSpawnpoint.forward * MaxRange, Color.yellow);
        Debug.DrawRay(_shellSpawnpoint.position, _shellSpawnpoint.forward * CurrentRange, Color.red);
        if (Physics.Raycast(_shellSpawnpoint.position, _shellSpawnpoint.forward,
                out hit, MaxRange))
        {
            currentEstimatedHitpoint = hit;
            return (float)Math.Round(hit.distance, 1);
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

    public void UpdateCurrentRange(float inputValue)
    {
        CurrentRange += inputValue * _properties.RangeChangeSpeed;
        CurrentRange = Mathf.Clamp(CurrentRange, MinRange, MaxRange);
        CurrentRange = (float)Math.Round(CurrentRange, 2);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(currentEstimatedHitpoint.point, 0.75f);
    }
}