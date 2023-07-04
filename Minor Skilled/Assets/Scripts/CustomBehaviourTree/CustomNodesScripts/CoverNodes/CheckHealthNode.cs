using FSM.TankStates;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.CoverNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Cover/CheckHealthNode")]
    public class CheckHealthNode : BehaviourNode
    {
        private float _requiredHealthToFlee;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            EnemyCombatState combatState = controller.GetState(E_TankState.Combat) as EnemyCombatState;
            float currentHealth = combatState.GetHealthPercentage();
            if (currentHealth <= _requiredHealthToFlee)
                _nodeState = NodeState.Success;

            return _nodeState;
        }
    }
}