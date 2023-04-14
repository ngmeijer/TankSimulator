using System;
using System.Collections.Generic;
using DG.Tweening;
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
    public PartState PartState;

    [SerializeField] private GameObject _propertiesCanvas;
    [SerializeField] private  Slider HealthSlider;
    [SerializeField] private  Slider ArmorSlider;

    [SerializeField] private TextMeshProUGUI _currentArmorText;
    [SerializeField] private TextMeshProUGUI _maxArmorText;
    
    [SerializeField] private TextMeshProUGUI _currentHealthText;
    [SerializeField] private TextMeshProUGUI _maxHealthText;

    [SerializeField] private List<GameObject> _vfxLevels;
    [SerializeField] private float _canvasTweenY = 2f;
    [SerializeField] private float _tweenSpeed = 1f;
    
    public int MaxHealth;
    public int MaxArmor;

    public int CurrentHealth;
    public int CurrentArmor;

    [HideInInspector] public UnityEvent OnDamageRegistered = new UnityEvent();
    [HideInInspector] public UnityEvent OnZeroHealthReached = new UnityEvent();
    
    private Vector3 _canvasStartPos;
    private Vector3 _canvasHidePos;
    private Vector3 _canvasStartScale;
    
    private void Awake()
    {
        CurrentHealth = MaxHealth;
        CurrentArmor = MaxArmor;
        PartState = PartState.Good;
        _canvasStartPos = _propertiesCanvas.transform.position;
        _canvasHidePos = _canvasStartPos;
        _canvasHidePos.y -= _canvasTweenY;
        _canvasStartScale = _propertiesCanvas.transform.localScale;
        
        _propertiesCanvas.SetActive(false);
        EnableCanvas(false);
        UpdateUI();
    }

    public void EnableCanvas(bool enabled)
    {
        if (enabled)
        {
            _propertiesCanvas.SetActive(true);
            _propertiesCanvas.transform.DOScale(_canvasStartScale, _tweenSpeed);
            _propertiesCanvas.transform.DOMoveY(_propertiesCanvas.transform.position.y + _canvasTweenY, _tweenSpeed);
        }
        else
        {
            _propertiesCanvas.transform.DOScale(Vector3.zero, _tweenSpeed);
            _propertiesCanvas.transform.DOMoveY(_canvasHidePos.y, _tweenSpeed)
            .OnComplete(() => _propertiesCanvas.SetActive(false));
        }
    }

    public void ReceiveCollisionData(Shell shellData)
    {
        CalculateNewStats(shellData);

        if (CurrentHealth <= 0)
        {
            OnZeroHealthReached.Invoke();
            EnableCanvas(false);
        }

        UpdateUI();
        
        ActivateNewVFX(CurrentHealth, MaxHealth);
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
            
            if (newArmor > 0) return;
            CurrentArmor = 0;
            CurrentHealth -= Mathf.Abs(newArmor);
        }
        else
        {
            CurrentHealth -= shellData.Damage;
        }
    }

    private void ActivateNewVFX(float currentHealth, float maxHealth)
    {
        //Still working on this
        float inverseHP = 1 - (currentHealth / maxHealth);
        if (inverseHP == 0) return;
        
        float selectedVFX = inverseHP * (_vfxLevels.Count - 1);
        selectedVFX = Mathf.Floor(selectedVFX);

        for (int i = 0; i < selectedVFX; i++) 
        {
            _vfxLevels[i].SetActive(true);
        }
    }
}