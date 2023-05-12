using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckFOVNode")]
public class CheckFOVNode : BehaviourNode
{
    private Vector3 _targetDirection;
    private Vector3 _targetPosition;
    private Transform _targetTransform;
    private Vector3 _currentTarget;
    [field: SerializeField] public string RangeName { set; get;}
    [field: SerializeField] public float RangeValue { set; get;}
    
    [field: SerializeField] public float ViewAngle { set; get;}
    
    public override NodeState Evaluate()
    {
        _nodeState = IsTargetInsideFOV() ? NodeState.Success : NodeState.Failure;
        
        return _nodeState;
    }
    
    //TODO: Refactor node (related to GetRandomNavMeshPointNode. Both check if a point is inside FOV)
    private bool IsTargetInsideFOV()
    {
        _currentTarget = _targetTransform != null ? _targetTransform.position : _targetPosition;
        _targetDirection = _currentTarget - _blackboard.ThisTrans.position;
        float totalAngle = Vector3.Angle(_blackboard.ThisTrans.forward, _targetDirection.normalized);
        
        return totalAngle <= ViewAngle / 2;
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

        if (_blackboard.ThisTrans == null)
            return;
        if (_blackboard.TurretTrans == null)
            return;
        
        Handles.color = TransparentGizmoColor;
        Handles.DrawSolidArc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, _blackboard.TurretTrans.forward, -ViewAngle / 2, RangeValue);
        Handles.DrawSolidArc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, _blackboard.TurretTrans.forward, ViewAngle / 2, RangeValue);
        Handles.Label(_blackboard.ThisTrans.position + Vector3.right * RangeValue, $"{RangeName}: {RangeValue}");
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_blackboard.TurretTrans.position, _blackboard.TurretTrans.forward * _blackboard.MaxInstantVisionRange);
    }
}