using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/SetPlayerAsDefinitiveFocus")]
    public class SetPlayerAsDefinitiveFocusNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = NodeState.Success;
            blackboard.DefiniteFocusPoint = GameManager.Instance.Player.transform.position;
        
            return _nodeState;
        }
    }
}