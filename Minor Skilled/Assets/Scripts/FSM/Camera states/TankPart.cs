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

    [SerializeField] private bool _lookAtCamera;
    
    public int MaxHealth;
    public int MaxArmor;

    public int CurrentHealth;
    public float CurrentArmor;

    [SerializeField] private bool _debugMode;
    private bool _repairing;
    public void IsRepairing(bool isTrue) => _repairing = isTrue;
    
    [HideInInspector] public UnityEvent OnDamageRegistered = new UnityEvent();
    [HideInInspector] public UnityEvent OnZeroHealthReached = new UnityEvent();
    
    private Vector3 _canvasStartPos;
    private Vector3 _canvasHidePos;
    private Vector3 _canvasStartScale;
    private int _explosionActivationOffset;
    
    private void Awake()
    {
        if (!_debugMode)
        {
            CurrentHealth = MaxHealth;
            CurrentArmor = MaxArmor;
        }

        SetStateText(PartState.Good);
        _canvasStartPos = _propertiesCanvas.transform.position;
        _canvasHidePos = _canvasStartPos;
        _canvasHidePos.y -= _canvasTweenY;
        _canvasStartScale = _propertiesCanvas.transform.localScale;

        _explosionActivationOffset = (int)0.2f * MaxHealth;
        
        _propertiesCanvas.SetActive(false);
        foreach (var vfx in _vfxLevels)
        {
            vfx.SetActive(false);
        }
        EnableCanvas(false);
        UpdateUI();
    }

    private void Update()
    {
        if(_lookAtCamera)
            _propertiesCanvas.transform.LookAt(GameManager.Instance.InspectionCameraTrans);
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
            _currentArmorText.SetText(CurrentArmor.ToString("f0"));
       
        if(_maxArmorText != null)
            _maxArmorText.SetText(MaxArmor.ToString());
        
        if(CurrentArmor >= MaxArmor && CurrentHealth == MaxHealth)
            SetStateText(PartState.Good);
        else if (CurrentArmor < MaxArmor)
            SetStateText(PartState.Caution);
        
        if(CurrentHealth < MaxHealth)
            SetStateText(PartState.Critical);
    }

    private void CalculateNewStats(Shell shellData)
    {
        if (CurrentArmor > 0)
        {
            int newArmor = (int)CurrentArmor - shellData.Damage;
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
            if (i >= _vfxLevels.Count) return;
            _vfxLevels[i].SetActive(true);
        }
    }

    public void RepairPart()
    {
        var increase=_repairPerSecond * Time.deltaTime;
        CurrentArmor += increase;
        
        CurrentArmor = Mathf.Clamp(CurrentArmor, 0, MaxArmor);
        UpdateUI();
        ActivateNewVFX(CurrentHealth, MaxHealth);
    }
}