using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootComponent : TankComponent
{
    [SerializeField] private GameObject defaultShellPrefab;
    [SerializeField] private ParticleSystem fireExplosion;
    public bool readyToFire { get; private set; } = true;

    public void FireShell()
    {
        GameObject shellInstance = Instantiate(defaultShellPrefab, componentManager.ShellSpawnpoint.position, componentManager.ShellSpawnpoint.rotation);
        Rigidbody rb = shellInstance.GetComponent<Rigidbody>();
        rb.AddForce(rb.transform.forward * componentManager.Properties.FireForce);
        fireExplosion.transform.position = componentManager.VFXPivot.position;
        fireExplosion.transform.rotation = componentManager.VFXPivot.rotation;
        fireExplosion.Play();
        readyToFire = false;
        StartCoroutine(ReloadCannon());
    }

    private void Update()
    {
        componentManager.HUDManager.UpdateDistanceUI(TrackDistance());
    }

    private IEnumerator ReloadCannon()
    {
        StartCoroutine(componentManager.HUDManager.UpdateReloadUI(componentManager.Properties.ReloadTime));
        yield return new WaitForSeconds(componentManager.Properties.ReloadTime);

        readyToFire = true;
    }

    public float TrackDistance()
    {
        RaycastHit hit;
        Debug.DrawRay(componentManager.ShellSpawnpoint.position, componentManager.ShellSpawnpoint.forward * 1000, Color.red);
        if (Physics.Raycast(componentManager.ShellSpawnpoint.position, componentManager.ShellSpawnpoint.forward,
          out hit, Mathf.Infinity))
        {
            return (float)Math.Round(hit.distance, 2);
        }
        return 0;
    }
}
