using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : SingletonMonobehaviour<HUDManager>
{
    private Dictionary<int, TankComponentManager> _entities;
    private Dictionary<int, HUDUpdater> _hudInstances = new Dictionary<int, HUDUpdater>();
    private Transform _player;

    [Header("Movement")] [SerializeField] private TextMeshProUGUI _tankSpeedText;
    [SerializeField] private TextMeshProUGUI _gearIndexText;
    [SerializeField] private TextMeshProUGUI _rpmText;

    [Header("Shooting")] [SerializeField] private Slider _reloadBar;
    [SerializeField] private TextMeshProUGUI _readyText;
    [SerializeField] private TextMeshProUGUI _ammoCountText;
    [SerializeField] private TextMeshProUGUI _shellTypeText;
    [SerializeField] private float _crosshairOffsetMultiplier = 16.33f;
    [SerializeField] private TextMeshProUGUI _currentRangeText;
    [SerializeField] private TextMeshProUGUI _currentDistanceToTargetText;
    [SerializeField] private RectTransform _crosshairParent;
    [SerializeField] private RectTransform _mildotsContainer;
    [SerializeField] private RectTransform _mildotsBounds;
    private Vector2 _crosshairPosition;
    private Vector2 _mildotsPosition;
    private Vector2 _mildotsDelta;
    

    private void Start()
    {
        _entities = GameManager.Instance.GetEntities();
        _player = GameManager.Instance.GetPlayer();
        _crosshairPosition = _crosshairParent.anchoredPosition;
        _mildotsPosition = new Vector2(_mildotsContainer.anchoredPosition.x, 84);

        foreach (KeyValuePair<int, TankComponentManager> entity in _entities)
        {
            if (CheckIfEntityIsPlayer(entity.Value.transform))
                continue;

            _hudInstances.Add(entity.Key, entity.Value.hudUpdater);
        }
    }

    public void UpdateCurrentHealthForEntity(int id, int newHealth)
    {
        if (_hudInstances.TryGetValue(id, out HUDUpdater hud))
        {
            hud.UpdateHealth(newHealth);
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

    public void UpdateCrosshair(float scrollInput, float currentDistanceToTarget, float currentRange)
    {
        //3rd person
        if (GameManager.Instance.CamMode == CameraMode.ThirdPerson)
        {
            if (scrollInput > 0)
                _crosshairPosition.y += 1 * _crosshairOffsetMultiplier;
            else if (scrollInput < 0)
                _crosshairPosition.y -= 1 * _crosshairOffsetMultiplier;

            float minY = _mildotsBounds.anchoredPosition.y;
            float maxY = minY + _mildotsBounds.sizeDelta.y;
            _crosshairPosition.y = Mathf.Clamp(_crosshairPosition.y, minY, maxY);
            _crosshairParent.anchoredPosition = _crosshairPosition;
        }

        // //ADS
        // if (GameManager.Instance.CamMode == CameraMode.ADS)
        // {
        //     if (scrollInput > 0)
        //         _mildotsPosition.y -= 1 * _crosshairOffsetMultiplier;
        //     else if (scrollInput < 0)
        //         _mildotsPosition.y += 1 * _crosshairOffsetMultiplier;
        //     else return;
        //     _mildotsPosition.y = Mathf.Clamp(_mildotsDelta.y, -430f, 180f);
        //     _mildotsContainer.anchoredPosition = _mildotsPosition;
        // }

        //
        _currentRangeText.SetText($"{currentRange}m");
        _currentDistanceToTargetText.SetText(currentDistanceToTarget != 0 ? $"{currentDistanceToTarget}m" : $"∞");
    }

    public void UpdateCurrentRange(float currentRange)
    {
        _currentRangeText.SetText($"{currentRange}m");
    }
}