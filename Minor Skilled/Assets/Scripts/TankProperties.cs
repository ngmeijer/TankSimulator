﻿using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tank Properties")]
public class TankProperties : ScriptableObject
{
    public string TankName;
    public AnimationCurve MotorTorque;
    public AnimationCurve GearRatios;
    public int MaxGears = 4;
    public float SingleTrackTorqueMultiplier = 2000f;
    public float TurretRotateSpeed = 60f;
    public float TankMass;
    public float KickbackForce = 9000f;

    public float ShellSpeed = 50f;
    public float ReloadTime = 4f;
    public float TP_VerticalSensitivity = 1f;
    public float ADS_VerticalSensitivity = 0.75f;
    public int APAmmo = 50;
    public int HEATAmmo = 50;
    public int HEAmmo = 50;
    public int MaxArmor = 100;
    public int MaxHealth = 50;

    [Header("Popup content")] 
    public string OnShellFired;
    public string OnEntityDeath;
}