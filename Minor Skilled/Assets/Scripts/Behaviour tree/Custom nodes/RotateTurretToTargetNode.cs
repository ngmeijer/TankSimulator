using UnityEditor;
using UnityEngine;

public class RotateTurretToTargetNode : BehaviourNode
{
    private EnemyTankCombatState _combatState;
    private float _dotResult;
    
    public RotateTurretToTargetNode(AIBlackboard blackboard) : base(blackboard)
    {
        _combatState = _blackboard.AIController.StateSwitcher.CombatState as EnemyTankCombatState;
    }

    public override NodeState Evaluate()
    {
        if (IsAimingAtTarget())
            _nodeState = NodeState.Success;
        else
        {
            int direction = 0;
            if (_dotResult > 0)
                direction = 1;
            else if (_dotResult < 0)
                direction = -1;
            
            MoveTurretToTarget(direction);
            _nodeState = NodeState.Running;
        }

        return _nodeState;
    }

    private bool IsAimingAtTarget()
    {
        Vector3 direction =  _blackboard.TurretLookAtPosition - _blackboard.ThisTrans.position;
        direction.Normalize();
        _dotResult = Vector3.Dot(direction, _blackboard.TurretTrans.right);

        return _dotResult is > -0.01f and < 0.01f;
    }

    public override void DrawGizmos()
    {
        
    }

    private void MoveTurretToTarget(float direction)
    {
        _combatState.RotateTurret(direction);
    }
}