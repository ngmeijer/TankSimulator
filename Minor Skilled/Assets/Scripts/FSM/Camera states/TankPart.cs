using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _armorSlider;
    [SerializeField] private TextMeshProUGUI _stateText;

    [SerializeField] private TextMeshProUGUI _currentArmorText;
    [SerializeField] private TextMeshProUGUI _maxArmorText;
    
    [SerializeField] private TextMeshProUGUI _currentHealthText;
    [SerializeField] private TextMeshProUGUI _maxHealthText;

    [SerializeField] private List<GameObject> _vfxLevels;
    [SerializeField] private float _canvasTweenY = 2f;
    [SerializeField] private float _tweenSpeed = 1f;

    [SerializeField] private int _repairPerSecond = 5;
    
    public int MaxHealth;
    public int MaxArmor;

    public int CurrentHealth;
    public int CurrentArmor;

    [HideInInspector] public UnityEvent OnDamageRegistered = new UnityEvent();
    [HideInInspector] public UnityEvent OnZeroHealthReached = new UnityEvent();
    
    private Vector3 _canvasStartPos;
    private Vector3 _canvasHidePos;
    private Vector3 _canvasStartScale;
    private int _explosionActivationOffset;
    
    private void Awake()
    {
        CurrentHealth = MaxHealth;
        CurrentArmor = MaxArmor;
        SetStateText(PartState.Good);
        _canvasStartPos = _propertiesCanvas.transform.position;
        _canvasHidePos = _canvasStartPos;
        _canvasHidePos.y -= _canvasTweenY;
        _canvasStartScale = _propertiesCanvas.transform.localScale;

        _explosionActivationOffset = (int)0.2f * MaxHealth;
        
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
        if(_healthSlider != null)
            _healthSlider.value = (float)CurrentHealth / MaxHealth;
        if(_armorSlider != null)
            _armorSlider.value = (float)CurrentArmor / MaxArmor;
        
        if(_currentHealthText != null)
            _currentHealthText.SetText(CurrentHealth.ToString());
        
        if(_maxHealthText != null)
            _maxHealthText.SetText(MaxHealth.ToString());
        
        if(_currentArmorText != null)
            _currentArmorText.SetText(CurrentArmor.ToString());
       
        if(_maxArmorText != null)
            _maxArmorText.SetText(MaxArmor.ToString());
    }

    private void CalculateNewStats(Shell shellData)
    {
        if (CurrentArmor > 0)
        {
            int newArmor = CurrentArmor - shellData.Damage;
            CurrentArmor = newArmor;
            SetStateText(PartState.Caution);
            
            if (newArmor > 0) return;
            CurrentArmor = 0;
            CurrentHealth -= Mathf.Abs(newArmor);
        }
        else
        {
            SetStateText(PartState.Critical);
            CurrentHealth -= shellData.Damage;
        }
    }

    private void SetStateText(PartState state)
    {
        PartState = state;
        Color textColor = Color.black;
        switch (state)
        {
            case PartState.Good:
                textColor = Color.green;
                break;
            case PartState.Caution:
                textColor = Color.yellow;
                break;
            case PartState.Critical:
                textColor = Color.red;
                break;
            case PartState.Destroyed:
                textColor = Color.black;
                break;
        }

        if (_stateText != null)
        {
            _stateText.SetText($"{PartState}");
            _stateText.color = textColor;
        }
    }

    private void ActivateNewVFX(float currentHealth, float maxHealth)
    {
        float inverseHP = 1 - (currentHealth / maxHealth);
        if (inverseHP == 0) return;
        
        float selectedVFX = inverseHP * (_vfxLevels.Count) + _explosionActivationOffset;
        selectedVFX = Mathf.FloorToInt(selectedVFX);
        
        for (int i = 0; i < selectedVFX; i++)
        {
            _vfxLevels[i].SetActive(true);
        }
    }

    public void RepairPart()
    {
        CurrentArmor += _repairPerSecond;
        SetStateText(PartState.Good);
        UpdateUI();
        ActivateNewVFX(CurrentHealth, MaxHealth);
    }

    public void TestDamage(int damage)
    {
        if (CurrentArmor > 0)
        {
            int newArmor = CurrentArmor - damage;
            CurrentArmor = newArmor;
            SetStateText(PartState.Caution);
                
            if (newArmor < 0)
            {
                CurrentArmor = 0;
                CurrentHealth -= Mathf.Abs(newArmor);
            }
        }
        else
        {
            SetStateText(PartState.Critical);
            CurrentHealth -= damage;
        }
        
        UpdateUI();
        ActivateNewVFX(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0)
        {
            OnZeroHealthReached.Invoke();
        }
    }
}