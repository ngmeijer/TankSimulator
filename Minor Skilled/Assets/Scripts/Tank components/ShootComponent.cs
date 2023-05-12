using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ShootComponent : TankComponent
{
    private const float AIR_DENSITY = 1.2f;
    private const float SHELL_FRONTAL_AREA = 0.035f;
    private const float DRAG_COEFFICIENT = 0.1f;

    [SerializeField] protected Transform _shellSpawnpoint;
    [SerializeField] protected ParticleSystem _fireExplosion;
    [SerializeField] protected List<Shell> _shellPrefabs;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Transform _rayCaster;

    public Transform Raycaster
    {
        get { return _rayCaster; }
    }
    private List<Shell> _firedShells = new();
    private int _maxShellTypes;
    private int _currentShellIndex;
    private HUDCombatState _hudCombatState;

    public float CurrentRange;
    public float RangePercent { get; private set; }
    public float MinRange { get; } = 0f;
    public bool IsReloading;
    public bool CanFire = true;
    public float MaxRange { get; private set; } = 1000f;
    private int _currentAmmoCountForShell;
    public int GetCurrentAmmoCount() => _currentAmmoCountForShell;

    private string _currentShellType;
    public string GetCurrentShellType() => _currentShellType;
    
    private Dictionary<string, int> _ammoCountsPerShellType = new Dictionary<string, int>();

    protected override void Awake()
    {
        base.Awake();

        CurrentRange = 100f;

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

    private void Start()
    {
        _hudCombatState = HUDStateSwitcher.Instance.HUDCombatState as HUDCombatState;
        
        _hudCombatState.UpdateAmmoCount(GetCurrentAmmoCount());
        _hudCombatState.UpdateShellTypeUI(GetCurrentShellType());
    }

    private void Update()
    {
        RangePercent = CurrentRange / MaxRange;
        
        UpdateShellsVelocity();
    }

    public virtual void FireShell()
    {
        if (_currentAmmoCountForShell <= 0)
        {
            CanFire = false;
            return;
        }
        _firedShells.Clear();
        InstantiateShell();
        HandleCannonFireFX();
        InitiateReloadSequence();

        _componentManager.EventManager.OnShellFired.Invoke(_properties.OnShellFired);
    }

    private void UpdateShellsVelocity()
    {
        foreach (Shell currentShell in _firedShells)
        {
            double deceleration = GetDeceleration(currentShell.RB) * Time.deltaTime;
            currentShell.RB.velocity -= currentShell.RB.velocity.normalized * (float)deceleration;
        }
    }

    private static double CalculateDragForce(Rigidbody rb) => DRAG_COEFFICIENT * 0.5f * AIR_DENSITY * Mathf.Pow(rb.velocity.magnitude, 2) * SHELL_FRONTAL_AREA;

    private double GetDeceleration(Rigidbody rb)
    {
        double dragForce = CalculateDragForce(rb);
        Vector3 currentVelocity = rb.velocity;
        Vector3 inverseVelocity = currentVelocity.normalized * -1 * (float)dragForce;
        double deceleration = currentVelocity.magnitude - inverseVelocity.magnitude;

        deceleration /= rb.mass;

        return deceleration;
    }


    private void InstantiateShell()
    {
        GameObject shellInstance = Instantiate(_shellPrefabs[_currentShellIndex].gameObject,
            _shellSpawnpoint.position, _shellSpawnpoint.rotation, GameManager.Instance.GetShellParent());
        Shell shell = shellInstance.GetComponent<Shell>();
        _firedShells.Add(shell);
        shell.RB.velocity = shell.transform.forward * _properties.ShellSpeed;
    }

    private void HandleCannonFireFX()
    {
        if (_fireExplosion != null)
        {
            //_fireExplosion.transform.SetPositionAndRotation(_VFXPivot.position, _VFXPivot.rotation);
            //_fireExplosion.Play();
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
        IsReloading = true;
        yield return new WaitForSeconds(_componentManager.Properties.ReloadTime);

        IsReloading = false;
        CanFire = true;
    }

    public virtual void SwitchShell()
    {
        _currentShellIndex++;
        if (_currentShellIndex > _maxShellTypes - 1)
            _currentShellIndex = 0;
        _currentShellType = _shellPrefabs[_currentShellIndex].GetShellType();

        _currentAmmoCountForShell = _ammoCountsPerShellType[_currentShellType];
    }

    public void UpdateCurrentRange(float inputValue, float multiplier)
    {
        inputValue = Mathf.Clamp(inputValue, -1, 1);
        CurrentRange += inputValue * multiplier;
        CurrentRange = Mathf.Clamp(CurrentRange, MinRange, MaxRange);
        CurrentRange = (float)Math.Round(CurrentRange, 2);
        
        GameManager.Instance.BarrelRotationValue = CurrentRange / MaxRange;
    }

    public int GetShellCount()
    {
        _ammoCountsPerShellType.TryGetValue(_currentShellType, out _currentAmmoCountForShell);

        return _currentAmmoCountForShell;
    }
}