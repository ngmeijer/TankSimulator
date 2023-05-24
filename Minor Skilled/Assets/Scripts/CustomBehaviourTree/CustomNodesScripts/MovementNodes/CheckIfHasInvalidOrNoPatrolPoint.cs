using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/CheckIfHasValidPatrolPoint")]
    public class CheckIfHasInvalidOrNoPatrolPoint : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            //If the agent has NO patrol point, the node returns failure and the parent selector node will continue. (Sample new positions)
            if (blackboard.CurrentPatrolPoint == Vector3.zero)
                _nodeState = NodeState.Failure;
            else
            {
                if (CheckIfPointIsValid(blackboard.CurrentPatrolPoint, blackboard, controller))
                    _nodeState = NodeState.Success;
                else _nodeState = NodeState.Failure;
            }
            
            return _nodeState;
        }

        private bool CheckIfPointIsValid(Vector3 patrolPoint, AIBlackboard blackboard, AIController controller)
        {
            NavMeshPath path = new();
            bool hasPath = controller.NavAgent.CalculatePath(patrolPoint, path);
            if (!hasPath)
                return false;
            
            //A path has been calculated for a point that's inside/under an object (mountain, hills)
            //so the point cannot be reached. Get a new point.
            if (path.status is NavMeshPathStatus.PathInvalid or NavMeshPathStatus.PathPartial) 
                return false;

            blackboard.PatrolPath = path;
            
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