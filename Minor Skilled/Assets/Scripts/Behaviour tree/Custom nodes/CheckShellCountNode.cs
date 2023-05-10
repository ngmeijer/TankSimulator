public class CheckShellCountNode : CombatNode
{
    public CheckShellCountNode(AIBlackboard blackboard) : base(blackboard)
    {
        
    }

    public override NodeState Evaluate()
    {
        _nodeState = NodeState.Failure;

        if (HasShells())
            _nodeState = NodeState.Success;
        
        return _nodeState;
    }

    private bool HasShells()
    {
        return _combatState.HasShells();
    }

    public override void DrawGizmos()
    {
        
    }
}