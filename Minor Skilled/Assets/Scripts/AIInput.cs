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
        if (_componentManager.HasDied) yield break;
        yield return new WaitForSeconds(_componentManager.Properties.ReloadTime);
        if (!_componentManager.ShootComponent.CanFire) yield break;
        
        _shootComponent.FireShell();

        StartCoroutine(testShoot());
    }
}
