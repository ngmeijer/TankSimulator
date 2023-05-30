using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/SetCanGenerateNavPoints")]
    public class SetCanGenerateNavPointsNode : BehaviourNode
    {
        [SerializeField] private bool _canGenerate;
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            blackboard.CanGenerateNavPoints = _canGenerate;
            _nodeState = NodeState.Success;
            return _nodeState;
        }
    }
}