using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootNode : SequenceNode
{
    public ShootNode(AIBlackboard blackboard) : base(blackboard)
    {
        CheckIfCanSeePositionNode canSeePositionNode = new(blackboard, GameManager.Instance.Player.transform, blackboard.MaxShootingRange);
        AddChildNode(canSeePositionNode);

        RotateTurretToTargetNode rotateTurretToTargetNode = new(blackboard);
        AddChildNode(rotateTurretToTargetNode);
        
        CheckShellCountNode shellCountNode = new(blackboard);
        AddChildNode(shellCountNode);

        CheckReloadingNode reloadCheckNode = new(blackboard);
        AddChildNode(reloadCheckNode);

        FireShellNode shellFireNode = new(blackboard);
        AddChildNode(shellFireNode);
    }
    
    public override NodeState Evaluate()
    {
        base.Evaluate();
        
        return _nodeState;
    }
}