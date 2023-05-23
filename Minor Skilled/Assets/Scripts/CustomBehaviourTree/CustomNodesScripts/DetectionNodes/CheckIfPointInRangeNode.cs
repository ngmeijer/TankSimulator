using UnityEditor;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckIfPointInRangeNode")]
    public class CheckIfPointInRangeNode : BehaviourNode
    {
        public CustomKeyValue MinRange;
        public CustomKeyValue MaxRange;

        [SerializeField] private Color _rangeDiscColour;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = PointCheck.PointInRange(blackboard.PointToRotateTurretTo, controller.transform.position,
                MinRange.Value, MaxRange.Value)
                ? NodeState.Success
                : NodeState.Failure;

            return _nodeState;
        }

        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            base.DrawGizmos(blackboard, controller);

            Handles.color = _rangeDiscColour;
            Handles.DrawWireDisc(controller.transform.position, controller.transform.up, MaxRange.Value);

            if (MaxRange.Name == "")
                return;
            Handles.Label(controller.transform.position + controller.transform.right *
                MaxRange.Value, $"{MaxRange.Name}: {MaxRange.Value}");
        }
    }
}