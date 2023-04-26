using System.Collections.Generic;

public class SequenceNode : BehaviourNode
{
    public SequenceNode(AIBlackboard blackboard) : base(blackboard)
    {
        
    }
    
    public override NodeState Evaluate()
    {
        bool hasRunningChild = false;
        
        foreach (var child in _childNodes)
        {
            NodeState state = child.Evaluate();

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