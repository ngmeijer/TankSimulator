using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Cover/CheckHealthNode")]
public class CheckHealthNode : CombatNode
{
    private float _requiredHealthToFlee;

    public override NodeState Evaluate()
    {
        float currentHealth = _combatState.GetHealthPercentage();
        if (currentHealth <= _requiredHealthToFlee)
            _nodeState = NodeState.Success;

        return _nodeState;
    }
}