using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Shooting/CombatNode")]
public class CombatNode : BehaviourNode 
{
    protected EnemyTankCombatState _combatState;

    protected virtual void OnEnable()
    {
        _combatState = _blackboard.AIController.StateSwitcher.CombatState as EnemyTankCombatState;
        Debug.Log($"Testing blackboard: {_blackboard}");
    }

    public override NodeState Evaluate()
    {
        return _nodeState;
    }
}