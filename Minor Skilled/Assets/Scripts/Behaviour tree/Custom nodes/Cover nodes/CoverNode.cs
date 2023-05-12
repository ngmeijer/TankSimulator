using Behaviour_tree.Custom_nodes;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Cover/CoverNode")]
public class CoverNode : SequenceNode
{
    // public CoverNode(AIBlackboard blackboard) : base(blackboard)
    // {
    //     // CheckArmorNode checkArmorNode = new(_blackboard, 0.3f);
    //     // AddChildNode(checkArmorNode);
    //     //
    //     // //Are we already in cover? If the node fails,
    //     // //it's not necessary to find cover and the chain breaks.
    //     // CheckLineOfSightNode checkLineOfSightNode = new(_blackboard);
    //     // AddChildNode(checkLineOfSightNode);
    //     //
    //     // FindSuitableCoverNode findCoverNode = new(_blackboard);
    //     // AddChildNode(findCoverNode);
    //     // //string coverChain = findCoverNode.ShowAscendingLeafChain();
    //     // //Debug.Log($"Cover chain: {coverChain}");
    //     //
    //     // MoveToPositionNode moveToPosNode = new(_blackboard);
    //     // AddChildNode(moveToPosNode);
    // }
}