using System;
using System.Linq;

namespace Behaviour_tree.Template_nodes
{
    public class SelectorNode : BehaviourNode
    {
        public SelectorNode(AIBlackboard blackboard) : base(blackboard)
        {
        
        }

        public override NodeState Evaluate()
        {
            foreach (var state in _childNodes.Select(child => child.Evaluate()))
            {
                switch (state)
                {
                    case NodeState.Running:
                        //Debug.Log($"SELECTOR:<color=orange> Branch:</color> ({child.ShowAscendingLeafChain()})");
                        continue;
                    case NodeState.Success:
                        _nodeState = NodeState.Success;
                        // if(child.GetChildCount() == 0)
                        //     Debug.Log($"SELECTOR: <color=green> Branch: </color>  ({child.ShowAscendingLeafChain()})");
                        return _nodeState;
                    case NodeState.Failure:
                        _nodeState = NodeState.Failure;
                        // if(child.GetChildCount() == 0)
                        //     Debug.Log($"SELECTOR: <color=red> Branch: </color>    ({child.ShowAscendingLeafChain()})");
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