using CustomBehaviourTree.CustomNodesScripts.DetectionNodes;
using UnityEditor;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/GetBestPatrolPoint")]
    public class GetBestPatrolPoint : SequenceNode
    {
        [SerializeField] private CustomKeyValue _viewAngle;
        [SerializeField] private CustomKeyValue _viewRange;

        [SerializeField] private Color _fovConeColour;
        
        [Header("Sorted based on priority. Point should first be in FOV, then Line of Sight.")]
        [SerializeField] private bool CheckFOV;
        [SerializeField] private bool CheckLineOfSight;
        private Vector3 _bestPatrolPoint;
        
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            foreach (var patrolPoint in blackboard.GeneratedPatrolPoints)
            {
                if (!CheckFOV)
                    continue;
                if (!PointCheck.IsPointInsideFOV(patrolPoint, controller.transform, _viewAngle.Value))
                    continue;

                if (!CheckLineOfSight)
                    continue;
                if (!PointCheck.HasLineOfSight(controller.ComponentManager.Raycaster.position, patrolPoint))
                    continue;

                _bestPatrolPoint = patrolPoint;
                _nodeState = NodeState.Success;
            }

            blackboard.CurrentPatrolPoint = _bestPatrolPoint;
            
            return _nodeState;
        }
        
        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            Transform turretTransform = controller.ComponentManager.TurretControlComponent.TurretTransform;
            Handles.color = _fovConeColour;
            Handles.DrawSolidArc(controller.transform.position, controller.transform.up, turretTransform.forward,
                -_viewAngle.Value / 2, _viewRange.Value);
            Handles.DrawSolidArc(controller.transform.position, controller.transform.up, turretTransform.forward,
                _viewAngle.Value / 2, _viewRange.Value);
            Handles.Label(controller.transform.position + Vector3.right * _viewRange.Value, $"{_viewAngle.Name}: {_viewAngle.Value}");
        }
    }
}