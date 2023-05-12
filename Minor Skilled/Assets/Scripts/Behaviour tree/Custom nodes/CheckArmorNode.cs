using UnityEngine;

public class CheckArmorNode : CombatNode
{
    private float _requiredArmorToFlee;
    
    public CheckArmorNode(AIBlackboard blackboard, float requiredArmorToFlee) : base(blackboard)
    {
        _requiredArmorToFlee = requiredArmorToFlee;
    }

    public override NodeState Evaluate()
    {
        float currentHealth = _combatState.GetArmorPercentage();
        if (currentHealth <= _requiredArmorToFlee)
            _nodeState = NodeState.Success;

        return _nodeState;
    }
}