using UnityEditor;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.NavMeshNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/CheckIfReachedDestinationNode")]
    public class CheckIfReachedDestinationNode : BehaviourNode
    {
        [SerializeField] private CustomKeyValue MaxStoppingDistance;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if (CheckIfReachedDestination(ref blackboard.MoveToPosition, blackboard, controller))
                _nodeState = NodeState.Success;
            else _nodeState = NodeState.Failure;
            
            return _nodeState;
        }
        
        private bool CheckIfReachedDestination(ref Vector3 destination, AIBlackboard blackboard, AIController controller)
        {
            controller.NavAgent.stoppingDistance = MaxStoppingDistance.Value;
            float distanceToTargetPos = Vector3.Distance(controller.transform.position, destination);
            if (distanceToTargetPos > MaxStoppingDistance.Value)
                return false;
            
            destination = Vector3.zero;
            blackboard.CurrentPatrolPoint = Vector3.zero; 
            controller.NavAgent.ResetPath(); 
            return true;
        }

        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(controller.transform.position, controller.transform.up, MaxStoppingDistance.Value);
            Handles.Label(controller.transform.position + Vector3.right * MaxStoppingDistance.Value, $"{MaxStoppingDistance.Name}: {MaxStoppingDistance.Value}");
        }
    }
}