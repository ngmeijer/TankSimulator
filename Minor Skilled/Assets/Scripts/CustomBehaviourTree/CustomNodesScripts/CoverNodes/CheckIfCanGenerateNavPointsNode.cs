using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/CheckIfCanGenerateNavPoints")]
    public class CheckIfCanGenerateNavPointsNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if (blackboard.CanGenerateNavPoints)
                _nodeState = NodeState.Success;
            else _nodeState = NodeState.Failure;
            
            return _nodeState;
        }
    }
}