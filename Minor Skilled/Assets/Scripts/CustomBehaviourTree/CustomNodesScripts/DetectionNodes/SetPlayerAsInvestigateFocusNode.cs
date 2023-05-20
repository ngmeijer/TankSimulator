using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/SetPlayerAsInvestigateFocus")]
    public class SetPlayerAsInvestigateFocusNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = NodeState.Success;
            blackboard.InvestigationFocusPoint = GameManager.Instance.Player.transform.position;
        
            return _nodeState;
        }
    }
}