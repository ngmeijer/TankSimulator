public class CombatNode : BehaviourNode 
{
    protected EnemyTankCombatState _combatState;

    public CombatNode(AIBlackboard blackboard) : base(blackboard)
    {
        _combatState = _blackboard.StateSwitcher.CombatState as EnemyTankCombatState;
    }

    public override NodeState Evaluate()
    {
        return _nodeState;
    }

    public override void DrawGizmos()
    {
        
    }
}