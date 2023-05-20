using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckSuspiciousActivityNode")]
    public class CheckSuspiciousActivityNode : SelectorNode
    {
        // //Any or several of these nodes can return SUCCESS, so should be a selector node rather than sequence.
        // public CheckSuspiciousActivityNode(AIBlackboard blackboard) : base(blackboard)
        // {
        //     CheckIfCanSeePositionNode playerSightingNode = new(blackboard, 
        //         GameManager.Instance.Player.transform, 
        //         new KeyValuePair<string, float>("Max investigation range", blackboard.MaxVisionInvestigationRange));
        //     AddChildNode(playerSightingNode);
        //     
        //     //Add sound check node (shell fired in hearing range)?
        // }
    }
}