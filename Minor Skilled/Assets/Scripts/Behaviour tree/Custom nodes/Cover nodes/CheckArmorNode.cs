using UnityEngine;


[CreateAssetMenu(menuName = "Behaviour tree/Cover/CheckArmorNode")]
public class CheckArmorNode : CombatNode
{
    private float _requiredArmorToFlee;

    public override NodeState Evaluate()
    {
        float currentHealth = _combatState.GetArmorPercentage();
        if (currentHealth <= _requiredArmorToFlee)
            _nodeState = NodeState.Success;

        return _nodeState;
    }
}