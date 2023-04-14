using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GameEvent : UnityEvent<string>
{
    
}

public class PopupManager : SingletonMonobehaviour<PopupManager>
{
    [SerializeField] private GameObject _popupPrefab;
    [SerializeField] private Transform _popupParent;
    
    public void CreatePopup(string arg0, Color popupColour)
    {
        GameObject popupContainer = Instantiate(_popupPrefab, _popupParent);
        Popup popupInstance = popupContainer.GetComponent<Popup>();
        popupInstance.UpdateContent(arg0);
        popupInstance.UpdateColor(popupColour);
    }
}