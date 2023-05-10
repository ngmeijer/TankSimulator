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

    protected NodeState _nodeState = NodeState.Failure;
    public NodeState CurrentNodeState
    {
        get { return _nodeState; }
    }

    public BehaviourNode ParentNode;
    protected List<BehaviourNode> _childNodes = new List<BehaviourNode>();
    public Color TransparentGizmoColor = new Color(255, 255, 255, 0.01f);
    public Color SolidGizmoColor = new Color(255, 255, 255);
    protected const float DEFAULT_ALPHA = 0.05f;

    public BehaviourNode(AIBlackboard blackboard)
    {
        _blackboard = blackboard;
    }
    
    public void AddChildNode(BehaviourNode node)
    {
        _childNodes.Add(node);
        node.ParentNode = this;
    }

    public int GetChildCount() => _childNodes.Count;

    public abstract NodeState Evaluate();

    public abstract void DrawGizmos();

    public virtual string ShowAscendingLeafChain(string currentChain = "")
    {
        currentChain += ToString();

        if (ParentNode == null)
            return currentChain;

        currentChain += " > ";
        currentChain = ParentNode.ShowAscendingLeafChain(currentChain);

        return currentChain;
    }
}