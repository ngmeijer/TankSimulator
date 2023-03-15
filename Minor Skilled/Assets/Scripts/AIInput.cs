using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : MonoBehaviour
{
    private TankComponentManager _componentManager;
    private BaseShootComponent _baseShootComponent;

    private void Awake()
    {
        _componentManager = GetComponent<TankComponentManager>();
        _baseShootComponent = GetComponent<BaseShootComponent>();
    }

    private void Start()
    {
        StartCoroutine(testShoot());
    }

    private IEnumerator testShoot()
    {
        if(_componentManager.HasDied) StopCoroutine(testShoot());
        yield return new WaitForSeconds(_componentManager.Properties.ReloadTime);
        
        _baseShootComponent.FireShell();
        if (_componentManager.HasDied) yield break;
        
        StartCoroutine(testShoot());
    }
}
