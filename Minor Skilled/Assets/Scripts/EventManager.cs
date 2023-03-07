using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public GameEvent OnShellFired = new GameEvent();
    public GameEvent OnTankComponentHit = new GameEvent();

    private PopupManager popupManager;
    
    private void Awake()
    {
        popupManager = FindObjectOfType<PopupManager>();
    }

    private void Start()
    {
        OnShellFired.AddListener((content) => popupManager.CreatePopup(content));
        OnTankComponentHit.AddListener((content) => popupManager.CreatePopup(content));
    }
}