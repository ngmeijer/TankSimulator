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
    public Color GizmoColor = new Color(255, 255, 255, 0.01f);
    protected const float DEFAULT_ALPHA = 0.05f;

    public BehaviourNode(AIBlackboard blackboard)
    {
        _blackboard = blackboard;
    }
    
    public void AddChildNode(BehaviourNode node)
    {
        _childNodes.Add(node);
    }

    public abstract NodeState Evaluate();

    public abstract void DrawGizmos();
}