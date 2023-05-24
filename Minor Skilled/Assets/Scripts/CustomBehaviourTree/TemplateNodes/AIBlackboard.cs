using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Behaviour tree/Blackboard instance")]
public class AIBlackboard : ScriptableObject
{
    public Vector3 PointToRotateTurretTo;
    public Vector3 LastTargetSighting;
    public Vector3 MoveToPosition;
    
    public float PathCalculationInterval;

    public List<Vector3> GeneratedPatrolPoints;
    public Vector3 CurrentPatrolPoint;
    public NavMeshPath PatrolPath;

    public void ResetValues()
    {
        PointToRotateTurretTo = Vector3.zero;
        LastTargetSighting = Vector3.zero;
        GeneratedPatrolPoints = null;
        CurrentPatrolPoint = Vector3.zero;
        MoveToPosition = Vector3.zero;
        PatrolPath = null;
    }
}