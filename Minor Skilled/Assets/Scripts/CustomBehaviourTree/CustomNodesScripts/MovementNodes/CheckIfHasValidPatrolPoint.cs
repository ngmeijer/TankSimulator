using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/CheckIfHasValidPatrolPoint")]
    public class CheckIfHasValidPatrolPoint : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if (blackboard.CurrentPatrolPoint == Vector3.zero)
                _nodeState = NodeState.Success;
            else
            {
                if (CheckIfPointIsValid(blackboard.CurrentPatrolPoint, controller))
                    _nodeState = NodeState.Failure;
                else _nodeState = NodeState.Success;
            }
            
            return _nodeState;
        }

        private bool CheckIfPointIsValid(Vector3 patrolPoint, AIController controller)
        {
            NavMeshPath path = new();
            bool hasPath = controller.NavAgent.CalculatePath(patrolPoint, path);
            if (!hasPath)
                return false;
            
            if (path.status is NavMeshPathStatus.PathInvalid or NavMeshPathStatus.PathPartial) 
                return false; 
            return true;
        }
        
        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            if (blackboard.CurrentPatrolPoint == Vector3.zero)
                return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(blackboard.CurrentPatrolPoint, 1f);
            Handles.Label(blackboard.CurrentPatrolPoint + GameManager.HandlesOffset * 2f, "Current patrol point");
        }
    }
}