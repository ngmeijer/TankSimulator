using UnityEditor;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/SetPlayerAsTurretFocus")]
    public class SetPlayerAsTurretFocusNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = NodeState.Success;
            blackboard.PointToRotateTurretTo = GameManager.Instance.Player.transform.position;
        
            return _nodeState;
        }
    }
}