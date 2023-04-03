using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStatsUpdater : MonoBehaviour
{
    [SerializeField] private GameObject _turretCanvas;
    [SerializeField] private GameObject _leftTrackCanvas;
    [SerializeField] private GameObject _rightTrackCanvas;
    [SerializeField] private GameObject _hullCanvas;

    public void ShowUI(bool enabled)
    {
        _turretCanvas.SetActive(enabled);
        _leftTrackCanvas.SetActive(enabled);
        _rightTrackCanvas.SetActive(enabled);
        _hullCanvas.SetActive(enabled);
    }

    public void UpdateValues(TankData newData)
    {
        
    }
}
