using UnityEditor;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts
{
    [CreateAssetMenu(menuName = "Behaviour tree/Utility/WaitForSecondsNode")]
    public class WaitForSecondsNode : BehaviourNode
    {
        [SerializeField] private float _waitTime;
        private float _currentTime;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if (!blackboard.ShouldCountDown)
            {
                _nodeState = NodeState.Failure;
                return _nodeState;
            }

            if (_currentTime > 0)
            {
                _currentTime -= Time.deltaTime;
                _nodeState = NodeState.Success;
                return _nodeState;
            }

            _currentTime = _waitTime;
            _nodeState = NodeState.Failure;
            blackboard.ShouldCountDown = false;
            return _nodeState;
        }

        public override void ResetValues()
        {
            _currentTime = _waitTime;
        }

        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            Handles.Label(controller.transform.position, $"Time left to wait: {_currentTime}");
        }
    }
}