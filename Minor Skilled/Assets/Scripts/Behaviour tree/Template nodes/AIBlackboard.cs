using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI blackboards/Blackboard instance")]
public class AIBlackboard : ScriptableObject
{
    [HideInInspector] public AIController AIController;
    [HideInInspector] public NavMeshAgent Agent;
    [HideInInspector] public Transform ThisTrans;
    [HideInInspector] public Transform TurretTrans;
    [HideInInspector] public Transform Raycaster;
    
    public float MaxStoppingDistance;
    public float MaxInstantVisionRange;
    public float MaxVisionInvestigationRange;
    public float MinPatrolRange;
    public float MaxPatrolRange;
    public float ViewAngle;
    public Vector3 TurretLookAtPosition;
    public Vector3 MoveToPosition;
    public float PathCalculationInterval;
    
    private void OnEnable()
    {
        if (AIController == null)
            return;
        
        Agent = AIController.NavAgent;
        ThisTrans = AIController.transform;
        TurretTrans = AIController.ComponentManager.TurretControlComponent.TurretTransform;
        Raycaster = AIController.ComponentManager.ShootComponent.Raycaster;
    }
}