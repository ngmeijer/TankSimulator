using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TankParts
{
    Turret,
    LeftTrack,
    RightTrack,
    HullLeft,
    HullRight,
    HullFront,
    HullBack
}

public class DamageRegistrationComponent : TankComponent
{
    private int _health;
    private int _armor;

    private int _maxArmor;
    private int _maxHealth;

    private GameObject _lastColliderHit;

    [SerializeField] private GameObject _destroyedTankGFX;
    [SerializeField] private GameObject _functioningTankGFX;
    [SerializeField] private Camera _destroyedTankCamera;
    [SerializeField] private GameObject _deathVFX;

    private void Start()
    {
        _maxHealth = _componentManager.Properties.MaxHealth;
        _maxArmor = _componentManager.Properties.MaxArmor;
        
        _health = _maxHealth;
        _armor = _maxArmor;
        
        _componentManager.EntityHUD.SetMaxHealth(_maxHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Shell")) return;
        if (!collision.collider.TryGetComponent(out Shell shell)) return;
        if (collision.collider.gameObject == _lastColliderHit) return;
        _lastColliderHit = collision.collider.gameObject;

        UpdateHealth(shell.GetDamage());
    }

    private void UpdateHealth(int damage)
    {
        _health -= damage;
        _componentManager.EntityHUD.UpdateHealth(_health);
        if (_health >= 0) return;

        _componentManager.EventManager.OnTankComponentHit.Invoke(_properties.OnEntityDeath);
        _componentManager.HasDied = true;
        _functioningTankGFX.SetActive(false);
        _destroyedTankGFX.SetActive(true);
        if(_destroyedTankCamera != null)
            _destroyedTankCamera.gameObject.SetActive(true);
        _deathVFX.SetActive(true);
        _componentManager.EventManager.OnEntityDeath?.Invoke(_componentManager);
    }
}
