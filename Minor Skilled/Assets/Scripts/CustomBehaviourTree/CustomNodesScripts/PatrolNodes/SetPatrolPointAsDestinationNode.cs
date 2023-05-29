using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.PatrolNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Patrolling/SetPatrolPointAsDestination")]
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