using System;
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
            
            switch (child.Evaluate(blackboard, controller))
            {
                case NodeState.Running:
                    hasRunningChild = true;
                    if(_showLogs)
                        Debug.Log(LogFormat(child, "orange"));
                    continue;
                case NodeState.Success:
                    if(_showLogs)
                        Debug.Log(LogFormat(child, "green"));
                    continue;
                case NodeState.Failure:
                    if(_showLogs)
                        Debug.Log(LogFormat(child, "red"));
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
    
    private string LogFormat(BehaviourNode child, string colour)
    {
        return $"{Indenter.GetIdent(NodeLevel)} SEQUENCE: <color={colour}> Branch: </color> [{child.NodeLevel}] ({child.name})";
    }
}