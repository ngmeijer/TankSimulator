using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NodeState{
    Running,
    Success,
    Failure
}

[Serializable]
public abstract class BehaviourNode
{
    protected AIBlackboard _blackboard;

    protected NodeState _nodeState;
    public NodeState CurrentNodeState
    {
        get { return _nodeState; }
    }

    protected List<BehaviourNode> _childNodes = new List<BehaviourNode>();
    public BehaviourNode ParentNode;

    public BehaviourNode()
    {
        
    }
    
    public void AddChildNode(BehaviourNode node)
    {
        _childNodes.Add(node);
    }

    public abstract NodeState Evaluate(AIBlackboard blackboard);

    public abstract void DrawGizmos();
}