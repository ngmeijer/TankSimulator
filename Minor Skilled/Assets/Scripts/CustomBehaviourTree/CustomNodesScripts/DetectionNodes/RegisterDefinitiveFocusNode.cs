using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/RegisterPlayerPosition")]
    public class RegisterDefinitiveFocusNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = NodeState.Success;
            blackboard.DefiniteFocusPoint = blackboard.InvestigationFocusPoint;
        
            return _nodeState;
        }
    }
}