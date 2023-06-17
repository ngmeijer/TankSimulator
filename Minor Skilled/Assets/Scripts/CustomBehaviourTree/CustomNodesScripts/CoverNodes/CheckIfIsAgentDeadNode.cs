using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.CoverNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Cover/CheckIfIsDeadNode")]
    public class CheckIfIsAgentDeadNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if (controller.ComponentManager.HasDied)
                _nodeState = NodeState.Success;
            else _nodeState = NodeState.Failure;
            
            return _nodeState;
        }
    }
}