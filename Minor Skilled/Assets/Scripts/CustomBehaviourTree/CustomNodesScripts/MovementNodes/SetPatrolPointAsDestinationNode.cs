using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/SetPatrolPointAsDestinationNode")]
    public class SetPatrolPointAsDestinationNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            blackboard.MoveToPosition = blackboard.CurrentPatrolPoint;

            _nodeState = NodeState.Success;
            return _nodeState;
        }
    }
}