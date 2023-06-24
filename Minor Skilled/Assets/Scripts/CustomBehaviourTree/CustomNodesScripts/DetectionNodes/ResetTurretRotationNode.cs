using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.PatrolNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Patrolling/ResetTurretRotation")]
    public class ResetTurretRotationNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = NodeState.Success;
            blackboard.PointToRotateTurretTo = controller.transform.position + controller.transform.forward * 10f;
        
            return _nodeState;
        }
    }
}