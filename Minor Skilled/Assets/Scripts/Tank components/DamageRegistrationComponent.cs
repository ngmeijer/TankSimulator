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
    private int _maxArmor;
    private int _maxHealth;

    public TankData CurrentData = new TankData();

    private GameObject _lastColliderHit;

    [SerializeField] private GameObject _destroyedTankGFX;
    [SerializeField] private GameObject _functioningTankGFX;
    [SerializeField] private Camera _destroyedTankCamera;
    [SerializeField] private GameObject _deathVFX;

    private void Start()
    {
        _maxHealth = _componentManager.Properties.MaxHealth;
        _maxArmor = _componentManager.Properties.MaxArmor;
        
        CurrentData.OverallHealth = _maxHealth;

        HUDManager.Instance.UpdateMaxHealthForEntity(_componentManager.ID, _maxHealth);
        HUDManager.Instance.UpdateCurrentHealthForEntity(_componentManager.ID, CurrentData.OverallHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Shell")) return;
        if (!collision.collider.TryGetComponent(out Shell shell)) return;
        if (collision.collider.gameObject == _lastColliderHit) return;
        _lastColliderHit = collision.collider.gameObject;

        UpdateHealth(shell.Damage);
    }

    private void UpdateHealth(int damage)
    {
        CurrentData.OverallHealth -= damage;
        HUDManager.Instance.UpdateCurrentHealthForEntity(_componentManager.ID, CurrentData.OverallHealth);
        if (CurrentData.OverallHealth >= 0) return;
        
        OnDeathActions();
    }

    private void OnDeathActions()
    {
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

public class TankData
{
    //Health cannot be regained! (Besides some kind of upgrade/powerup etc?)
    //Armor will first be degraded. Once armor is depleted, damage starts affecting the health
    //If any of the components gets destroyed, you die.
    
    public TankProperties Properties;
    
    //The average of all components health.
    public int OverallHealth;
    
    
    public float TurretHealth;
    
    //
    public float TurretArmor;
}
