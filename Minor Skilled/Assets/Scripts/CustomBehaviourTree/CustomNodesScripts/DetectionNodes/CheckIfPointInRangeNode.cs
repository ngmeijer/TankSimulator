using CustomBehaviourTree.CustomNodesScripts.CoverNodes;
using UnityEditor;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckIfPointInRangeNode")]
    public class CheckIfPointInRangeNode : CheckRangeBaseNode
    {
        public CustomKeyValue MinRange;
        
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = PointCheck.PointInRange(blackboard.PointToRotateTurretTo, controller.transform.position,
                MinRange.Value, MaxRange.Value)
                ? NodeState.Success
                : NodeState.Failure;

            return _nodeState;
        }
    }
}