using UnityEditor;
using UnityEngine;

public class CheckRangeNode : BehaviourNode
{
    private float _maxRange;
    private Vector3 _targetPosition;
    
    public CheckRangeNode(AIBlackboard blackboard, Vector3 targetPosition, float maxRange) : base(blackboard)
    {
        _maxRange = maxRange;
        _targetPosition = targetPosition;
    }
    
    public CheckRangeNode(AIBlackboard blackboard, Transform targetPosition, float maxRange) : base(blackboard)
    {
        _maxRange = maxRange;
        _targetPosition = targetPosition.position;
    }

    public override NodeState Evaluate()
    {
        _nodeState = PlayerInRange() ? NodeState.Success : NodeState.Failure;
        
        return _nodeState;
    }
    
    private bool PlayerInRange()
    {
        float distance = Vector3.Distance(_targetPosition, _blackboard.ThisTrans.position);

        return distance <= _maxRange;
    }

    public override void DrawGizmos()
    {
        if (_nodeState == NodeState.Success)
        {
            SolidGizmoColor = Color.red;
            TransparentGizmoColor = new Color(255, 0, 0, DEFAULT_ALPHA);
        }
        else
        {
            SolidGizmoColor = Color.green;
            TransparentGizmoColor = new Color(0, 255, 0, DEFAULT_ALPHA);    
        }
    }
}