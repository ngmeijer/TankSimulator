﻿using FSM.TankStates;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.CoverNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Cover/CheckArmorNode")]
    public class CheckArmorNode : BehaviourNode
    {
        [SerializeField] [Range(0f,1f)] private float _requiredArmorToFlee;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if (blackboard.IsCurrentlyRepairing)
                _nodeState = NodeState.Success;
            
            EnemyCombatState combatState = controller.GetState(E_TankState.Combat) as EnemyCombatState;
            float currentArmor = combatState.GetArmorPercentage();
            if (currentArmor <= _requiredArmorToFlee)
                _nodeState = NodeState.Failure;
            

            return _nodeState;
        }
    }
}