using UnityEngine;

public class MoveToPositionNode : CombatNode
{
    private Vector3 _targetPos;
    
    public MoveToPositionNode(AIBlackboard blackboard) : base(blackboard)
    {
        
    }
}