using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : SingletonMonobehaviour<HUDManager>
{
    public const int PLAYER_ID = 0;
    
    private Dictionary<int, TankComponentManager> _entities;
    private Dictionary<int, HUDUpdater> _hudInstances = new Dictionary<int, HUDUpdater>();
    private Transform _player;

    [Header("Movement")] [SerializeField] private GameObject _moveContainer;
    [SerializeField] private TextMeshProUGUI _tankSpeedText;
    [SerializeField] private TextMeshProUGUI _gearIndexText;
    [SerializeField] private TextMeshProUGUI _rpmText;

    [Header("Shooting")] [SerializeField] private GameObject _shootContainer;
    [SerializeField] private Slider _reloadBar;
    [SerializeField] private TextMeshProUGUI _readyText;
    [SerializeField] private TextMeshProUGUI _ammoCountText;
    [SerializeField] private TextMeshProUGUI _shellTypeText;
    [SerializeField] private TextMeshProUGUI _zoomLevelText;
    
    [Header("Crosshair")]
    [SerializeField] private RectTransform _crosshairParent;
    [SerializeField] private RectTransform _currentBarrelCrosshair;
    [SerializeField] private RectTransform _targetBarrelCrosshair;
    [SerializeField] private RectTransform _adsUIPosition;
    [SerializeField] private RectTransform _tpUIPosition;

    [Header("Health properties")] 
    [SerializeField] private HealthStatsUpdater _healthStatsUpdater;
    [SerializeField] private Slider _generalHealth;
    [SerializeField] private Slider _generalArmor;
    
    private void Start()
    {
        _entities = GameManager.Instance.GetEntities();
        _player = GameManager.Instance.GetPlayer();

        foreach (KeyValuePair<int, TankComponentManager> entity in _entities)
        {
            if (CheckIfEntityIsPlayer(entity.Value.transform))
                continue;

            _hudInstances.Add(entity.Key, entity.Value.hudUpdater);
        }
        
        EnableCombatUI(true);
    }

    public void UpdateCurrentHealthForEntity(int id, float currentHealth, float maxHealth)
    {
        if (id == PLAYER_ID)
        {
            _generalHealth.value = currentHealth / maxHealth;
            return;
        }
        
        if (_hudInstances.TryGetValue(id, out HUDUpdater hud))
        {
            hud.UpdateHealth((int)currentHealth);
        }
    }
    
    public void UpdateCurrentArmorForEntity(int id, float currentArmor, float maxArmor)
    {
        if (id == PLAYER_ID)
        {
            _generalArmor.value = currentArmor / maxArmor;
            return;
        }
        
        if (_hudInstances.TryGetValue(id, out HUDUpdater hud))
        {
            
        }
    }

    public void UpdateMaxHealthForEntity(int id, int maxHealth)
    {
        if (_hudInstances.TryGetValue(id, out HUDUpdater hud))
        {
            hud.SetMaxHealth(maxHealth);
        }
    }

    public void UpdateEntityName(int id, string tankName)
    {
        if (_hudInstances.TryGetValue(id, out HUDUpdater hud))
        {
            hud.SetName(tankName);
        }
    }

    private bool CheckIfEntityIsPlayer(Transform entity) => entity == _player;

    public void UpdateGearboxData(MovementData data)
    {
        _tankSpeedText.SetText($"{data.Velocity}");
        _gearIndexText.SetText($"{data.GearIndex}");
        _rpmText.SetText($"{data.RPM}");
    }

    public IEnumerator UpdateReloadUI(float reloadTime)
    {
        _reloadBar.gameObject.SetActive(true);
        _readyText.gameObject.SetActive(false);

        float currentTime = 0f;
        while (currentTime < reloadTime)
        {
            currentTime += Time.deltaTime;
            float lerpValue = currentTime / reloadTime;
            _reloadBar.value = Mathf.Lerp(_reloadBar.minValue, _reloadBar.maxValue, lerpValue);
            yield return null;
        }

        _readyText.gameObject.SetActive(true);
        _reloadBar.gameObject.SetActive(false);
    }

    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoCountText.SetText($"{ammoCount}");
    }

    public void UpdateShellTypeUI(string shellType)
    {
        _shellTypeText.SetText($"{shellType}");
    }

    public void UpdateCrosshair()
    {
        _currentBarrelCrosshair.position = GameManager.Instance.CurrentBarrelCrosshairPos;
        _targetBarrelCrosshair.position = GameManager.Instance.TargetBarrelCrosshairPos;
    }

    public void HandleCamModeUI(E_CameraState camState)
    {
        switch (camState)
        {
            case E_CameraState.ADS:
                _crosshairParent.anchoredPosition = _adsUIPosition.anchoredPosition;
                _zoomLevelText.gameObject.SetActive(true);
                break;
            case E_CameraState.ThirdPerson:
                _crosshairParent.anchoredPosition = _tpUIPosition.anchoredPosition;
                _zoomLevelText.gameObject.SetActive(false);
                break;
        }
    }

    public void SetZoomLevelText(int currentCameraFOVLevel)
    {
        _zoomLevelText.SetText($"{currentCameraFOVLevel}x");
    }

    public void EnableCombatUI(bool enabled)
    {
        _moveContainer.SetActive(enabled);
        _shootContainer.SetActive(enabled);
    }
}