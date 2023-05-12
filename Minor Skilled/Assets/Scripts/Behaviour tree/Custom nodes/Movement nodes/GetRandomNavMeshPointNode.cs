using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Behaviour tree/Movement/GetRandomNavMeshPointNode")]
public class GetRandomNavMeshPointNode : CombatNode
{
    // public GetRandomNavMeshPointNode(AIBlackboard blackboard) : base(blackboard)
    // {
    //     
    // }

    public override NodeState Evaluate()
    {
        Vector3 currentTargetPosition = SamplePosition();
        if (!CheckIfPointInFOV(currentTargetPosition))
            currentTargetPosition = SamplePosition();

        _blackboard.MoveToPosition = currentTargetPosition;
        return _nodeState;
    }

    private Vector3 SamplePosition()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomDirection = Random.insideUnitSphere * _blackboard.MaxPatrolRange;
        Vector3 randomPosition = _blackboard.ThisTrans.position + randomDirection;
        NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, _blackboard.MaxPatrolRange, 1);
        float distanceToPosition = Vector3.Distance(_blackboard.ThisTrans.position, hit.position);
        if (distanceToPosition < _blackboard.MinPatrolRange)
            finalPosition = SamplePosition();
        else finalPosition = hit.position;

        return finalPosition;
    }

    public bool CheckIfPointInFOV(Vector3 positionToCheck)
    {
        Vector3 targetDirection = positionToCheck - _blackboard.TurretTrans.position;
        float totalAngle = Vector3.Angle(_blackboard.TurretTrans.forward, targetDirection.normalized);
        
        return totalAngle <= _blackboard.ViewAngle / 2;
    }

    public override void DrawGizmos()
    {
        
    }
}