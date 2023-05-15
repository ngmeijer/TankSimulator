using UnityEngine;


[CreateAssetMenu(menuName = "Behaviour tree/Cover/CheckArmorNode")]
public class CheckArmorNode : BehaviourNode
{
    private float _requiredArmorToFlee;

    public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        EnemyTankCombatState combatState = controller.GetState(E_TankState.Combat) as EnemyTankCombatState;
        float currentHealth = combatState.GetArmorPercentage();
        if (currentHealth <= _requiredArmorToFlee)
            _nodeState = NodeState.Success;

        return _nodeState;
    }
}