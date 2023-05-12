using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Tree instance")]
public class BehaviourTree : ScriptableObject
{
    [SerializeField] private BehaviourNode _rootNode;

    public void AssignBlackboardToTreeNodes(AIBlackboard blackboard)
    {
        _rootNode?.AssignBlackboard(blackboard);
    }

    public void AssignParentNodesToTreeNodes()
    {
        _rootNode?.AssignParentToChildren();
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