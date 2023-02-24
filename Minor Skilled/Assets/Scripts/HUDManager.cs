using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Slider reloadIndicator;
    [SerializeField] private TextMeshProUGUI distanceIndicator;
    
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
}
