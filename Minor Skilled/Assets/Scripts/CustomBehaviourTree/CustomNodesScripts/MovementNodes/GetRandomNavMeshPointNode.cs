﻿using CustomBehaviourTree.CustomNodesScripts.DetectionNodes;
using UnityEngine;
using UnityEngine.AI;

namespace CustomBehaviourTree.CustomNodesScripts.NavMeshNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/GetRandomNavMeshPointNode")]
    public class GetRandomNavMeshPointNode : BehaviourNode
    {
        [SerializeField] private CustomKeyValue MaxPatrolRange;
        [SerializeField] private CustomKeyValue MinPatrolRange;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            // Vector3 currentTargetPosition = SamplePosition(blackboard, controller);
            // if (!CheckIfPointInFOV(blackboard, controller, currentTargetPosition))
            //     currentTargetPosition = SamplePosition(blackboard, controller);
            //
            // blackboard.MoveToPosition = currentTargetPosition;
            return _nodeState;
        }

        private Vector3 SamplePosition(AIBlackboard blackboard, AIController controller)
        {
            Vector3 finalPosition = Vector3.zero;
            Vector3 randomDirection = Random.insideUnitSphere * MaxPatrolRange.Value;
            Vector3 randomPosition = controller.transform.position + randomDirection;
            NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, MaxPatrolRange.Value, 1);
            float distanceToPosition = Vector3.Distance(controller.transform.position, hit.position);
            if (distanceToPosition < MinPatrolRange.Value)
                finalPosition = SamplePosition(blackboard, controller);
            else finalPosition = hit.position;

            return finalPosition;
        }
    }
}