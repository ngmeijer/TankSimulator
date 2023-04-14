using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : SingletonMonobehaviour<HUDManager>
{
    private Dictionary<int, TankComponentManager> _entities;

    [SerializeField] private GameObject _combatHUD;
    [SerializeField] private GameObject _menuHUD;
    [SerializeField] private Transform _targetsIndicatorParent;
    [SerializeField] private TextMeshProUGUI _inspectHostileText;

    [Header("Movement")] 
    [SerializeField] private TextMeshProUGUI _tankSpeedText;
    [SerializeField] private TextMeshProUGUI _gearIndexText;
    [SerializeField] private TextMeshProUGUI _rpmText;

    [Header("Shooting")]
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
    [SerializeField] private Slider _generalHealth;
    [SerializeField] private Slider _generalArmor;

    [SerializeField] private GameObject _enemyIndicatorPrefab;
    private Dictionary<int, GameObject> _enemyIndicators = new();

    private void Start()
    {
        _entities = GameManager.Instance.GetEntities();
        
        EnableCombatUI(true);
        EnableMenuUI(false);
        EnableInspectHostileText(false);
        SetZoomLevelText(0, false);
        CreateEnemyIndicators();
    }

    private void CreateEnemyIndicators()
    {
        foreach (KeyValuePair<int, TankComponentManager> entity in _entities)
        {
            if (entity.Key == GameManager.PLAYER_ID) continue;
            
            GameObject indicator = Instantiate(_enemyIndicatorPrefab, _targetsIndicatorParent);
            _enemyIndicators.Add(entity.Key, indicator);
        }
    }

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

    public void SetEnemyIndicator(int enemyIndex, Vector2 enemyPosition, bool inSight)
    {
        _enemyIndicators.TryGetValue(enemyIndex, out GameObject indicator);
        if (indicator == null) return;
        
        indicator.transform.position = enemyPosition;
        if (inSight)
        {
            if (indicator.activeInHierarchy) return;
            indicator.SetActive(true);
        }
        else indicator.SetActive(false);
    }

    public void HandleCamModeUI(E_CameraState camState)
    {
        switch (camState)
        {
            case E_CameraState.ADS:
                _crosshairParent.anchoredPosition = _adsUIPosition.anchoredPosition;
                break;
            case E_CameraState.ThirdPerson:
                _crosshairParent.anchoredPosition = _tpUIPosition.anchoredPosition;
                break;
        }
    }

    public void SetZoomLevelText(int currentCameraFOVLevel, bool enabled)
    {
        _zoomLevelText.SetText($"{currentCameraFOVLevel}x");
        _zoomLevelText.gameObject.SetActive(enabled);
    }

    public void EnableCombatUI(bool enabled)
    {
        _combatHUD.SetActive(enabled); 
    }

    public void EnableMenuUI(bool enabled)
    {
        _menuHUD.SetActive(enabled);
    }

    public void DestroyEntityIndicator(int entityID)
    {
        _enemyIndicators.TryGetValue(entityID, out GameObject indicator);
        _enemyIndicators.Remove(entityID);
        Destroy(indicator);
    }

    public void EnableInspectHostileText(bool enabled)
    {
        if (_inspectHostileText.gameObject.activeInHierarchy == enabled) return;
        
        _inspectHostileText.gameObject.SetActive(enabled);
    }
}