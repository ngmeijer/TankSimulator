using TankComponents;
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
                _nodeState = NodeState.Failure;
            else _nodeState = NodeState.Success;
            
            return _nodeState;
        }
    }
}