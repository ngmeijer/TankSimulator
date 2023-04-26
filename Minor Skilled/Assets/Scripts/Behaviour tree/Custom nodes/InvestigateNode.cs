using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class InvestigateNode : SequenceNode
{
    public InvestigateNode(AIBlackboard blackboard) : base(blackboard)
    {
        
    }

    public override NodeState Evaluate()
    {
        base.Evaluate();

        return _nodeState;
    }

    public override void DrawGizmos()
    {
        Handles.color = new Color(255, 0, 0, 0.05f);
        Handles.DrawSolidDisc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, _blackboard.MaxVisionInvestigationRange);
        Handles.Label(_blackboard.ThisTrans.position + _blackboard.ThisTrans.right * _blackboard.MaxVisionInvestigationRange, $"Max investigation range: {_blackboard.MaxVisionInvestigationRange}");
    }
}