using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI blackboards/Blackboard instance")]
public class AIBlackboard : ScriptableObject
{
    public Vector3 TurretLookAtPosition;
    public Vector3 MoveToPosition;
    public float PathCalculationInterval;
    public Vector3 TargetInvestigatePosition;
}