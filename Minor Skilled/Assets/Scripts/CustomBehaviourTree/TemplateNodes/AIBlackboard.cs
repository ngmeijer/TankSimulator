using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Behaviour tree/Blackboard instance")]
public class AIBlackboard : ScriptableObject
{
    public Vector3 MoveToPosition;
    public Vector3 InvestigationFocusPoint;
    public Vector3 DefiniteFocusPoint;
    public float PathCalculationInterval;

    public List<Vector3> PatrolPoints;
}