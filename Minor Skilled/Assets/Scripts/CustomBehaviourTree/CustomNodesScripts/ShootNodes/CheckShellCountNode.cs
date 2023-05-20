using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.ShootNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Shooting/CheckShellCountNode")]
    public class CheckShellCountNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            _nodeState = NodeState.Failure;

            if (HasShells(controller))
                _nodeState = NodeState.Success;
        
            return _nodeState;
        }

        private bool HasShells(AIController controller)
        {
            EnemyTankCombatState combatState = controller.GetState(E_TankState.Combat) as EnemyTankCombatState;
            return combatState.HasShells();
        }
    }
}
