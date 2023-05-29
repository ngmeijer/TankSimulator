namespace CustomBehaviourTree.CustomNodesScripts.CoverNodes
{
    public class RepairTankNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            EnemyTankCombatState combatState = controller.GetState(E_TankState.Combat) as EnemyTankCombatState;
            if (combatState.GetArmorPercentage() < 1f)
            {
                blackboard.IsCurrentlyRepairing = true;
                _nodeState = NodeState.Running;
                combatState.RepairTank();
                
                return _nodeState;
            }

            blackboard.IsCurrentlyRepairing = false;
            _nodeState = NodeState.Success;
            return _nodeState;
        }
    }
}