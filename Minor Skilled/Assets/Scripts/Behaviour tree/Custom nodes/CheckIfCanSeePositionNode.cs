using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckIfCanSeePositionNode : SequenceNode
{
    private Transform _targetTransform;

    public CheckIfCanSeePositionNode(AIBlackboard blackboard, Transform targetTransform, float maxRange) : base(blackboard)
    {
        _targetTransform = targetTransform;
        
        CheckRangeNode rangeNode = new(_blackboard, targetTransform, maxRange);
        AddChildNode(rangeNode);
        
        CheckFOVNode fovNode = new(_blackboard, targetTransform, maxRange);
        AddChildNode(fovNode);

        CheckLineOfSightNode lineOfSightNode = new(_blackboard);
        AddChildNode(lineOfSightNode);
    }

    public override NodeState Evaluate()
    {
        _nodeState = base.Evaluate();

        if (_nodeState == NodeState.Success)
        {
            _blackboard.TurretLookAtPosition = GameManager.Instance.Player.transform.position;
            _blackboard.MoveToPosition = _blackboard.TurretLookAtPosition;
        }

        return _nodeState;
    }
    
    public override void DrawGizmos()
    {
        base.DrawGizmos();
        
        if (_nodeState == NodeState.Failure)
            Gizmos.color = Color.red;
        else if (_nodeState == NodeState.Success)
            Gizmos.color = Color.green;
        
        if(_targetTransform != null)
            Gizmos.DrawSphere(_targetTransform.position, 2f);
    }
}