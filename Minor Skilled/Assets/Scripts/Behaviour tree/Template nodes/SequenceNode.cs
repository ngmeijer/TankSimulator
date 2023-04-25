using System.Collections.Generic;

public class SequenceNode : BehaviourNode
{
    public SequenceNode()
    {
        
    }
    
    public override NodeState Evaluate(AIBlackboard blackboard)
    {
        if (_blackboard == null)
            _blackboard = blackboard;
        
        bool hasRunningChild = false;
        
        foreach (var child in _childNodes)
        {
            NodeState state = child.Evaluate(blackboard);

            switch (state)
            {
                case NodeState.Running:
                    hasRunningChild = true;
                    _nodeState = NodeState.Success;
                    continue;
                case NodeState.Success:
                    continue;
                case NodeState.Failure:
                    _nodeState = NodeState.Failure;
                    return _nodeState;
            }
        }
        
        _nodeState = hasRunningChild ? NodeState.Success : NodeState.Failure;
        return _nodeState;
    }

    public override void DrawGizmos()
    {
        foreach (var child in _childNodes)
        {
            child.DrawGizmos();
        }
    }
}