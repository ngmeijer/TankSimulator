using System;
using UnityEngine;

public class BehaviourTree
{
    private BehaviourNode _rootNode;
    
    public BehaviourTree()
    {
        _rootNode = null;
    }

    public void SetRootNode(BehaviourNode rootNode)
    {
        _rootNode = rootNode;
    }

    public void EvaluateTree()
    {
        _rootNode?.Evaluate();
    }

    public void DrawGizmos()
    {
        _rootNode?.DrawGizmos();
    }
}