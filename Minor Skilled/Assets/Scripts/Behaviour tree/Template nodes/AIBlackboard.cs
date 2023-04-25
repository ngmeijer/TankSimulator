using System;
using UnityEngine;
using UnityEngine.AI;

public class AIBlackboard : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Transform ThisTrans;
    [HideInInspector] public Transform PlayerTrans;

    public Vector3 CurrentAgentDestination;

    public float MaxInstantVisionRange;
    public float MaxVisionInvestigationRange;
    public float MaxPatrolRange;

    private void Start()
    {
        PlayerTrans = GameManager.Instance.Player.transform;
    }
}