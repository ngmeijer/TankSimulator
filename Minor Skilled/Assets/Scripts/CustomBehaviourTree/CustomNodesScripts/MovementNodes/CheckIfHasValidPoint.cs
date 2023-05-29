using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace CustomBehaviourTree.CustomNodesScripts.NavMeshNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/CheckIfHasValidMoveToPoint")]
    public class CheckIfHasValidPoint : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if (blackboard.MoveToPosition == Vector3.zero)
                _nodeState = NodeState.Failure;
            else
            {
                if (CheckIfPointIsValid(blackboard.MoveToPosition, blackboard, controller))
                    _nodeState = NodeState.Success;
                else _nodeState = NodeState.Failure;
            }
            
            return _nodeState;
        }

        private bool CheckIfPointIsValid(Vector3 movePosition, AIBlackboard blackboard, AIController controller)
        {
            NavMeshPath path = new();
            bool hasPath = controller.NavAgent.CalculatePath(movePosition, path);
            if (!hasPath)
                return false;
            
            //A path has been calculated for a point that's inside/under an object (mountain/hills, house)
            //so the point cannot be reached. Get a new point.
            if (path.status is NavMeshPathStatus.PathInvalid or NavMeshPathStatus.PathPartial) 
                return false;

            blackboard.GeneratedNavPath = path;
            
            return true;
        }

        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            if (blackboard.MoveToPosition != Vector3.zero)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(blackboard.MoveToPosition, 2f);
            }
        }
    }
}