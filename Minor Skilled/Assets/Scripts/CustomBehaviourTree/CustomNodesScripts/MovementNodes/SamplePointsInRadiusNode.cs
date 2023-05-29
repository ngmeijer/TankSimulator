using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/SamplePointsInRadius")]
    public class SamplePointsInRadiusNode : BehaviourNode
    {
        [SerializeField] private CustomKeyValue MinPatrolRange;
        [SerializeField] private CustomKeyValue MaxPatrolRange;
        
        [SerializeField] private int _maxPointsAllowed;
        [SerializeField] private List<Vector3> _pointsGenerated = new List<Vector3>();
        [SerializeField] private Color _rangeDiscColour;
        [SerializeField] private Color _pointsGeneratedColour;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _pointsGenerated.Clear();
            _pointsGenerated.Add(controller.transform.position);
            List<Vector3> _tempPoints = new List<Vector3>();

            if (_maxPointsAllowed == 0)
            {
                Debug.Log($"MaxPointsAllowed in {name} is 0. You won't be seeing any generated NavMesh points :)");
                return _nodeState;
            }

            for (int i = 0; i < _maxPointsAllowed; i++)
            {
                Vector3 position = GetNavPosition(controller);
                if(position == Vector3.zero)
                    continue;
                
                _tempPoints.Add(position);
            }

            _pointsGenerated.AddRange(_tempPoints);
            _pointsGenerated.Remove(controller.transform.position);
            blackboard.GenericPointsFound = _pointsGenerated;

            _nodeState = NodeState.Success;

            return _nodeState;
        }

        private Vector3 GetNavPosition(AIController controller)
        {
            Vector3 randomDirection = Random.insideUnitSphere;
            Vector3 potentialPoint = controller.transform.position + randomDirection * MaxPatrolRange.Value;

            if (NavMesh.SamplePosition(potentialPoint, out NavMeshHit hit, MaxPatrolRange.Value, NavMesh.AllAreas))
            {
                potentialPoint = hit.position;
                    
                float distanceFromAgentToPoint = Vector3.Distance(controller.transform.position, potentialPoint);
                if (distanceFromAgentToPoint < MinPatrolRange.Value)
                {
                    return Vector3.zero;
                }

                return hit.position;
            }

            return Vector3.zero;
        }
        

        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            base.DrawGizmos(blackboard, controller);

            if(MaxPatrolRange.Name != "")
                Handles.Label(controller.transform.position + controller.transform.forward * MaxPatrolRange.Value, $"{MaxPatrolRange.Name}: {MaxPatrolRange.Value}");
            
            Handles.color = _rangeDiscColour;
            Handles.DrawWireDisc(controller.transform.position, controller.transform.up, MaxPatrolRange.Value);
            
            if(MinPatrolRange.Name != "")
                Handles.Label(controller.transform.position + controller.transform.forward * MinPatrolRange.Value, $"{MinPatrolRange.Name}: {MinPatrolRange.Value}");
            
            Handles.color = _rangeDiscColour;
            Handles.DrawWireDisc(controller.transform.position, controller.transform.up, MinPatrolRange.Value);

            if (_pointsGenerated != null)
            {
                foreach (var point in _pointsGenerated)
                {
                    Gizmos.color = _pointsGeneratedColour;
                    Gizmos.DrawSphere(point, 0.5f);
                }
            }
        }

        public override void ResetValues()
        {
            _pointsGenerated = new();
        }
    }
}