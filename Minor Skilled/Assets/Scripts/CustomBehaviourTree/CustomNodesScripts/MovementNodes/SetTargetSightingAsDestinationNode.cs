using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/SetTargetSightingAsDestinationNode")]
    public class SetTargetSightingAsDestinationNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            blackboard.MoveToPosition = blackboard.LastTargetSighting;

            _nodeState = NodeState.Success;
            return _nodeState;
        }
    }
}