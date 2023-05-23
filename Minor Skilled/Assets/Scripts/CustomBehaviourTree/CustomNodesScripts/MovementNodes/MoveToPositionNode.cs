using CustomBehaviourTree.CustomNodesScripts.DetectionNodes;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/MoveToPositionNode")]
    public class MoveToPositionNode : BehaviourNode
    {
        private Vector3 _targetPos;
        private float _currentTime;
        [SerializeField] private CustomKeyValue MaxStoppingDistance;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            float distanceToTargetPos = Vector3.Distance(controller.transform.position, blackboard.LastTargetSighting);
            if (distanceToTargetPos <= MaxStoppingDistance.Value)
            {
                controller.NavAgent.ResetPath();
                _nodeState = NodeState.Success;
                return _nodeState;
            }

            _nodeState = NodeState.Running;
            _currentTime += Time.deltaTime;
            if (_currentTime > blackboard.PathCalculationInterval)
            {
                NavMeshPath path = new();
                _currentTime = 0f;
                controller.NavAgent.CalculatePath(blackboard.LastTargetSighting, path);
                controller.NavAgent.SetPath(path);
            }
        
            return _nodeState;
        }

        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(controller.transform.position, controller.transform.up, MaxStoppingDistance.Value);
            Handles.Label(controller.transform.position + Vector3.right * MaxStoppingDistance.Value, $"{MaxStoppingDistance.Name}: {MaxStoppingDistance.Value}");

            if (controller.NavAgent.path != null)
            {
                Vector3[] pathPositions = controller.NavAgent.path.corners;
            
                Gizmos.color = Color.yellow;
                for(int i = 0; i < pathPositions.Length; i++)
                {
                    Handles.Label(pathPositions[i] + GameManager.HandlesOffset, $"Path point {i} of {pathPositions.Length - 1}");
                    Gizmos.DrawSphere(pathPositions[i], 0.5f);

                    if (i >= 1)
                        Debug.DrawLine(pathPositions[i - 1], pathPositions[i], Color.red);
                }
            }
        }
    }
}