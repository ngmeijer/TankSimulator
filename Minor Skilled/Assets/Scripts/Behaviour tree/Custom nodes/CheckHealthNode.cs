public class CheckHealthNode : CombatNode
{
    private float _requiredHealthToFlee;
    
    public CheckHealthNode(AIBlackboard blackboard, float requiredHealthToFlee) : base(blackboard)
    {
        _requiredHealthToFlee = requiredHealthToFlee;
    }

    public override NodeState Evaluate()
    {
        float currentHealth = _combatState.GetHealthPercentage();
        if (currentHealth <= _requiredHealthToFlee)
            _nodeState = NodeState.Success;

        return _nodeState;
    }
}