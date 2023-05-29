using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.PatrolNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Patrolling/CopyPatrolPoints")]
    public class CopyPatrolPointsNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            blackboard.GeneratedPatrolPoints = blackboard.GenericPointsFound;
            
            _nodeState = NodeState.Success;
            return _nodeState;
        }
    }
}