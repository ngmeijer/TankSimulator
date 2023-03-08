using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GameEvent : UnityEvent<string>
{
    
}

public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject _popupPrefab;
    [SerializeField] private Transform _popupParent;
    
    public void CreatePopup(string arg0)
    {
        GameObject popupInstance = Instantiate(_popupPrefab, _popupParent);
        Popup component = popupInstance.GetComponent<Popup>();
        component.UpdateContent(arg0);
    }
}