using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : MonoBehaviour
{
    private TankComponentManager _componentManager;
    private ShootComponent _shootComponent;

    private void Awake()
    {
        _componentManager = GetComponent<TankComponentManager>();
        _shootComponent = GetComponent<ShootComponent>();
    }

    private void Start()
    {
        StartCoroutine(testShoot());
    }

    private IEnumerator testShoot()
    {
        if(_componentManager.HasDied) StopCoroutine(testShoot());
        yield return new WaitForSeconds(_componentManager.Properties.ReloadTime);
        
        _shootComponent.FireShell();
        if (_componentManager.HasDied) yield break;
        
        StartCoroutine(testShoot());
    }
}
