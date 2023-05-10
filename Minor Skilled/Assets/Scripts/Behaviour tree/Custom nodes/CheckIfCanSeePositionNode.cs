using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckIfCanSeePositionNode : SequenceNode
{
    private Vector3 _targetPosition;
    private Transform _targetTransform;
    
    public CheckIfCanSeePositionNode(AIBlackboard blackboard, Vector3 targetPosition, float maxRange) : base(blackboard)
    {
        _targetPosition = targetPosition;
            
        CheckRangeNode rangeNode = new(_blackboard, targetPosition, maxRange);
        AddChildNode(rangeNode);
        
        CheckFOVNode fovNode = new(_blackboard);
        AddChildNode(fovNode);

        CheckLineOfSightNode lineOfSightNode = new(_blackboard);
        AddChildNode(lineOfSightNode);
    }
    
    public CheckIfCanSeePositionNode(AIBlackboard blackboard, Transform targetTransform, float maxRange) : base(blackboard)
    {
        _targetTransform = targetTransform;
        
        CheckRangeNode rangeNode = new(_blackboard, targetTransform, maxRange);
        AddChildNode(rangeNode);
        
        CheckFOVNode fovNode = new(_blackboard);
        AddChildNode(fovNode);

        CheckLineOfSightNode lineOfSightNode = new(_blackboard);
        AddChildNode(lineOfSightNode);
    }

    public override NodeState Evaluate()
    {
        _nodeState = base.Evaluate();
        
        return _nodeState;
    }
    
    public override void DrawGizmos()
    {
        if (_nodeState == NodeState.Failure)
            Gizmos.color = Color.red;
        else if (_nodeState == NodeState.Success)
            Gizmos.color = Color.green;
        
        if(_targetTransform != null)
            Gizmos.DrawSphere(_targetTransform.position, 2f);
        else Gizmos.DrawSphere(_targetPosition, 2f);
    }
}