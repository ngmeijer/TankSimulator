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
    
    private float _viewAngle;
    [field:SerializeField] public float ViewAngle { get; private set; }
    
    public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        _nodeState = IsTargetInsideFOV(controller) ? NodeState.Success : NodeState.Failure;
        
        return _nodeState;
    }
    
    //TODO: Refactor node (related to GetRandomNavMeshPointNode. Both check if a point is inside FOV)
    private bool IsTargetInsideFOV(AIController controller)
    {
        _currentTarget = _targetTransform != null ? _targetTransform.position : _targetPosition;
        _targetDirection = _currentTarget - controller.transform.position;
        float totalAngle = Vector3.Angle(controller.transform.forward, _targetDirection.normalized);
        
        return totalAngle <= ViewAngle / 2;
    }

    public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
    {
        SolidGizmoColor = Color.red;
        
        if (_nodeState == NodeState.Success)
        {
            SolidGizmoColor = Color.green;
        }

        Transform turretTransform = controller.ComponentManager.TurretControlComponent.TurretTransform;
        Handles.color = TransparentGizmoColor;
        Handles.DrawSolidArc(controller.transform.position, controller.transform.up, turretTransform.forward, -ViewAngle / 2, RangeValue);
        Handles.DrawSolidArc(controller.transform.position, controller.transform.up, turretTransform.forward, ViewAngle / 2, RangeValue);
        Handles.Label(controller.transform.position + Vector3.right * RangeValue, $"{RangeName}: {RangeValue}");
    }
}