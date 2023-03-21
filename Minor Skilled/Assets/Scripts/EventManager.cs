using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public GameEvent OnShellFired = new GameEvent();
    public GameEvent OnTankComponentHit = new GameEvent();
    public UnityEvent<TankComponentManager> OnEntityDeath = new UnityEvent<TankComponentManager>();
    public UnityEvent<CameraMode> OnCameraChanged = new UnityEvent<CameraMode>();

    private void Start()
    {
        OnShellFired.AddListener((content) => PopupManager.Instance.CreatePopup(content, Color.red));
        OnTankComponentHit.AddListener((content) => PopupManager.Instance.CreatePopup(content, Color.red));
    }
}