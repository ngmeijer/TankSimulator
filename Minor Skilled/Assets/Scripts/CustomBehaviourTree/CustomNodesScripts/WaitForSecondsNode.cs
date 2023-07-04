using System;
using UnityEditor;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts
{
    [CreateAssetMenu(menuName = "Behaviour tree/Utility/WaitForSecondsNode")]
    public class WaitForSecondsNode : BehaviourNode
    {
        [SerializeField] private string _waitPurpose;
        [SerializeField] private float _waitTime;
        private float _currentTime;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            //Continue branch if there is no countdown
            if (!blackboard.ShouldCountDown)
            {
                _nodeState = NodeState.Success;
                return _nodeState;
            }

            if (_currentTime > 0)
            {
                _currentTime -= Time.deltaTime;
                _nodeState = NodeState.Failure;
                return _nodeState;
            }

            _currentTime = _waitTime;
            _nodeState = NodeState.Success;
            blackboard.ShouldCountDown = false;
            return _nodeState;
        }

        public override void ResetValues()
        {
            _currentTime = _waitTime;
        }

#if UNITY_EDITOR
        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            if (_nodeState == NodeState.Failure)
            {
                float currentTime = (float)Math.Round(_currentTime,2);
                Handles.Label(controller.transform.position, $"{_waitPurpose}: time left to wait: {currentTime} seconds");
            }
        }
        #endif
    }
}