﻿using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/InvestigateNode")]
    public class InvestigateNode : SequenceNode
    {
        // public InvestigateNode(AIBlackboard blackboard) : base(blackboard)
        // {
        //     CheckSuspiciousActivityNode suspiciousActivityNode = new(blackboard);
        //     AddChildNode(suspiciousActivityNode);
        //     
        //     RotateTurretToTargetNode lookAtPositionNode = new(blackboard);
        //     AddChildNode(lookAtPositionNode);
        //
        //     MoveToPositionNode moveToPositionNode = new(blackboard);
        //     AddChildNode(moveToPositionNode);
        // }
    }
}