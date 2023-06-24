using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Decorator/ForceSuccess")]
public class ForceSuccessNode : BehaviourNode
{
    public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        return _nodeState = NodeState.Success;
    }
}