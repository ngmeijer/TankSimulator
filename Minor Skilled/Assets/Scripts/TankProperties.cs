using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tank Properties")]
public class TankProperties : ScriptableObject
{
    public string TankName;
    public float Acceleration = 1500f;
    public float ReverseAcceleration = 500f;
    public float SingleTrackSpeed = 2000f;
    public float MaxSpeed = 20f;
    public float HullRotateSpeed = 15f;
    public float TurretRotateSpeed = 60f;
    public float TurretTiltSpeed = 80f;
    public float TankMass;

    public float FireForce = 50f;
    public float ReloadTime = 4f;
    public int AmmoCount = 5;
    public int MaxArmor = 100;
    public int MaxHealth = 50;
}