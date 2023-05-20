using CustomBehaviourTree.CustomNodesScripts.DetectionNodes;
using UnityEngine;
using UnityEngine.AI;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/GetInvestigationPositionNavMeshNode")]
    public class GetInvestigationPositionNavMeshNode : BehaviourNode
    {
        [SerializeField] private CustomKeyValue MaxInvestigationRange;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            Vector3 currentTargetPosition = SamplePosition(blackboard);
            if (currentTargetPosition != Vector3.zero)
            {
                blackboard.MoveToPosition = currentTargetPosition;
                blackboard.DefiniteFocusPoint = currentTargetPosition;
                _nodeState = NodeState.Success;
            }
            else _nodeState = NodeState.Failure;

            return _nodeState;
        }

        private Vector3 SamplePosition(AIBlackboard blackboard)
        {
            NavMesh.SamplePosition(blackboard.InvestigationFocusPoint, out NavMeshHit hit, MaxInvestigationRange.Value, 1);
            return hit.position;
        }
    }
}