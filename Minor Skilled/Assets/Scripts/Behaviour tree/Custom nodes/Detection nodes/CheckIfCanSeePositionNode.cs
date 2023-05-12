using System.Collections.Generic;
using Behaviour_tree.Custom_nodes;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckIfCanSeePositionNode")]
public class CheckIfCanSeePositionNode : SequenceNode
{
    private Transform _targetTransform;
    
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