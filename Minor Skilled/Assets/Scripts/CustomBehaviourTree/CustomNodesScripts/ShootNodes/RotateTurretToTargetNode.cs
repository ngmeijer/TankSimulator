using UnityEditor;
using UnityEngine;

namespace CustomBehaviourTree.CustomNodesScripts.ShootNodes
{
    [CreateAssetMenu(menuName = "Behaviour tree/Shooting/RotateTurretToTargetNode")]
    public class RotateTurretToTargetNode : BehaviourNode
    {
        private float _dotResult;

        public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
        {
            if (IsAimingAtTarget(blackboard, controller))
                _nodeState = NodeState.Success;
            else
            {
                int direction = 0;
                if (_dotResult > 0)
                    direction = 1;
                else if (_dotResult < 0)
                    direction = -1;
            
                MoveTurretToTarget(controller, direction);
                _nodeState = NodeState.Running;
            }

            return _nodeState;
        }

        private bool IsAimingAtTarget(AIBlackboard blackboard, AIController controller)
        {
            Vector3 direction =  blackboard.PointToRotateTurretTo - controller.transform.position;
            direction.Normalize();
            Transform turretTransform = controller.ComponentManager.TurretControlComponent.TurretTransform;
            _dotResult = Vector3.Dot(direction, turretTransform.right);

            return _dotResult is > -0.01f and < 0.01f;
        }
    
        private void MoveTurretToTarget(AIController controller, float direction)
        {
            EnemyTankCombatState combatState = controller.GetState(E_TankState.Combat) as EnemyTankCombatState;
            combatState.RotateTurret(direction);
        }
    }
}