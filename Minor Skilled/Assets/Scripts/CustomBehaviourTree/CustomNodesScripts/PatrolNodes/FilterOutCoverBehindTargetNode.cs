using System.Collections.Generic;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.PatrolNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Cover/FilterOutCoverBehindTarget")]
    public class FilterOutCoverBehindTargetNode : BehaviourNode
    {
        private List<Vector3> _filteredOutPoints;
        
        //Will probably be reiterating on this
        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            foreach (var coverPoint in blackboard.GeneratedCoverPoints)
            {
                //Calculate dot product from cover point to player position
                float dot = Vector3.Dot(coverPoint, GameManager.Instance.Player.EntityOrigin.position);
                
                if (dot < 0)
                    _filteredOutPoints.Add(coverPoint);
            }

            foreach (var point in _filteredOutPoints)
            {
                blackboard.GeneratedCoverPoints.Remove(point);
            }

            if (blackboard.GeneratedCoverPoints.Count != 0)
                _nodeState = NodeState.Success;
            
            return _nodeState;
        }

        public override void ResetValues()
        {
            _filteredOutPoints = new List<Vector3>();
        }
    }
}