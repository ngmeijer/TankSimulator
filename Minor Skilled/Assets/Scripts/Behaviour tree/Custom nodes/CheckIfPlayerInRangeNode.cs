using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckIfPlayerInRangeNode : BehaviourNode
{
    private KeyValuePair<string, float> _maxRange;
    
    public CheckIfPlayerInRangeNode(KeyValuePair<string, float> maxRange, AIBlackboard blackboard) : base(blackboard)
    {
        _maxRange = maxRange;
    }
    
    public override NodeState Evaluate()
    {
        _nodeState = PlayerInRange() ? NodeState.Success : NodeState.Failure;

        return _nodeState;
    }

    public override void DrawGizmos()
    {
        Handles.color = new Color(0,0,0, DEFAULT_ALPHA);
        float invertedAngle = (360 - _blackboard.ViewAngle) / 2;
        Handles.DrawSolidArc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, -_blackboard.ThisTrans.forward, -invertedAngle, _blackboard.MaxShootingRange);
        Handles.DrawSolidArc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, -_blackboard.ThisTrans.forward, invertedAngle, _blackboard.MaxShootingRange);
        Handles.Label(_blackboard.ThisTrans.position + _blackboard.ThisTrans.right * 
            _maxRange.Value, $"{_maxRange.Key}: {_maxRange}");
    }

    private bool PlayerInRange()
    {
        float distance =
            Vector3.Distance(GameManager.Instance.Player.EntityOrigin.position, _blackboard.ThisTrans.position);

        return distance <= _maxRange.Value;
    }
}