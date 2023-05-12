using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Shooting/RotateTurretToTargetNode")]
public class RotateTurretToTargetNode : CombatNode
{
    private float _dotResult;

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
    
    private void MoveTurretToTarget(float direction)
    {
        _combatState.RotateTurret(direction);
    }
}