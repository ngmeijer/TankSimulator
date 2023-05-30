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
    public Vector3 CurrentPatrolPoint;
    public Vector3 CurrentCoverPoint;
    public float PathCalculationInterval;

    public List<Vector3> GenericPointsFound;
    public List<Vector3> GeneratedPatrolPoints;
    public List<Vector3> GeneratedCoverPoints;
    
    public NavMeshPath GeneratedNavPath;
    public bool ShouldCountDown;
    public bool IsCurrentlyRepairing;
    public bool CanGenerateNavPoints;

    public void ResetValues()
    {
        PointToRotateTurretTo = Vector3.zero;
        LastTargetSighting = Vector3.zero;
        GenericPointsFound = null;
        GeneratedCoverPoints = null;
        GeneratedPatrolPoints = null;
        CurrentPatrolPoint = Vector3.zero;
        CurrentCoverPoint = Vector3.zero;
        MoveToPosition = Vector3.zero;
        GeneratedNavPath = null;
        ShouldCountDown = true;
        CanGenerateNavPoints = false;
    }
}