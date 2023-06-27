using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckIfHasBeenShot")]
    public class CheckIfHasBeenShotNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            DamageRegistrationComponent component = controller.ComponentManager.DamageComp;
            if (component.RecentlyShot)
                _nodeState = NodeState.Success;
            else _nodeState = NodeState.Failure;
            
            return _nodeState;
        }
    }
}