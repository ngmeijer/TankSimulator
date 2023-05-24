using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Tree instance")]
public class BehaviourTree : ScriptableObject
{
    [SerializeField] private BehaviourNode _rootNode;

    public void AssignParentNodesToTreeNodes()
    {
        _rootNode?.AssignParentToChildren();
    }

    public void EvaluateTree(AIBlackboard blackboard, AIController controller)
    {
        _rootNode.NodeLevel = 0;
        _rootNode?.Evaluate(blackboard, controller);
    }

    public void DrawGizmos(AIBlackboard blackboard, AIController controller)
    {
        if (blackboard == null)
            return;
        if (controller == null)
            return;
        
        _rootNode?.DrawGizmos(blackboard, controller);
    }

    public void ResetNodes()
    {
        _rootNode?.ResetValues();
    }
}