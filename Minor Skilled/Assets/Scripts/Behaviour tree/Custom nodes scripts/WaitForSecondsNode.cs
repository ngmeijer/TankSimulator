using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Utility/WaitForSecondsNode")]
public class WaitForSecondsNode : BehaviourNode
{
    [SerializeField] private float _waitTime;
    private float _currentTime;
    
    public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        if (_currentTime < _waitTime)
        {
            _currentTime += Time.deltaTime;
            _nodeState = NodeState.Running;
        }
        else
        {
            _nodeState = NodeState.Success;
            _currentTime = 0f;
        }

        return _nodeState;
    }

    public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
    {
        Handles.Label(controller.transform.position, $"Time left to wait: {_waitTime - _currentTime}");
    }
}