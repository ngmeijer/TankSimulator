using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShellType
{
    
}

public class Shell : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private GameObject GFX;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private string shellType;
    [SerializeField] private float forceOnImpact = 10000;
    [SerializeField] private Transform centerOfMass;
    private bool vfxHasPlayed;
    public int Damage;
    public int AbsoluteArmorPenetration;
    public int RelativeArmorPenetration;
    public int CriticalChance;
    
    //if crit hit:
    //  Total damage = damage * criticalDamageMultiplier;
    public int CriticalDamageMultiplier;
    
    private void Start()
    {
        rb.centerOfMass = centerOfMass.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        rb.velocity = Vector3.zero;
        if (explosion != null)
        {
            if (explosion.isPlaying) return;
            if (vfxHasPlayed) return;
            explosion.transform.parent = null;
            explosion.transform.up = other.contacts[0].normal;
            explosion.transform.position = other.contacts[0].point;
            explosion.Play();
            vfxHasPlayed = true;
        }
        rb.AddExplosionForce(forceOnImpact, transform.position, 12);
        GFX.SetActive(false);
        rb.isKinematic = true;
        GFX.transform.localPosition = Vector3.zero;
        GFX.transform.localRotation = Quaternion.identity;
    }

    public string GetShellType() => shellType;
}
