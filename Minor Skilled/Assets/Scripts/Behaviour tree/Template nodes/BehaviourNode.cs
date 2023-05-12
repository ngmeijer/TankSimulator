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
public class BehaviourNode : ScriptableObject
{
    [HideInInspector] public AIBlackboard _blackboard;
    protected NodeState _nodeState = NodeState.Failure;

    public BehaviourNode ParentNode;
    [SerializeField] protected List<BehaviourNode> _childNodes = new List<BehaviourNode>();
    public Color TransparentGizmoColor = new Color(255, 255, 255, 0.01f);
    public Color SolidGizmoColor = new Color(255, 255, 255);
    protected const float DEFAULT_ALPHA = 0.05f;

    public int GetChildCount() => _childNodes.Count;

    public virtual NodeState Evaluate()
    {
        return _nodeState;
    }

    public void AssignBlackboard(AIBlackboard blackboard)
    {
        _blackboard = blackboard;

        foreach (var child in _childNodes)
        {
            child.AssignBlackboard(blackboard);
        }
    }

    public virtual void DrawGizmos()
    {
        if (_blackboard == null)
            return;
        
        foreach (var child in _childNodes)
        {
            child.DrawGizmos();
        }
    }

    public virtual string ShowAscendingLeafChain(string currentChain = "")
    {
        currentChain += ToString();

        if (ParentNode == null)
            return currentChain;

        currentChain += " > ";
        currentChain = ParentNode.ShowAscendingLeafChain(currentChain);

        return currentChain;
    }

    public void AssignParentToChildren()
    {
        foreach (var child in _childNodes)
        {
            child.ParentNode = this;
        }
    }
}