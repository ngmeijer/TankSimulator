using System;
using System.Linq;
using UnityEngine;

namespace Behaviour_tree.Template_nodes
{
    public class SelectorNode : BehaviourNode
    {
        public SelectorNode(AIBlackboard blackboard) : base(blackboard)
        {
        
        }

        public override NodeState Evaluate()
        {
            //Could use LINQ here, but wouldn't be able to do child.Function().
            foreach (var child in _childNodes)
            {
                switch (child.Evaluate())
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

        public override void DrawGizmos()
        {
            foreach (var child in _childNodes)
            {
                child.DrawGizmos();
            }
        }
    }
}