using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckIfPlayerIsAlive")]
    public class CheckIfPlayerIsAliveNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if (GameManager.Instance.Player.HasDied)
                _nodeState = NodeState.Failure;
            else _nodeState = NodeState.Success;

            return _nodeState;
        }
    }
}