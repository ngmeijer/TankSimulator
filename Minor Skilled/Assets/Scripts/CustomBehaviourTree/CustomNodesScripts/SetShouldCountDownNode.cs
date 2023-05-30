using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts
{
    [CreateAssetMenu(menuName = "Behaviour tree/Utility/SetShouldCountDown")]
    public class SetShouldCountDownNode : BehaviourNode
    {
        [SerializeField] private bool _shouldCountDown;
        
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            blackboard.ShouldCountDown = _shouldCountDown;
            _nodeState = NodeState.Success;
            Debug.Log($"enabled should wait. {_nodeState}");
            return _nodeState;
        }
    }
}