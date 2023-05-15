using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverRaycaster : MonoBehaviour
{
    [SerializeField] private int RaysToShoot = 30;
    [SerializeField] private float Cooldown = 1f;

    private void Start()
    {
        //StartCoroutine(ShootRays());
    }

    private IEnumerator ShootRays()
    {
        float angle = 0;
        for (int i = 0; i < RaysToShoot; i++)
        {
            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / RaysToShoot;

            Vector3 dir = new Vector3(transform.position.x * x, transform.position.y * y, 0);
            RaycastHit hit;
            Debug.DrawLine(transform.position, dir, Color.red);
            if (Physics.Raycast(transform.position, dir, out hit))
            {
                //here is how to do your cool stuff ;)
            }
            
            Debug.Break();
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(ShootRays());
    }
}
