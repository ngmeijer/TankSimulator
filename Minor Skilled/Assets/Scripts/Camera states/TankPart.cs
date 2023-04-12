using System;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private GameObject Canvas;
    [SerializeField] private  Slider HealthSlider;
    [SerializeField] private  Slider ArmorSlider;

    [SerializeField] private TextMeshProUGUI _currentArmorText;
    [SerializeField] private TextMeshProUGUI _maxArmorText;
    
    [SerializeField] private TextMeshProUGUI _currentHealthText;
    [SerializeField] private TextMeshProUGUI _maxHealthText;

    public int MaxHealth;
    public int MaxArmor;

    public int CurrentHealth;
    public int CurrentArmor;

    [HideInInspector] public UnityEvent OnDamageRegistered = new UnityEvent();
    [HideInInspector] public UnityEvent OnZeroHealthReached = new UnityEvent();

    public List<GameObject> _vfxLevels;

    private Vector3 _hitpoint;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        CurrentArmor = MaxArmor;
        PartState = PartState.Good;
        EnableCanvas(false);
        UpdateUI();
    }

    public void EnableCanvas(bool enabled) => Canvas.SetActive(enabled);

    public void ReceiveCollisionData(Shell shellData)
    {
        _hitpoint = shellData.Hitpoint;
        CalculateNewStats(shellData);

        if (CurrentHealth <= 0)
        {
            OnZeroHealthReached.Invoke();
            EnableCanvas(false);
        }

        UpdateUI();
        
        CheckVFXRequirement(CurrentHealth, MaxHealth);
        OnDamageRegistered.Invoke();
    }

    private void UpdateUI()
    {
        HealthSlider.value = (float)CurrentHealth / MaxHealth;
        ArmorSlider.value = (float)CurrentArmor / MaxArmor;
        
        _currentHealthText.SetText(CurrentHealth.ToString());
        _maxHealthText.SetText(MaxHealth.ToString());
        
        _currentArmorText.SetText(CurrentArmor.ToString());
        _maxArmorText.SetText(MaxArmor.ToString());
    }

        private void CalculateNewStats(Shell shellData)
    {
        if (CurrentArmor > 0)
        {
            int newArmor = CurrentArmor - shellData.Damage;
            CurrentArmor = newArmor;
            if (newArmor <= 0)
            {
                CurrentArmor = 0;
                CurrentHealth -= Mathf.Abs(newArmor);
            }
        }
        else
        {
            CurrentHealth -= shellData.Damage;
        }
    }

    private void CheckVFXRequirement(float currentHealth, float maxHealth)
    {
        float inverseHP = 1 - (currentHealth / maxHealth);
        if (inverseHP == 0) return;
        
        float selectedVFX = inverseHP * (_vfxLevels.Count - 1);
        selectedVFX = Mathf.Floor(selectedVFX);

        for (int i = 0; i < selectedVFX; i++) 
        {
            _vfxLevels[i].SetActive(true);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(_hitpoint, 0.2f);
    }
}