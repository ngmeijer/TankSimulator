using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.CoverNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Cover/CheckIfHasValidCoverPoint")]
    public class CheckIfHasValidCoverPointNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if (blackboard.CurrentCoverPoint != Vector3.zero)
            {
                blackboard.MoveToPosition = blackboard.CurrentCoverPoint;
                _nodeState = NodeState.Success;
            }
            
            return _nodeState;
        }
    }
}