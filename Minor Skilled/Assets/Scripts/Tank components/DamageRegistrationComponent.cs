using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum TankParts
{
    Turret,
    LeftTrack,
    RightTrack,
    Hull
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
        
        HUDManager.Instance.UpdateMaxHealthForEntity(_componentManager.ID, _maxHealth);
        HUDManager.Instance.UpdateCurrentHealthForEntity(_componentManager.ID, CurrentData.GetCurrentTotalHealth(), CurrentData.GetMaxTotalHealth());

        Debug.Assert(CurrentData.TankParts.Count != 0, $"Tank parts have not been added in DamageRegistrationComponent, GameObject '{gameObject.name}' ");
        foreach (var part in CurrentData.TankParts)
        {
            part.OnDamageRegistered.AddListener(UpdateGeneralStats);
        }
    }

    private void UpdateGeneralStats()
    {
        HUDManager.Instance.UpdateCurrentHealthForEntity(_componentManager.ID, CurrentData.GetCurrentTotalHealth(), CurrentData.GetMaxTotalHealth());
        HUDManager.Instance.UpdateCurrentArmorForEntity(_componentManager.ID, CurrentData.GetCurrentTotalArmor(), CurrentData.GetMaxTotalArmor());
        if (CurrentData.GetCurrentTotalHealth() > 0) return;
        
        _componentManager.EventManager.OnTankDestruction.Invoke();
    }

    public void ShowUI(bool enabled)
    {
        foreach (var part in CurrentData.TankParts)
        {
            part.EnableCanvas(enabled);
        }
    }
}