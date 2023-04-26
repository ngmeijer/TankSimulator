using System;
using UnityEngine;
using UnityEngine.AI;

public class AIBlackboard : MonoBehaviour
{
    protected TankComponentManager _componentManager;
    public NavMeshAgent Agent;
    public Transform ThisTrans;
    public Transform TurretTrans;
    public Transform Raycaster;

    public Vector3 CurrentAgentDestination;

    public float MaxInstantVisionRange;
    public float MaxVisionInvestigationRange;
    public float MaxPatrolRange;
    public float MaxShootingRange;
    public float ViewAngle;

    private void Start()
    {
        _componentManager = GetComponent<TankComponentManager>();
    }
}