using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tank Properties")]
public class TankProperties : ScriptableObject
{
    public float _acceleration = 2f;
    public float _maxSpeed = 20f;
    public float _rotateSpeed = 15f;
}