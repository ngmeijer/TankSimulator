using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimateButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _tweenSpeed;
    [SerializeField] private float _targetX;
    private float _startX;
    
    private void Start()
    {
        _startX = transform.localPosition.x;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.DOLocalMoveZ(_targetX,_tweenSpeed);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.DOLocalMoveZ(_startX,_tweenSpeed);
    }
}
