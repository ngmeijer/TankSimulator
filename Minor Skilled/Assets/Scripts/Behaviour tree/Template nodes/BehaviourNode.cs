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
    protected NodeState _nodeState = NodeState.Failure;

    public BehaviourNode ParentNode;
    [SerializeField] protected List<BehaviourNode> _childNodes = new();
    public Color TransparentGizmoColor;
    public Color SolidGizmoColor;
    protected const float DEFAULT_ALPHA = 0.05f;

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