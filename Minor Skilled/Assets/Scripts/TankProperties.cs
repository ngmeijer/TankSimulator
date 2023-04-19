using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tank Properties")]
public class TankProperties : ScriptableObject
{
    public string TankName;
    public AnimationCurve MotorTorque;
    public AnimationCurve GearRatios;
    public int MaxGears = 4;
    public float SingleTrackTorqueMultiplier = 2000f;
    public float TP_HorizontalSensitivity = 30f;
    public float[] ADS_HorizontalSensitivity;
    public float TankMass;
    public float KickbackForce = 9000f;

    public float ShellSpeed = 50f;
    public float ReloadTime = 4f;
    public float TP_VerticalSensitivity = 1f;
    public float[] ADS_VerticalSensitivity;
    public int APAmmo = 50;
    public int HEATAmmo = 50;
    public int HEAmmo = 50;

    [Header("Popup content")] 
    public string OnShellFired;
    public string OnEntityDeath;
}