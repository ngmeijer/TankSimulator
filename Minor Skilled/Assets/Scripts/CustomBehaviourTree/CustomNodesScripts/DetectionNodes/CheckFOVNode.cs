using UnityEditor;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckFOVNode")]
    public class CheckFOVNode : BehaviourNode
    {
        private Vector3 _targetPosition;

        [SerializeField] private Color _fovConeColour;
        [field: SerializeField] public string RangeName { set; get; }
        [field: SerializeField] public float RangeValue { set; get; }

        [field: SerializeField] private float _viewAngle;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = PointCheck.IsPointInsideFOV(blackboard.PointToRotateTurretTo, controller.transform, _viewAngle)
                ? NodeState.Success
                : NodeState.Failure;

            return _nodeState;
        }

        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            Transform turretTransform = controller.ComponentManager.TurretControlComponent.TurretTransform;
            Handles.color = _fovConeColour;
            Handles.DrawSolidArc(controller.transform.position, controller.transform.up, turretTransform.forward,
                -_viewAngle / 2, RangeValue);
            Handles.DrawSolidArc(controller.transform.position, controller.transform.up, turretTransform.forward,
                _viewAngle / 2, RangeValue);
            Handles.Label(controller.transform.position + Vector3.right * RangeValue, $"{RangeName}: {RangeValue}");
        }
    }
}