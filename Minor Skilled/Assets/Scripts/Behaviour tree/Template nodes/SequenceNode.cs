using System.Collections.Generic;
using UnityEngine;

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
                    // Debug.Log($"SEQUENCE:<color=orange> Branch:</color> ({child.ShowAscendingLeafChain()})");
                    continue;
                case NodeState.Success:
                    // if(child.GetChildCount() == 0)
                        // Debug.Log($"SEQUENCE:<color=green> Branch:</color> ({child.ShowAscendingLeafChain()})");
                    continue;
                case NodeState.Failure:
                    // Debug.Log($"SEQUENCE:<color=red> Branch:</color>    ({child.ShowAscendingLeafChain()})");
                    _nodeState = NodeState.Failure;
                    return _nodeState;
            }
        }
        
        _nodeState = hasRunningChild ? NodeState.Running : NodeState.Success;
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