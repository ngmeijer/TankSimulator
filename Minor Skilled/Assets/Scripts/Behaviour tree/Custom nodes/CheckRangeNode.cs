using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckRangeNode : BehaviourNode
{
    private KeyValuePair<string, float> _maxRange;
    private Vector3 _targetPosition;
    private Transform _targetTransform;

    public CheckRangeNode(AIBlackboard blackboard, Transform targetTransform, KeyValuePair<string, float> maxRange) : base(blackboard)
    {
        _maxRange = maxRange;
        _targetTransform = targetTransform;
    }

    public override NodeState Evaluate()
    {
        _nodeState = TargetInRange() ? NodeState.Success : NodeState.Failure;
        return _nodeState;
    }
    
    private bool TargetInRange()
    {
        Vector3 target = _targetTransform ? _targetTransform.position : _targetPosition;
        float distance = Vector3.Distance(target, _blackboard.ThisTrans.position);

        return distance <= _maxRange.Value;
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

        Handles.color = TransparentGizmoColor;
        Handles.DrawWireDisc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, _maxRange.Value);
        Handles.Label(_blackboard.ThisTrans.position + Vector3.right * _maxRange.Value, $"{_maxRange.Key}: {_maxRange.Value}");
    }
}