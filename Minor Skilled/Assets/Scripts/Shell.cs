using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private GameObject GFX;
    [SerializeField] private Rigidbody rb;

    private void OnCollisionEnter(Collision other)
    {
        explosion.transform.parent = null;
        rb.AddExplosionForce(10000, transform.position, 12);
        explosion.Play();
        GFX.SetActive(false);
    }
}
