using System;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
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

    private void OnDrawGizmos()
    {
        _rootNode?.DrawGizmos();
    }
}