using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.CoverNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Cover/CheckIfInCover")]
    public class CheckIfInCoverNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            bool hasHitPlayer = false;
            Color hitColor = Color.magenta;

            foreach (var raycastTransform in controller.CoverRaycastPositions)
            {
                bool hasHitCollider = Physics.Linecast(raycastTransform.position, GameManager.Instance.Player.EntityOrigin.position, 1 << 7);
                if (!hasHitCollider)
                    continue;
                
                hitColor = Color.red; 
                hasHitPlayer = true;
                
                Debug.DrawLine(raycastTransform.position, GameManager.Instance.Player.EntityOrigin.position, hitColor);
            }
            
            //Ray has hit player? Continue with cover finding branch.
            _nodeState = hasHitPlayer ? NodeState.Failure : NodeState.Success;

            return _nodeState;
        }
    }
}