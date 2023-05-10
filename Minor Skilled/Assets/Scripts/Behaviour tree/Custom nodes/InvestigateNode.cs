using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class InvestigateNode : SequenceNode
{
    public InvestigateNode(AIBlackboard blackboard) : base(blackboard)
    {
        CheckSuspiciousActivityNode suspActivityNode = new(blackboard);
        AddChildNode(suspActivityNode);
        
        RotateTurretToTargetNode lookAtPositionNode = new(blackboard, blackboard.InvestigatePosition);
        AddChildNode(lookAtPositionNode);
    }

    public override NodeState Evaluate()
    {
        Console.Clear();
        base.Evaluate();

        return _nodeState;
    }

    public override void DrawGizmos()
    {
        Handles.color = new Color(0, 0, 255, 0.05f);
        Handles.DrawWireDisc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, _blackboard.MaxVisionInvestigationRange);
        Handles.Label(_blackboard.ThisTrans.position + _blackboard.ThisTrans.right * _blackboard.MaxVisionInvestigationRange, $"Max investigation range: {_blackboard.MaxVisionInvestigationRange}");
    }
}

public class CheckSuspiciousActivityNode : SequenceNode
{
    public CheckSuspiciousActivityNode(AIBlackboard blackboard) : base(blackboard)
    {
        CheckIfCanSeePositionNode playerSightingNode = new(blackboard, GameManager.Instance.Player.EntityOrigin, blackboard.MaxVisionInvestigationRange);
        AddChildNode(playerSightingNode);
    }

    public override NodeState Evaluate()
    {
        base.Evaluate();

        if (_nodeState == NodeState.Success)
            _blackboard.InvestigatePosition = GameManager.Instance.Player.EntityOrigin.position;
        
        return _nodeState;
    }

    public override void DrawGizmos()
    {
        
    }
}