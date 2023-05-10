using UnityEditor;
using UnityEngine;

public class MoveToPositionNode : CombatNode
{
    private Vector3 _targetPos;
    
    public MoveToPositionNode(AIBlackboard blackboard) : base(blackboard)
    {
        
    }

    public override NodeState Evaluate()
    {
        base.Evaluate();

        float distanceToTargetPos = Vector3.Distance(_blackboard.ThisTrans.position, _blackboard.MoveToPosition);
        if (distanceToTargetPos <= _blackboard.MaxStoppingDistance)
        {
            _blackboard.Agent.ResetPath();
            _nodeState = NodeState.Success;
            return _nodeState;
        }

        _nodeState = NodeState.Running;
        _blackboard.Agent.SetDestination(_blackboard.MoveToPosition);

        return _nodeState;
    }

    public override void DrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, _blackboard.MaxStoppingDistance);
        Handles.Label(_blackboard.ThisTrans.position + Vector3.right * _blackboard.MaxStoppingDistance, $"Stopping distance: {_blackboard.MaxStoppingDistance}");
    }
}