using System.Collections.Generic;

public class SelectorNode : BehaviourNode
{
    public SelectorNode()
    {
        
    }

    public override NodeState Evaluate(AIBlackboard blackboard)
    {
        if (_blackboard == null)
            _blackboard = blackboard;
        
        foreach (var child in _childNodes)
        {
            NodeState state = child.Evaluate(blackboard);

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