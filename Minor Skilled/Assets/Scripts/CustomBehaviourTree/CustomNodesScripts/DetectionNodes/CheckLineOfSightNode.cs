using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckLineOfSightNode")]
    public class CheckLineOfSightNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if (PointCheck.HasLineOfSight(controller.ComponentManager.Raycaster.position,
                    GameManager.Instance.Player.EntityOrigin.position,
                    LayerMask.NameToLayer("Player")
                ))
            {
                _nodeState = NodeState.Success;
            }
            else
            {
                _nodeState = NodeState.Failure;
            }


            return _nodeState;
        }

        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            if (_nodeState == NodeState.Success)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            if (GameManager.Instance == null)
                return;

            Gizmos.DrawLine(controller.ComponentManager.Raycaster.position,
                GameManager.Instance.Player.EntityOrigin.position);
        }
    }
}