using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.DetectionNodes
{
    public static class PointCheck
    {
        public static bool IsPointInsideFOV(Vector3 point, Transform thisTransform, float viewAngle)
        {
            Vector3 targetDirection = point - thisTransform.position;
            float totalAngle = Vector3.Angle(thisTransform.forward, targetDirection.normalized);
        
            return totalAngle <= viewAngle / 2;
        }
        
        public static bool PointInRange(Vector3 targetPosition, Vector3 thisPosition, float minRange, float maxRange)
        {
            float distance =
                Vector3.Distance(targetPosition, thisPosition);

            return distance >= minRange && distance <= maxRange;
        }
        
        public static bool HasLineOfSight(Vector3 raycastPos, Vector3 targetPosition, int layerIndex = -1)
        {
            bool hitCollider;
            if (layerIndex == -1)
            {
                hitCollider = Physics.Linecast(
                    raycastPos,
                    targetPosition, out RaycastHit hit);
            }
            else hitCollider = Physics.Linecast(
                raycastPos,
                targetPosition, out RaycastHit hit, 1 << layerIndex);

            return hitCollider;
        }
    }
}