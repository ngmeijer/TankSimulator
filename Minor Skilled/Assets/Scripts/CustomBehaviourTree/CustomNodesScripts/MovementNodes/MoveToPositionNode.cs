using CustomBehaviourTree.CustomNodesScripts.DetectionNodes;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace CustomBehaviourTree.CustomNodesScripts.NavMeshNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/MoveToPositionNode")]
    public class MoveToPositionNode : BehaviourNode
    {
        private float _currentTime;
        private Vector3 _currentMoveToPosition;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = NodeState.Running;
            
            //Did the target position not change? Then don't recalculate path
            if (blackboard.MoveToPosition == _currentMoveToPosition)
            {
                return _nodeState;
            }
            
            //If a path is available and the agent has no path yet, set one
            if (blackboard.GeneratedNavPath != null && controller.NavAgent.path != null)
            {
                controller.NavAgent.SetPath(blackboard.GeneratedNavPath);
                return _nodeState;
            }
            
            _currentTime += Time.deltaTime;
            if (_currentTime < blackboard.PathCalculationInterval)
                return _nodeState;

            CalculateNewPath(blackboard, controller);

            return _nodeState;
        }

        private void CalculateNewPath(AIBlackboard blackboard, AIController controller)
        {
            _currentTime = 0f;
            NavMeshPath path = new();
            _currentMoveToPosition = blackboard.MoveToPosition;
            controller.NavAgent.CalculatePath(blackboard.MoveToPosition, path);
            controller.NavAgent.SetPath(path);
        }

        public override void ResetValues()
        {
            _currentTime = 0f;
            _currentMoveToPosition = Vector3.zero;
        }

        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
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