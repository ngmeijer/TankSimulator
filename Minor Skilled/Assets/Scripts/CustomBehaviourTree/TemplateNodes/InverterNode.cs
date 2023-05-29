using UnityEngine;

namespace CustomBehaviourTree.TemplateNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Decorator/Inverter")]
    public class InverterNode : BehaviourNode
    {
        private BehaviourNode _node;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if(_childNodes.Count == 1)
                _node = _childNodes[0];
            else
            {
                Debug.Log($"Child node count of '{name}' is not 1. Ensure it has only & at least 1 child.");
                return _nodeState;
            }
            
            _nodeState = _node.Evaluate(blackboard, controller) switch
            {
                NodeState.Running => NodeState.Running,
                NodeState.Success => NodeState.Failure,
                NodeState.Failure => NodeState.Success,
                _ => _nodeState
            };

            return _nodeState;
        }
    }
}