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

    public void EvaluateTree(AIBlackboard _blackboard)
    {
        _rootNode?.Evaluate(_blackboard);
    }

    private void OnDrawGizmos()
    {
        _rootNode?.DrawGizmos();
    }
}