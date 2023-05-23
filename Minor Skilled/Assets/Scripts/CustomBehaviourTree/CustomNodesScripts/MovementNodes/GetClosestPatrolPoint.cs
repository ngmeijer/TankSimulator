using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/GetClosestPatrolPoint")]
    public class GetClosestPatrolPoint : BehaviourNode
    {
        private Vector3 _bestPatrolPoint;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            float currentBestDistance = float.MaxValue;

            foreach (var patrolPoint in blackboard.GeneratedPatrolPoints)
            {
                float distance = Vector3.Distance(controller.transform.position, patrolPoint);
                if (distance < currentBestDistance)
                {
                    currentBestDistance = distance;
                    _bestPatrolPoint = patrolPoint;
                }
            }

            blackboard.CurrentPatrolPoint = _bestPatrolPoint;
            _nodeState = NodeState.Success;

            return _nodeState;
        }
    }
}