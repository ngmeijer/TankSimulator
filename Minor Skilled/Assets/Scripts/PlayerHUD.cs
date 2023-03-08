using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : BaseHUDManager
{
    [SerializeField] private Slider reloadIndicator;
    [SerializeField] private TextMeshProUGUI distanceIndicator;
    [SerializeField] private TextMeshProUGUI tankSpeedIndicator;
    [SerializeField] private TextMeshProUGUI ammoCountIndicator;
    [SerializeField] private TextMeshProUGUI shellTypeIndicator;
    [SerializeField] private GameObject turretRotationUI;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private float minYCrosshair;
    [SerializeField] private float maxYCrosshair;

    [SerializeField] private GameObject HUDContainer;
    [SerializeField] private GameObject gameOverScreen;

    public IEnumerator UpdateReloadUI(float reloadTime)
    {
        float currentTime = 0f;
        while (currentTime < reloadTime)
        {
            currentTime += Time.deltaTime;
            float lerpValue = currentTime / reloadTime;
            reloadIndicator.value = Mathf.Lerp(reloadIndicator.minValue, reloadIndicator.maxValue, lerpValue);
            yield return null;
        }
    }

    public override void SetMaxHealth(int maxHealth)
    {
        base.SetMaxHealth(maxHealth);
        healthIndicator.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHealth);
    }

    public void UpdateCrosshairYPosition(float offsetY)
    {
        Vector3 tempPos = crosshair.transform.localPosition;
        tempPos.y += offsetY;
        Debug.Log(tempPos.y);
        tempPos.y = Mathf.Clamp(tempPos.y, minYCrosshair, maxYCrosshair);
        crosshair.transform.localPosition = tempPos;
    }

    public void UpdateDistanceUI(float trackDistance)
    {
        if (trackDistance == 0)
        {
            distanceIndicator.SetText("");
            return;
        }
        distanceIndicator.SetText($"{trackDistance}m");
    }

    public void UpdateTankSpeedUI(float speed)
    {
        tankSpeedIndicator.SetText($"{speed} km/h");
    }

    public void UpdateAmmoCountUI(float ammoCount)
    {
        ammoCountIndicator.SetText($"{ammoCount}");
    }

    public void UpdateShellTypeUI(string type)
    {
        shellTypeIndicator.SetText(type);
    }

    public void SetTurretRotationUI(Vector3 turretRot)
    {
        Quaternion rot = turretRotationUI.transform.rotation;
        rot.eulerAngles = new Vector3(0,0, -turretRot.y);
        turretRotationUI.transform.rotation = rot;
    }

    public void OnPlayerKilled()
    {
        Cursor.lockState = CursorLockMode.Confined; 
        HUDContainer.SetActive(false);
        gameOverScreen.SetActive(true);
    }
}
