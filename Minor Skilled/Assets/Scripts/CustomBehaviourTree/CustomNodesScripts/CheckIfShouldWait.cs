using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts
{
    [CreateAssetMenu(menuName = "Behaviour tree/Utility/ChecKIfShouldWait")]
    public class CheckIfShouldWait : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if (blackboard.ShouldCountDown)
                _nodeState = NodeState.Success;
            else _nodeState = NodeState.Failure;
            
            return _nodeState;
        }
    }
}