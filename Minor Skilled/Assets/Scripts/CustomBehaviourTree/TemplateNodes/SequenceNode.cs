﻿using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Composite/SequenceNode")]
public class SequenceNode : BehaviourNode
{
    public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        bool hasRunningChild = false;
        
        foreach (var child in _childNodes)
        {
            if (child == null)
            {
                Debug.Log($"Null reference exception. Check node '{this}'s for child nodes that are null.");
                continue;
            }

            child.NodeLevel = NodeLevel + 1;
            child.LogNode();
            
            switch (child.Evaluate(blackboard, controller))
            {
                case NodeState.Running:
                    hasRunningChild = true;
                    continue;
                case NodeState.Success:
                    continue;
                case NodeState.Failure:
                    _nodeState = NodeState.Failure;
                    return _nodeState;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        _nodeState = hasRunningChild ? NodeState.Running : NodeState.Success;
        return _nodeState;
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
}