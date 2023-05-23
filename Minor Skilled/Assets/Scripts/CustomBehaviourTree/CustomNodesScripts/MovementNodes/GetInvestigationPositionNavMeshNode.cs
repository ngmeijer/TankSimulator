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
            Vector3 currentTargetPosition = SamplePosition(blackboard.PointToRotateTurretTo);
            if (currentTargetPosition != Vector3.zero)
            {
                blackboard.PointToRotateTurretTo = currentTargetPosition;
                _nodeState = NodeState.Success;
            }
            else _nodeState = NodeState.Failure;

            return _nodeState;
        }

        private Vector3 SamplePosition(Vector3 position)
        {
            NavMesh.SamplePosition(position, out NavMeshHit hit, MaxInvestigationRange.Value, 1);
            return hit.position;
        }
    }
}