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

            _nodeState = NodeState.Success;
            return _nodeState;
        }
    }
}