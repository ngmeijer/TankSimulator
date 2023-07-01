using System;
using TankComponents;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/CalmDownTimer")]
    public class CalmDownTimerNode : BehaviourNode
    {
        [SerializeField] private float _calmDownTime;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            DamageRegistrationComponent component = controller.ComponentManager.DamageComp;
            if (component.CalmDownTimer > 0 && blackboard.ShouldCountDown)
                component.CalmDownTimer -= Time.deltaTime;

            return _nodeState;
        }
    }
}