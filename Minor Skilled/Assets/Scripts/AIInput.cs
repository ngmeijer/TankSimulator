using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : MonoBehaviour
{
    private TankComponentManager componentManager;
    private BaseShootComponent _baseShootComponent;

    private void Awake()
    {
        componentManager = GetComponent<TankComponentManager>();
        _baseShootComponent = GetComponent<BaseShootComponent>();
    }

    private void Start()
    {
        StartCoroutine(testShoot());
    }

    private IEnumerator testShoot()
    {
        if(componentManager.HasDied) StopCoroutine(testShoot());
        yield return new WaitForSeconds(componentManager.Properties.ReloadTime);
        
        _baseShootComponent.FireShell();
        if (componentManager.HasDied) yield break;
        
        StartCoroutine(testShoot());
    }
}
