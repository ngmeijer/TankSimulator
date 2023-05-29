using UnityEditor;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.CoverNodes
{
    public class CheckRangeBaseNode : BehaviourNode
    {
        public CustomKeyValue MaxRange;
        [SerializeField] private Color _rangeDiscColour;
        
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