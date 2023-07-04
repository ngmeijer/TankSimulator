using UnityEditor;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/SetLastTargetSighting")]
    public class SetLastTargetSightingNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = NodeState.Success;

            blackboard.LastTargetSighting = blackboard.PointToRotateTurretTo;
            
            return _nodeState;
        }
        
#if UNITY_EDITOR
        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            base.DrawGizmos(blackboard, controller);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(blackboard.LastTargetSighting, 1f);
            Handles.Label(blackboard.LastTargetSighting + GameManager.HandlesOffset, "Last target sighting");
        }
        #endif
    }
}