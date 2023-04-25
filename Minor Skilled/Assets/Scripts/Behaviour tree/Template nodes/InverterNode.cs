public class InverterNode : BehaviourNode
{
    private BehaviourNode _node;
    
    public InverterNode(BehaviourNode node)
    {
        _node = node;
    }
    
    public override NodeState Evaluate(AIBlackboard blackboard)
    {
        if (_blackboard == null)
            _blackboard = blackboard;
        
        _nodeState = _node.Evaluate(blackboard) switch
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