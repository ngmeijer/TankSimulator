public class FindSuitableCoverNode : CombatNode
{
    public FindSuitableCoverNode(AIBlackboard blackboard) : base(blackboard)
    {
        
    }

    public override NodeState Evaluate()
    {
        return _nodeState;
    }

    public override void DrawGizmos()
    {
        
    }
}