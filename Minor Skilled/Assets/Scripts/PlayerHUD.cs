using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : BaseHUDManager
{
    [SerializeField] private Slider _reloadIndicator;
    [SerializeField] private TextMeshProUGUI _distanceIndicator;
    [SerializeField] private TextMeshProUGUI _tankSpeedIndicator;
    [SerializeField] private TextMeshProUGUI _gearIndicator;
    [SerializeField] private TextMeshProUGUI _rpmIndicator;
    [SerializeField] private TextMeshProUGUI _torqueIndicator;
    [SerializeField] private TextMeshProUGUI _ammoCountIndicator;
    [SerializeField] private TextMeshProUGUI _shellTypeIndicator;
    [SerializeField] private GameObject _turretRotationUI;
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private float _minYCrosshair;
    [SerializeField] private float _maxYCrosshair;

    [SerializeField] private TextMeshProUGUI _rpmCalculationText;
    [SerializeField] private TextMeshProUGUI _calculationText;
    [SerializeField] private GameObject _HUDContainer;
    [SerializeField] private GameObject _gameOverScreen;

    public IEnumerator UpdateReloadUI(float reloadTime)
    {
        float currentTime = 0f;
        while (currentTime < reloadTime)
        {
            currentTime += Time.deltaTime;
            float lerpValue = currentTime / reloadTime;
            _reloadIndicator.value = Mathf.Lerp(_reloadIndicator.minValue, _reloadIndicator.maxValue, lerpValue);
            yield return null;
        }
    }

    public override void UpdateName(string name)
    {
        
    }

    public override void SetMaxHealth(int maxHealth)
    {
        base.SetMaxHealth(maxHealth);
        _healthIndicator.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHealth);
    }

    public override void UpdateSpeed(float speed)
    {
        _tankSpeedIndicator.SetText($"{speed}");
    }

    public override void UpdateGearData(int gear, int rpm, int torque)
    {
        _gearIndicator.SetText($"{gear}");
        _rpmIndicator.SetText($"{rpm}");
        _torqueIndicator.SetText($"{torque}");
    }

    public override void UpdateCalculationText(string text)
    {
        _calculationText.SetText(text);
    }

    public override void UpdateWheelRPMCalculation(string text)
    {
        _rpmCalculationText.SetText(text);
    }

    public void UpdateCrosshairYPosition(float offsetY)
    {
        Vector3 tempPos = _crosshair.transform.localPosition;
        tempPos.y += offsetY;
        Debug.Log(tempPos.y);
        tempPos.y = Mathf.Clamp(tempPos.y, _minYCrosshair, _maxYCrosshair);
        _crosshair.transform.localPosition = tempPos;
    }

    public override void UpdateDistanceUI(float trackDistance)
    {
        if (trackDistance == 0)
        {
            _distanceIndicator.SetText("");
            return;
        }
        _distanceIndicator.SetText($"{trackDistance}m");
    }

    public void UpdateTankSpeedUI(float speed)
    {
        _tankSpeedIndicator.SetText($"{speed} km/h");
    }

    public override void UpdateAmmoCountUI(int ammoCount)
    {
        _ammoCountIndicator.SetText($"{ammoCount}");
    }

    public override  void UpdateShellTypeUI(string type)
    {
        _shellTypeIndicator.SetText(type);
    }

    public void SetTurretRotationUI(Vector3 turretRot)
    {
        Quaternion rot = _turretRotationUI.transform.rotation;
        rot.eulerAngles = new Vector3(0,0, -turretRot.y);
        _turretRotationUI.transform.rotation = rot;
    }

    public void OnPlayerKilled()
    {
        Cursor.lockState = CursorLockMode.Confined; 
        _HUDContainer.SetActive(false);
        _gameOverScreen.SetActive(true);
    }
}
