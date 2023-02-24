using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestRange : MonoBehaviour
{
    private Dictionary<string, Vector3> _hitsDetected = new Dictionary<string, Vector3>();

    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit registered");
        _hitsDetected.Add($"Hit {_hitsDetected.Count}", collision.GetContact(0).point);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var hit in _hitsDetected)
        {
            Handles.Label(hit.Value + new Vector3(0, 0.5f, 0), hit.Key);
            Gizmos.DrawSphere(hit.Value, 0.5f);
        }
    }
}
