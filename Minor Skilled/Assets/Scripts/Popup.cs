
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private float _destroyDelay = 3f;
    [SerializeField] private float _offsetY;
    [SerializeField] private Image _background;
    
    private void Start()
    {
        Destroy(this.gameObject, _destroyDelay);
    }

    public void UpdateContent(string content)
    {
        _contentText.SetText(content);
    }

    public void UpdateColor(Color popupColour)
    {
        _background.color = popupColour;
    }
}