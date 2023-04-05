using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthStatsUpdater : MonoBehaviour
{
    [SerializeField] private List<GameObject> _worldCanvases;

    [Header("Hull UI")]
    [SerializeField] private Slider _hullHealth;
    [SerializeField] private Slider _hullArmor;
    [SerializeField] private TextMeshProUGUI _stateText;
    
    [Header("Turret UI")]
    [SerializeField] private Slider _turretHealth;
    [SerializeField] private Slider _turretArmor;
    
    private void Update()
    {
        foreach (var canvas in _worldCanvases)
        {
            //canvas.transform.LookAt(GameManager.Instance.InspectCameraPosition);
        }
    }

    public void UpdateValues(TankData newData)
    {
        if (newData == null)
        {
            Debug.Log("Incoming TankData in HealthStatsUpdater is null. Trace back to source");
            return;
        }

        foreach (var tankPart in newData.TankParts)
        {
            TankParts part = tankPart.ThisPart;
            float currentHealthPercent = (float)tankPart.CurrentHealth / tankPart.MaxHealth;
            float currentArmorPercent = (float)tankPart.CurrentArmor / tankPart.MaxArmor;

            switch (part)
            {
                case TankParts.Turret:
                    
                    break;
                case TankParts.LeftTrack:
                    break;
                case TankParts.RightTrack:
                    break;
                case TankParts.Hull:
                    _hullHealth.value = currentHealthPercent;
                    _hullArmor.value = currentArmorPercent;
                    _stateText.SetText(tankPart.PartState.ToString());
                    break;
            }
        }
        
        
        HUDManager.Instance.UpdateCurrentHealthForEntity(0, newData.GetCurrentTotalHealth(), newData.GetMaxTotalHealth());
        HUDManager.Instance.UpdateCurrentArmorForEntity(0, newData.GetCurrentTotalArmor(), newData.GetMaxTotalArmor());
    } 
}
