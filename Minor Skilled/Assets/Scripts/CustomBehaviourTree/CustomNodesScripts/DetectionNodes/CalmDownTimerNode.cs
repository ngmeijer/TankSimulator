using System;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/CalmDownTimer")]
    public class CalmDownTimerNode : BehaviourNode
    {
        [SerializeField] private float _calmDownTime;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            DamageRegistrationComponent component = controller.ComponentManager.DamageComponent;
            // if (component.CalmDownTimer > 0 && blackboard.should)
            //     component.CalmDownTimer -= Time.deltaTime;

            return _nodeState;
        }
    }
}