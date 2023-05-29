using System.Collections.Generic;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.CoverNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Cover/CopyCoverPoints")]
    public class CopyCoverPointsNode : BehaviourNode
    {
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            blackboard.GeneratedCoverPoints = blackboard.GenericPointsFound;

            _nodeState = NodeState.Success;
            return _nodeState;
        }
    }
}