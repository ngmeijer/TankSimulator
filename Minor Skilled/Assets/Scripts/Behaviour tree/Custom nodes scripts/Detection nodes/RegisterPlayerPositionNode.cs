using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Detection/RegisterPlayerPosition")]
public class RegisterPlayerPositionNode : BehaviourNode
{
    public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        _nodeState = NodeState.Success;
        blackboard.TargetInvestigatePosition = GameManager.Instance.Player.transform.position;
        
        return _nodeState;
    }
}