using UnityEngine;

public class CheckIfPlayerInRangeNode : BehaviourNode
{
    public CheckIfPlayerInRangeNode()
    {
        
    }
    
    public override NodeState Evaluate(AIBlackboard blackboard)
    {
        _nodeState = PlayerInRange(blackboard) ? NodeState.Success : NodeState.Failure;

        return _nodeState;
    }

    public override void DrawGizmos()
    {
        
    }

    private bool PlayerInRange(AIBlackboard blackboard)
    {
        float distance =
            Vector3.Distance(GameManager.Instance.Player.transform.position, blackboard.ThisTrans.position);

        return distance <= blackboard.MaxInstantVisionRange;
    }
}