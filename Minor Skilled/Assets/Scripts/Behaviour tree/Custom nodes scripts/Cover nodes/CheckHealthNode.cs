using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Cover/CheckHealthNode")]
public class CheckHealthNode : BehaviourNode
{
    private float _requiredHealthToFlee;

    public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        EnemyTankCombatState combatState = controller.GetState(E_TankState.Combat) as EnemyTankCombatState;
        float currentHealth = combatState.GetHealthPercentage();
        if (currentHealth <= _requiredHealthToFlee)
            _nodeState = NodeState.Success;

        return _nodeState;
    }
}