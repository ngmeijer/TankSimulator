using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/SamplePointsInRadiusNode")]
    public class SamplePointsInRadiusNode : BehaviourNode
    {
        [SerializeField] private CustomKeyValue PatrolRange;
        [SerializeField] private int _maxPointsGenerated;
        [SerializeField] private List<Vector3> _pointsGenerated = new List<Vector3>();
        [SerializeField] private float _minDistanceBetweenPoints;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _pointsGenerated.Clear();
            _pointsGenerated.Add(controller.transform.position);
            List<Vector3> _tempPoints = new List<Vector3>();

            for (int i = 0; i < _maxPointsGenerated; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere;
                Vector3 potentialPoint = controller.transform.position + randomDirection * PatrolRange.Value;

                if (NavMesh.SamplePosition(potentialPoint, out NavMeshHit hit, PatrolRange.Value, NavMesh.AllAreas))
                {
                    potentialPoint = hit.position;

                    bool isValidPoint = true;
                    foreach (var point in _pointsGenerated)
                    {
                        float distanceToPotentialPoint = Vector3.Distance(point, potentialPoint);
                        if (distanceToPotentialPoint < _minDistanceBetweenPoints)
                        {
                            isValidPoint = false;
                            break;
                        }
                    }

                    if (isValidPoint)
                    {
                        _tempPoints.Add(potentialPoint);
                    }
                }
            }

            _pointsGenerated.AddRange(_tempPoints);
            _pointsGenerated.Remove(controller.transform.position);
            blackboard.PatrolPoints = _pointsGenerated;



            return _nodeState;
        }

        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            base.DrawGizmos(blackboard, controller);

            if(PatrolRange.Name != "")
                Handles.Label(controller.transform.position + controller.transform.forward * PatrolRange.Value, $"{PatrolRange.Name}: {PatrolRange.Value}");
            
            Handles.color = Color.cyan;
            Handles.DrawWireDisc(controller.transform.position, controller.transform.up, PatrolRange.Value);

            foreach (var point in _pointsGenerated)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(point, 0.5f);
            }
        }
    }
}