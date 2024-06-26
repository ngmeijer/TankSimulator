﻿using FSM.TankStates;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.ShootNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Shooting/FireShellNode")]
    public class FireShellNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            EnemyCombatState combatState = controller.GetState(E_TankState.Combat) as EnemyCombatState;
            combatState.FireShell();

            _nodeState = NodeState.Success;
            return _nodeState;
        }
    }
}