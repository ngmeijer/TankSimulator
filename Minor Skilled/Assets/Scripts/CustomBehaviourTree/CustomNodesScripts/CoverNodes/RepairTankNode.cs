using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.CoverNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Cover/RepairTank")]
    public class RepairTankNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            EnemyTankCombatState combatState = controller.GetState(E_TankState.Combat) as EnemyTankCombatState;
            if (combatState.GetArmorPercentage() < 1f)
            {
                blackboard.IsCurrentlyRepairing = true;
                _nodeState = NodeState.Success;
                combatState.RepairTank();
                
                return _nodeState;
            }

            blackboard.IsCurrentlyRepairing = false;
            _nodeState = NodeState.Failure;
            return _nodeState;
        }
    }
}