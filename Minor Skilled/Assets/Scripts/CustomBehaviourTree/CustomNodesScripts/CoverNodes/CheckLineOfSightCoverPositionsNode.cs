using System.Collections.Generic;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.CoverNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Cover/FilterLineOfSightCovers")]
    public class CheckLineOfSightCoverPositionsNode : BehaviourNode
    {
        private List<Vector3> _filteredOutPoints;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            Color hitColor = Color.green;

            float currentShortestDistance = float.MaxValue;
            Vector3 chosenCoverPoint = Vector3.zero;
            foreach (var coverPoint in blackboard.GeneratedCoverPoints)
            {
                List<Vector3> RelativeRaycastPositions = new()
                {
                    coverPoint + controller.CoverRaycastPositions[0].localPosition,
                    coverPoint + controller.CoverRaycastPositions[1].localPosition,
                    coverPoint + controller.CoverRaycastPositions[2].localPosition,
                    coverPoint + controller.CoverRaycastPositions[3].localPosition
                };

                foreach (Vector3 pos in RelativeRaycastPositions)
                {
                    bool hasHitCollider = Physics.Linecast(pos,
                        GameManager.Instance.Player.EntityOrigin.position, out RaycastHit hit);
                    if (hasHitCollider)
                    {
                        //If player has been hit with linecast, point is not a valid cover.
                        if (hit.collider.transform.root.CompareTag("Player") ||
                            hit.collider.transform.root.CompareTag("Enemy"))
                        {
                            hitColor = Color.red;
                            _filteredOutPoints.Add(coverPoint);
                        }
                        else
                        {
                            float distanceToAgent = Vector3.Distance(coverPoint, controller.transform.position);
                            if (distanceToAgent < currentShortestDistance)
                            {
                                currentShortestDistance = distanceToAgent;
                                chosenCoverPoint = coverPoint;
                            }
                        }
                    }

                    Debug.DrawLine(pos, GameManager.Instance.Player.EntityOrigin.position, hitColor);
                }
            }

            foreach (var point in _filteredOutPoints)
            {
                blackboard.GeneratedCoverPoints.Remove(point);
            }

            if (chosenCoverPoint != Vector3.zero)
            {
                _nodeState = NodeState.Success;
                blackboard.CurrentCoverPoint = chosenCoverPoint;
            }

            return _nodeState;
        }

        public override void ResetValues()
        {
            _filteredOutPoints = new();
        }
    }
}