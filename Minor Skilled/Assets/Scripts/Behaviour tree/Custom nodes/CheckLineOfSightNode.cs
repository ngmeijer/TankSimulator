using UnityEngine;

namespace Behaviour_tree.Custom_nodes
{
    public class CheckLineOfSightNode : BehaviourNode
    {
        public CheckLineOfSightNode(AIBlackboard blackboard) : base(blackboard)
        {
        }

        public override NodeState Evaluate()
        {
            _nodeState = CanSeePlayer() ? NodeState.Success : NodeState.Failure;
        
            return _nodeState;
        }
    
        private bool CanSeePlayer()
        {
            bool hitCollider = Physics.Linecast(
                _blackboard.Raycaster.position,
                GameManager.Instance.Player.EntityOrigin.position, out RaycastHit hit, 1);
        
            return hitCollider && hit.collider.transform.root.CompareTag("Player");
        }

        public override void DrawGizmos()
        {
            if (_nodeState == NodeState.Success)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
        
            Gizmos.DrawLine(_blackboard.Raycaster.position, GameManager.Instance.Player.EntityOrigin.position);
        }
    }
}