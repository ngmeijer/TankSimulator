using UnityEditor;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckIfPointInRangeNode")]
    public class CheckIfPointInRangeNode : BehaviourNode
    {
        public CustomKeyValue MinRange;
        public CustomKeyValue MaxRange;
    
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = PointInRange(blackboard, controller) ? NodeState.Success : NodeState.Failure;

            return _nodeState;
        }

        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            base.DrawGizmos(blackboard, controller);

            Handles.color = SolidGizmoColor;
            Handles.DrawWireDisc(controller.transform.position, controller.transform.up, MaxRange.Value);
        
            if (MaxRange.Name == "")
                return;
            Handles.Label(controller.transform.position + controller.transform.right * 
                MaxRange.Value, $"{MaxRange.Name}: {MaxRange.Value}");
        }

        private bool PointInRange(AIBlackboard blackboard, AIController controller)
        {
            float distance =
                Vector3.Distance(blackboard.InvestigationFocusPoint, controller.transform.position);

            return distance >= MinRange.Value && distance <= MaxRange.Value;
        }
    }
}