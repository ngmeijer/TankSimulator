using System;
using System.Linq;
using UnityEngine;

    [CreateAssetMenu(menuName = "Behaviour tree/Composite/SelectorNode")]
    public class SelectorNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            //Could use LINQ here, but wouldn't be able to do child.Function().
            foreach (var child in _childNodes)
            {
                switch (child.Evaluate(blackboard, controller))
                {
                    case NodeState.Running:
                        Debug.Log($"SELECTOR:<color=orange> Branch:</color> ({child.ShowAscendingLeafChain()})");
                        continue;
                    case NodeState.Success:
                        _nodeState = NodeState.Success;
                        Debug.Log($"SELECTOR: <color=green> Branch: </color>  ({child.ShowAscendingLeafChain()})");
                        return _nodeState;
                    case NodeState.Failure:
                        _nodeState = NodeState.Failure;
                        Debug.Log($"SELECTOR: <color=red> Branch: </color>    ({child.ShowAscendingLeafChain()})");
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _nodeState = NodeState.Failure;
            return _nodeState;
        }

        public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
        {
            foreach (var child in _childNodes)
            {
                child.DrawGizmos(blackboard, controller);
            }
        }
    }