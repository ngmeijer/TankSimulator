using System;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

[CreateAssetMenu(menuName = "Behaviour tree/Decorator/Inverter")]
public class InverterNode : BehaviourNode
{
    private BehaviourNode _node;

    private void OnEnable()
    {
        _node = _childNodes[0];
    }

    public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        _nodeState = _node.Evaluate(blackboard, controller) switch
        {
            NodeState.Running => NodeState.Running,
            NodeState.Success => NodeState.Failure,
            NodeState.Failure => NodeState.Success,
            _ => _nodeState
        };

        return _nodeState;
    }
}