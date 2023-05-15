using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckIfCanSeePositionNode")]
public class CheckIfCanSeePositionNode : SequenceNode
{
    private Transform _targetTransform;
    
    public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        _nodeState = base.Evaluate(blackboard, controller);

        if (_nodeState == NodeState.Success)
        {
            blackboard.TurretLookAtPosition = GameManager.Instance.Player.transform.position;
            blackboard.MoveToPosition = blackboard.TurretLookAtPosition;
        }
        
        return _nodeState;
    }
    
    public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
    {
        base.DrawGizmos(blackboard, controller);
        
        if (_nodeState == NodeState.Failure)
            Gizmos.color = Color.red;
        else if (_nodeState == NodeState.Success)
            Gizmos.color = Color.green;
        
        if(_targetTransform != null)
            Gizmos.DrawSphere(_targetTransform.position, 2f);
    }
}