using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TankComponentManager : MonoBehaviour
{
    public TankProperties Properties;
    public Rigidbody TankRB;
    
    public List<WheelCollider> LeftTrackWheelColliders = new List<WheelCollider>(); 
    public List<WheelCollider> RightTrackWheelColliders = new List<WheelCollider>();

    private void OnValidate()
    {
        TankRB.mass = Properties.TankMass;
    }
}