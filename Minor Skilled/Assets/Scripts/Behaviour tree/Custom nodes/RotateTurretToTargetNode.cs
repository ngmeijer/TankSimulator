using UnityEditor;
using UnityEngine;

public class RotateTurretToTargetNode : BehaviourNode
{
    private EnemyTankCombatState _combatState;
    private Vector3 _targetPosition;
    private Transform _targetTransform;
    private Vector3 _currentTarget;
    private float _dotResult;
    
    public RotateTurretToTargetNode(AIBlackboard blackboard, Vector3 targetPosition) : base(blackboard)
    {
        _targetPosition = targetPosition;
        _combatState = _blackboard.StateSwitcher.CombatState as EnemyTankCombatState;
    }
    
    public RotateTurretToTargetNode(AIBlackboard blackboard, Transform targetTransform) : base(blackboard)
    {
        _targetTransform = targetTransform;
        _combatState = _blackboard.StateSwitcher.CombatState as EnemyTankCombatState;
    }

    public override NodeState Evaluate()
    {
        if (_targetTransform != null)
            _currentTarget = _targetTransform.position;
        else _currentTarget = _targetPosition;

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
            _nodeState = NodeState.Failure;
        }

        return _nodeState;
    }

    private bool IsAimingAtTarget()
    {
        Vector3 direction =  _currentTarget - _blackboard.ThisTrans.position;
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