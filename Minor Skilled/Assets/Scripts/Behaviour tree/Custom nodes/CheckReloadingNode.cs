public class CheckReloadingNode : BehaviourNode
{
    private EnemyTankCombatState _combatState;

    public CheckReloadingNode(AIBlackboard blackboard) : base(blackboard)
    {
        _combatState = _blackboard.AIController.StateSwitcher.CombatState as EnemyTankCombatState;
    }

    public override NodeState Evaluate()
    {
        _nodeState = IsReloading() ? NodeState.Failure : NodeState.Success;

        return _nodeState;
    }

    private bool IsReloading()
    {
        return _combatState.IsReloading();
    }

    public override void DrawGizmos()
    {
        
    }
}