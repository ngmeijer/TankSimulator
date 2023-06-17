using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.NavMeshNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/StopMovement")]
    public class StopMovementNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            blackboard.GeneratedNavPath = null;
            blackboard.MoveToPosition = Vector3.zero;
            controller.NavAgent.isStopped = true;
            controller.NavAgent.ResetPath();
            controller.NavAgent.enabled = false;
            controller.NavAgent.enabled = true;
            
            _nodeState = NodeState.Success;
            return _nodeState;
        }
    }
}