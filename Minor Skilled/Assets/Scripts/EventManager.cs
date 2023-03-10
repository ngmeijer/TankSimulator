using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public GameEvent OnShellFired = new GameEvent();
    public GameEvent OnTankComponentHit = new GameEvent();
    public UnityEvent<TankComponentManager> OnEntityDeath = new UnityEvent<TankComponentManager>();

    private PopupManager _popupManager;
    
    private void Awake()
    {
        _popupManager = FindObjectOfType<PopupManager>();
    }

    private void Start()
    {
        OnShellFired.AddListener((content) => _popupManager.CreatePopup(content));
        OnTankComponentHit.AddListener((content) => _popupManager.CreatePopup(content));
    }
}