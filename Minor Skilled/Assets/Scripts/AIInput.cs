using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : MonoBehaviour
{
    private TankComponentManager componentManager;
    private ShootComponent shootComponent;

    private void Awake()
    {
        componentManager = GetComponent<TankComponentManager>();
        shootComponent = GetComponent<ShootComponent>();
    }

    private void Start()
    {
        StartCoroutine(testShoot());
    }

    private IEnumerator testShoot()
    {
        if(componentManager.HasDied) StopCoroutine(testShoot());
        yield return new WaitForSeconds(componentManager.Properties.ReloadTime);
        
        shootComponent.FireShell();
        StartCoroutine(testShoot());
    }
}
