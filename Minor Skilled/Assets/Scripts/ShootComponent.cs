using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootComponent : MonoBehaviour
{
    [SerializeField] private GameObject defaultShellPrefab;
    private TankComponentManager componentManager;
    [SerializeField] private ParticleSystem fireExplosion;
    private bool readyToFire;

    private void Awake()
    {
        componentManager = GetComponent<TankComponentManager>();
    }

    public void FireShell()
    {
        GameObject shellInstance = Instantiate(defaultShellPrefab, componentManager.ShellSpawnpoint.position, componentManager.ShellSpawnpoint.rotation);
        Rigidbody rb = shellInstance.GetComponent<Rigidbody>();
        rb.AddForce(rb.transform.forward * componentManager.Properties.FireForce);
        fireExplosion.transform.position = componentManager.VFXPivot.position;
        fireExplosion.transform.rotation = componentManager.VFXPivot.rotation;
        fireExplosion.Play();
        readyToFire = false;
    }

    private IEnumerator ReloadCannon()
    {
        
        yield return null;
    }

    public float TrackDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(componentManager.ShellSpawnpoint.position, componentManager.ShellSpawnpoint.forward,
          out hit, Mathf.Infinity, 0))
        {
            return hit.distance;
        }
        return 0;
    }
}
