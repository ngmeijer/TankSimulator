using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPositionNode : CombatNode
{
    private Vector3 _targetPos;
    private float _currentTime;
    
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
        _currentTime += Time.deltaTime;
        if (_currentTime > _blackboard.PathCalculationInterval)
        {
            NavMeshPath path = new();
            _currentTime = 0f;
            _blackboard.Agent.CalculatePath(_blackboard.MoveToPosition, path);
            _blackboard.Agent.SetPath(path);
        }
        
        return _nodeState;
    }

    public override void DrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, _blackboard.MaxStoppingDistance);
        Handles.Label(_blackboard.ThisTrans.position + Vector3.right * _blackboard.MaxStoppingDistance, $"Stopping distance: {_blackboard.MaxStoppingDistance}");

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_blackboard.MoveToPosition, 2f);

        if (_blackboard.Agent.path != null)
        {
            Vector3[] pathPositions = _blackboard.Agent.path.corners;
            
            Gizmos.color = Color.yellow;
            foreach (var pathPoint in pathPositions)
            {
                Gizmos.DrawSphere(pathPoint, 0.5f);
            }
        }
    }
}