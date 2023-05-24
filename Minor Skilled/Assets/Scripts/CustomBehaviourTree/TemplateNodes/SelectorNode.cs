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
                if (child == null)
                {
                    Debug.Log($"Null reference exception. Check node '{this}'s for child nodes that are null.");
                    continue;
                }
                
                child.NodeLevel = NodeLevel + 1;
                switch (child.Evaluate(blackboard, controller))
                {
                    case NodeState.Running:
                        if(_showLogs)
                            Debug.Log(LogFormat(child, "orange"));
                        continue;
                    case NodeState.Success:
                        _nodeState = NodeState.Success;
                        if(_showLogs)
                            Debug.Log(LogFormat(child, "green"));
                        return _nodeState;
                    case NodeState.Failure:
                        _nodeState = NodeState.Failure;
                        if(_showLogs)
                            Debug.Log(LogFormat(child, "red"));
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _nodeState = NodeState.Failure;
            return _nodeState;
        }

        private string LogFormat(BehaviourNode child, string colour)
        {
            return $"{Indenter.GetIdent(NodeLevel)} SELECTOR: <color={colour}> Branch: </color> [{child.NodeLevel}] ({child.name})";
        }
    }