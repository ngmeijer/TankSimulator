using UnityEngine;

public class WaitNode : BehaviourNode
{
    private float _waitTime;
    private float _currentTime;
    public bool ShouldWait;
    
    public WaitNode(AIBlackboard blackboard, float waitTime) : base(blackboard)
    {
        _waitTime = waitTime;
    }

    public override NodeState Evaluate()
    {
        if (!ShouldWait)
        {
            _currentTime = 0;
            return NodeState.Success;
        }
            
        _currentTime += Time.deltaTime;
        if (_currentTime >= _waitTime)
        {
            _currentTime = 0;
            return NodeState.Success;
        }

        return NodeState.Failure;
    }

    public override void DrawGizmos()
    {
        throw new System.NotImplementedException();
    }
}