using System.Collections.Generic;
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
        
        private List<Vector3> _potentialPatrolPoints = new List<Vector3>();

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            ResetValues();
            CheckPatrolPointRequirements(blackboard, controller);

            if (_potentialPatrolPoints.Count != 0)
            {
                //Could also do this based on distance (closest to agent) but chose to do this for now.
                int randomPoint = Random.Range(0, _potentialPatrolPoints.Count - 1);
                blackboard.CurrentPatrolPoint = _potentialPatrolPoints[randomPoint];
            }
            
            //If the previous loop didn't yield any results again (even more unlikely)
            //just pick a random point.
            if (_potentialPatrolPoints.Count == 0)
            {
                int randomPoint = Random.Range(0, blackboard.GeneratedPatrolPoints.Count - 1);
                blackboard.CurrentPatrolPoint = blackboard.GeneratedPatrolPoints[randomPoint];
            }

            if (blackboard.CurrentPatrolPoint != Vector3.zero)
                _nodeState = NodeState.Success;
            
            return _nodeState;
        }

        private void CheckPatrolPointRequirements(AIBlackboard blackboard, AIController controller)
        {
            //Get best point. Should be in FOV and have a line of sight. 
            foreach (var patrolPoint in blackboard.GeneratedPatrolPoints)
            {
                if (!PointCheck.IsPointInsideFOV(patrolPoint, controller.transform, _viewAngle.Value))
                    continue; 
                    
                if(!PointCheck.HasLineOfSight(controller.ComponentManager.Raycaster.position, patrolPoint))
                    continue;
                
                _potentialPatrolPoints.Add(patrolPoint);
            }

            //If the previous loop didn't yield any results (very unlikely it will happen but it's an edge case)
            //then run the loop again but remove the line of sight requirement
            if (_potentialPatrolPoints.Count != 0)
                return;
            
            foreach (var patrolPoint in blackboard.GeneratedPatrolPoints)
            { 
                if (!PointCheck.IsPointInsideFOV(patrolPoint, controller.transform, _viewAngle.Value)) 
                    continue;
                
                _potentialPatrolPoints.Add(patrolPoint);
            }
        }

        public override void ResetValues()
        {
            _potentialPatrolPoints = new List<Vector3>();
        }
        
        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            Transform turretTransform = controller.ComponentManager.TurretControlComponent.TurretTransform;
            Handles.color = _fovConeColour;
            Handles.DrawSolidArc(controller.transform.position, controller.transform.up, turretTransform.forward,
                -_viewAngle.Value / 2, _viewRange.Value);
            Handles.DrawSolidArc(controller.transform.position, controller.transform.up, turretTransform.forward,
                _viewAngle.Value / 2, _viewRange.Value);
        }
    }
}