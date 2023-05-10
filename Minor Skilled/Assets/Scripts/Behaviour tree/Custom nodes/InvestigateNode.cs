using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class InvestigateNode : SequenceNode
{
    public InvestigateNode(AIBlackboard blackboard) : base(blackboard)
    {
        CheckSuspiciousActivityNode suspiciousActivityNode = new(blackboard);
        AddChildNode(suspiciousActivityNode);
        
        RotateTurretToTargetNode lookAtPositionNode = new(blackboard);
        AddChildNode(lookAtPositionNode);

        MoveToPositionNode moveToPositionNode = new(blackboard);
        AddChildNode(moveToPositionNode);
    }

    public override NodeState Evaluate()
    {
        base.Evaluate();

        return _nodeState;
    }
}