using UnityEditor;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/CheckIfReachedDestinationNode")]
    public class CheckIfReachedDestinationNode : BehaviourNode
    {
        [SerializeField] private CustomKeyValue MaxStoppingDistance;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if (CheckIfReachedDestination(blackboard.MoveToPosition, blackboard, controller))
                _nodeState = NodeState.Success;

            return _nodeState;
        }
        
        private bool CheckIfReachedDestination(Vector3 destination, AIBlackboard blackboard, AIController controller)
        {
            controller.NavAgent.stoppingDistance = MaxStoppingDistance.Value;
            float distanceToTargetPos = Vector3.Distance(controller.transform.position, destination);
            if (distanceToTargetPos <= MaxStoppingDistance.Value)
            {
                blackboard.CurrentPatrolPoint = Vector3.zero;
                controller.NavAgent.ResetPath();
                _nodeState = NodeState.Success;
                return true;
            }

            return false;
        }

        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(controller.transform.position, controller.transform.up, MaxStoppingDistance.Value);
            Handles.Label(controller.transform.position + Vector3.right * MaxStoppingDistance.Value, $"{MaxStoppingDistance.Name}: {MaxStoppingDistance.Value}");
        }
    }
}