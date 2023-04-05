using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum PartState
{
    Good,
    Caution,
    Critical,
    Destroyed
}

public class TankPart : MonoBehaviour
{
    public TankParts ThisPart;
    public PartState PartState;

    public GameObject Canvas;
    public Slider HealthSlider;
    public Slider ArmorSlider;

    public int MaxHealth;
    public int MaxArmor;

    public int CurrentHealth;
    public int CurrentArmor;

    public UnityEvent OnDamageRegistered = new UnityEvent();

    public List<GameObject> _vfxLevels;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        CurrentArmor = MaxArmor;
        PartState = PartState.Good;
        EnableCanvas(false);
    }

    public void EnableCanvas(bool enabled) => Canvas.SetActive(enabled);

    public void ReceiveCollisionData(Shell shellData)
    {
        if (CurrentArmor > 0)
        {
            int newArmor = CurrentArmor - shellData.Damage;
            CurrentArmor = newArmor;
            if (newArmor < 0)
            {
                CurrentArmor = 0;
                CurrentHealth -= newArmor;
            }
        }
        else
        {
            CurrentHealth -= shellData.Damage;
        }

        HealthSlider.value = (float)CurrentHealth / MaxHealth;
        ArmorSlider.value = (float)CurrentArmor / MaxArmor;
        
        CheckVFXRequirement(CurrentHealth, MaxHealth);
        OnDamageRegistered.Invoke();
    }

    private void CheckVFXRequirement(float currentHealth, float maxHealth)
    {
        float healthPercentage = currentHealth / maxHealth;
        float vfxActivationInterval = 1f / _vfxLevels.Count;

        for (float i = _vfxLevels.Count; i == 0; i--)
        {
            float totalVFXInterval = i * vfxActivationInterval;
            if (healthPercentage <= totalVFXInterval)
            {
                _vfxLevels[(int)i - 1].SetActive(true);
            }
        }
    }
}