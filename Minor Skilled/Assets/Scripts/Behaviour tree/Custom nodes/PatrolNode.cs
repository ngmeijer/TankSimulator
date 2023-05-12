using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNode : SequenceNode
{
    public PatrolNode(AIBlackboard blackboard) : base(blackboard)
    {
        GetRandomNavMeshPointNode getRandomPointNode = new GetRandomNavMeshPointNode(_blackboard);
        AddChildNode(getRandomPointNode);

        MoveToPositionNode moveToPositionNode = new MoveToPositionNode(_blackboard);
        AddChildNode(moveToPositionNode);
    }

    public override void DrawGizmos()
    {
        Handles.color = new Color(0, 255, 0, 0.05f);
        Handles.DrawSolidDisc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, _blackboard.MaxPatrolRange);
        Handles.Label(_blackboard.ThisTrans.position + _blackboard.ThisTrans.right * _blackboard.MaxPatrolRange, $"Max patrol range: {_blackboard.MaxPatrolRange}");

        Handles.color = new Color(255, 0, 0, 0.05f);
        Handles.DrawSolidDisc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, _blackboard.MaxInstantVisionRange);
        Handles.Label(_blackboard.ThisTrans.position + _blackboard.ThisTrans.right * _blackboard.MaxInstantVisionRange, $"Max instant vision range: {_blackboard.MaxInstantVisionRange}");
    }
}