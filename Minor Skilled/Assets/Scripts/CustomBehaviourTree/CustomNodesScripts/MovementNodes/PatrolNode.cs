using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.MovementNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Movement/PatrolNode")]
    public class PatrolNode : SequenceNode
    {
        // public PatrolNode(AIBlackboard blackboard) : base(blackboard)
        // {
        //     GetRandomNavMeshPointNode getRandomPointNode = new GetRandomNavMeshPointNode(_blackboard);
        //     AddChildNode(getRandomPointNode);
        //
        //     MoveToPositionNode moveToPositionNode = new MoveToPositionNode(_blackboard);
        //     AddChildNode(moveToPositionNode);
        // }
    }
}