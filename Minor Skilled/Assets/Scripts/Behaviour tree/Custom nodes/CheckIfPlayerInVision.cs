using UnityEngine;

public class CheckIfPlayerInVision : BehaviourNode
{
    public override NodeState Evaluate(AIBlackboard blackboard)
    {
        if (RaycastToPlayer(blackboard.ThisTrans, blackboard.PlayerTrans))
            _nodeState = NodeState.Success;
        else _nodeState = NodeState.Failure;

        return _nodeState;
    }

    public override void DrawGizmos()
    {
        if (_nodeState == NodeState.Failure)
            Gizmos.color = Color.red;
        else if (_nodeState == NodeState.Success)
            Gizmos.color = Color.green;
        Gizmos.DrawLine(_blackboard.ThisTrans.position, _blackboard.PlayerTrans.position);
    }

    private bool RaycastToPlayer(Transform thisTrans, Transform playerTrans)
    {
        if (Physics.Raycast(thisTrans.position, playerTrans.position - thisTrans.position, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }
}