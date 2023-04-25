using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class InvestigateNode : SequenceNode
{
    private CheckIfPlayerInRangeNode playerRangeNode;
    private CheckIfPlayerInVision playerVisionNode;
    
    public InvestigateNode()
    {
        
    }

    public override NodeState Evaluate(AIBlackboard blackboard)
    {
        base.Evaluate(blackboard);

        return _nodeState;
    }

    public override void DrawGizmos()
    {
        Handles.color = new Color(255, 0, 0, 0.05f);
        Handles.DrawSolidDisc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, _blackboard.MaxVisionInvestigationRange);
        Handles.Label(_blackboard.ThisTrans.position + _blackboard.ThisTrans.right * _blackboard.MaxVisionInvestigationRange, $"Max investigation range: {_blackboard.MaxVisionInvestigationRange}");
    }
}