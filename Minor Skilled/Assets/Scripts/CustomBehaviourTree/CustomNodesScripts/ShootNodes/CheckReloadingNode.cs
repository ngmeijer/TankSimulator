using FSM.TankStates;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.ShootNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Shooting/CheckReloadingNode")]
    public class CheckReloadingNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = IsReloading(controller) ? NodeState.Failure : NodeState.Success;

            return _nodeState;
        }

        private bool IsReloading(AIController controller)
        {
            EnemyTankCombatState combatState = controller.GetState(E_TankState.Combat) as EnemyTankCombatState;
            return combatState.IsReloading();
        }
    }
}