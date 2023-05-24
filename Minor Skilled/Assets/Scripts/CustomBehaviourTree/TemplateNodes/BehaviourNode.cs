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
public abstract class BehaviourNode : ScriptableObject
{
    public int NodeLevel;
    protected NodeState _nodeState = NodeState.Failure;
    public BehaviourNode ParentNode;
    [SerializeField] protected List<BehaviourNode> _childNodes = new();
    [SerializeField] protected bool _showLogs;

    public int GetChildCount() => _childNodes.Count;

    public virtual NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        return _nodeState;
    }

    public virtual void DrawGizmos(AIBlackboard blackboard, AIController controller)
    {
        foreach (var child in _childNodes)
        {
            if (child == null)
            {
                Debug.Log($"Null reference exception. Check node '{this}'s for child nodes that are null.");
                continue;
            }
            child.DrawGizmos(blackboard, controller);
        }
    }

    public virtual string ShowAscendingLeafChain(string currentChain = "")
    {
        currentChain += this.name;

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

    protected void OnValidate()
    {
        foreach (var child in _childNodes)
        {
            if (child == null)
                continue;
            child.SetLogEnabled(_showLogs);
        }
    }

    public void SetLogEnabled(bool enabled)
    {
        _showLogs = enabled;
    }

    public virtual void ResetValues()
    {
        foreach (var child in _childNodes)
        {
            if (child == null)
            {
                continue;
            }
            child.ResetValues();
        }
    }
}