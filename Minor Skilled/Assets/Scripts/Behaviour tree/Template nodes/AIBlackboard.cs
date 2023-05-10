using System;
using UnityEngine;
using UnityEngine.AI;

public class AIBlackboard : MonoBehaviour
{
    public StateSwitcher StateSwitcher;
    public NavMeshAgent Agent;
    public Transform ThisTrans;
    public Transform TurretTrans;
    public Transform Raycaster;

    public Vector3 CurrentAgentDestination;
    public Vector3 SelectedCoverPosition;

    public float MaxInstantVisionRange;
    public float MaxVisionInvestigationRange;
    public float MaxPatrolRange;
    public float MaxShootingRange;
    public float ViewAngle;
    public Vector3 InvestigatePosition;

    private void Awake()
    {
        StateSwitcher = GetComponent<StateSwitcher>();
    }
}