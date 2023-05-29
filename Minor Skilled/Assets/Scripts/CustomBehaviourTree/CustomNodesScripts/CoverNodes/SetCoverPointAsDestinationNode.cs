using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.PatrolNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Cover/SetCoverPointAsDestination")]
    public class SetCoverPointAsDestinationNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            blackboard.MoveToPosition = blackboard.CurrentCoverPoint;

            _nodeState = NodeState.Success;
            return _nodeState;
        }
    }
}