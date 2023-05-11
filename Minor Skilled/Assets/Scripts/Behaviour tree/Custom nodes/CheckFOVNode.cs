using UnityEditor;
using UnityEngine;

public class CheckFOVNode : BehaviourNode
{
    private Vector3 _targetDirection;
    private Vector3 _targetPosition;
    private Transform _targetTransform;
    private Vector3 _currentTarget;
    private float _maxRange;

    public CheckFOVNode(AIBlackboard blackboard, Vector3 targetPosition, float maxRange) : base(blackboard)
    {
        _targetPosition = targetPosition;
        _maxRange = maxRange;
    }
    
    public CheckFOVNode(AIBlackboard blackboard, Transform targetTransform, float maxRange) : base(blackboard)
    {
        _targetTransform = targetTransform;
        _maxRange = maxRange;
    }

    public override NodeState Evaluate()
    {
        _nodeState = IsTargetInsideFOV() ? NodeState.Success : NodeState.Failure;
        
        return _nodeState;
    }
    
    //Refactor node (related to GetRandomNavMeshPointNode. Both check if a point is inside FOV)
    private bool IsTargetInsideFOV()
    {
        _currentTarget = _targetTransform != null ? _targetTransform.position : _targetPosition;
        _targetDirection = _currentTarget - _blackboard.TurretTrans.position;
        float totalAngle = Vector3.Angle(_blackboard.TurretTrans.forward, _targetDirection.normalized);
        
        return totalAngle <= _blackboard.ViewAngle / 2;
    }

    public override void DrawGizmos()
    {
        SolidGizmoColor = Color.red;
        TransparentGizmoColor = new Color(255, 0, 0, DEFAULT_ALPHA);
        
        if (_nodeState == NodeState.Success)
        {
            SolidGizmoColor = Color.green;
            TransparentGizmoColor = new Color(0, 255, 0, DEFAULT_ALPHA);
        }
        
        Handles.color = TransparentGizmoColor;
        Handles.DrawSolidArc(_blackboard.TurretTrans.position, _blackboard.TurretTrans.up, _blackboard.TurretTrans.forward, -_blackboard.ViewAngle / 2, _maxRange);
        Handles.DrawSolidArc(_blackboard.TurretTrans.position, _blackboard.TurretTrans.up, _blackboard.TurretTrans.forward, _blackboard.ViewAngle / 2, _maxRange);
        Handles.Label(_blackboard.TurretTrans.position + Vector3.right * _maxRange, $"Max range: {_maxRange}");
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_blackboard.TurretTrans.position, _blackboard.TurretTrans.forward * _blackboard.MaxInstantVisionRange);
    }
}