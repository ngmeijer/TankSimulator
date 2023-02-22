using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tank Properties")]
public class TankProperties : ScriptableObject
{
    public float Acceleration = 1500f;
    public float SingleTrackSpeed = 2000f;
    public float MaxSpeed = 20f;
    public float HullRotateSpeed = 15f;
    public float TurretRotateSpeed = 60f;
    public float TurretTiltSpeed = 80f;
    public float TankMass;
}