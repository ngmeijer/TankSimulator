using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.NavMeshNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/StopMovementNode")]
    public class StopMovementNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            blackboard.GeneratedNavPath = null;
            blackboard.MoveToPosition = Vector3.zero;
            controller.NavAgent.isStopped = true;
            blackboard.CanGenerateNavPoints = false;

            _nodeState = NodeState.Success;
            return _nodeState;
        }
    }
}