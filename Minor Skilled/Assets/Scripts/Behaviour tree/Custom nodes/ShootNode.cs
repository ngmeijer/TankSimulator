using System.Collections.Generic;
using UnityEngine;

public class ShootNode : SequenceNode
{
    public ShootNode(AIBlackboard blackboard) : base(blackboard)
    {
        //Does the agent have a Line of Sight to the player?
        KeyValuePair<string, float> rangeData = new("Max shooting range", blackboard.MaxShootingRange);
        CheckIfCanSeePlayer canSeePlayerNode = new(rangeData, blackboard);
        AddChildNode(canSeePlayerNode);

        //can the agent fire (has shells & !reloading?)
        
        //Fire shell
    }
    
    public override NodeState Evaluate()
    {
        base.Evaluate();
        
        return _nodeState;
    }
}