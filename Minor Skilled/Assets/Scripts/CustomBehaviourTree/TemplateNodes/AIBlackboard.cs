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
    
    public float PathCalculationInterval;

    public List<Vector3> GeneratedPatrolPoints;
    public List<Vector3> ValidPatrolPoints;
    public Vector3 CurrentPatrolPoint;

    public void ResetValues()
    {
        PointToRotateTurretTo = Vector3.zero;
        LastTargetSighting = Vector3.zero;
        GeneratedPatrolPoints = null;
        ValidPatrolPoints = null;
        CurrentPatrolPoint = Vector3.zero;
    }
}