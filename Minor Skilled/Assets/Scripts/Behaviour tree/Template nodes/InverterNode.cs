using System.Drawing;
using Color = UnityEngine.Color;

public class InverterNode : BehaviourNode
{
    private BehaviourNode _node;
    
    public InverterNode(AIBlackboard blackboard, BehaviourNode node) : base(blackboard)
    {
        _node = node;
    }
    
    public override NodeState Evaluate()
    {
        _nodeState = _node.Evaluate() switch
        {
            NodeState.Running => NodeState.Running,
            NodeState.Success => NodeState.Failure,
            NodeState.Failure => NodeState.Success,
            _ => _nodeState
        };

        return _nodeState;
    }

    public override void DrawGizmos()
    {
        
    }
}