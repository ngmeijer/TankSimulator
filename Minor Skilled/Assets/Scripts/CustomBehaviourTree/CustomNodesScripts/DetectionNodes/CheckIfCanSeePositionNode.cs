using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    /// <summary>
    /// A collection class to check if a point lies in range, FOV and has line of sight.
    /// Then registers that point as Focus point and Target MovePosition
    /// </summary>
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckIfCanSeePositionNode")]
    public class CheckIfCanSeePositionNode : SequenceNode
    {
        private Transform _targetTransform;
    
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = base.Evaluate(blackboard, controller);

            if (_nodeState == NodeState.Success)
            {
                blackboard.DefiniteFocusPoint = blackboard.InvestigationFocusPoint;
                blackboard.MoveToPosition = blackboard.DefiniteFocusPoint;
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
}