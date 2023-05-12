using UnityEngine;

public class WaitForSecondsNode : BehaviourNode
{
    private float _waitTime;
    private float _currentTime;
    
    // public WaitForSecondsNode(AIBlackboard blackboard, float waitTime) : base(blackboard)
    // {
    //     _waitTime = waitTime;
    // }

    public override NodeState Evaluate()
    {
        if (_currentTime < _waitTime)
        {
            _currentTime += Time.deltaTime;
            _nodeState = NodeState.Running;
        }
        else _nodeState = NodeState.Success;

        return _nodeState;
    }

    public override void DrawGizmos()
    {
        throw new System.NotImplementedException();
    }
}