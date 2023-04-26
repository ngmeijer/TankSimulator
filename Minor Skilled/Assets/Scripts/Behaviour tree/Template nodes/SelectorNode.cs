using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : BehaviourNode
{
    public SelectorNode(AIBlackboard blackboard) : base(blackboard)
    {
        
    }

    public override NodeState Evaluate()
    {
        foreach (var child in _childNodes)
        {
            NodeState state = child.Evaluate();

            switch (state)
            {
                case NodeState.Running:
                    continue;
                case NodeState.Success:
                    _nodeState = NodeState.Success;
                    return _nodeState;
                case NodeState.Failure:
                    _nodeState = NodeState.Failure;
                    return _nodeState;
            }
        }

        _nodeState = NodeState.Failure;
        return _nodeState;
    }

    public override void DrawGizmos()
    {
        
    }
}