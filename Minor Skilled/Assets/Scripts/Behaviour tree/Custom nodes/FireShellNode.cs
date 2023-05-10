using UnityEngine;

public class FireShellNode : CombatNode
{
    public FireShellNode(AIBlackboard blackboard) : base(blackboard)
    {
    }

    public override NodeState Evaluate()
    {
        _combatState.FireShell();

        _nodeState = NodeState.Success;
        return _nodeState;
    }

    public override void DrawGizmos()
    {
        
    }
}