using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tank Properties")]
public class TankProperties : ScriptableObject
{
    public float Acceleration = 2f;
    public float BrakeTorque = 500f;
    public float LeftTrackSpeed;
    public float RightTrackSpeed;
    public float MaxSpeed = 20f;
    public float HullRotateSpeed = 15f;
    public float TurretRotateSpeed = 60f;
}