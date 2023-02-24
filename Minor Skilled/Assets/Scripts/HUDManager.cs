using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Slider reloadIndicator;
    [SerializeField] private TextMeshProUGUI distanceIndicator;
    [SerializeField] private TextMeshProUGUI tankSpeedIndicator;
    [SerializeField] private TextMeshProUGUI ammoCountIndicator;
    
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
}
