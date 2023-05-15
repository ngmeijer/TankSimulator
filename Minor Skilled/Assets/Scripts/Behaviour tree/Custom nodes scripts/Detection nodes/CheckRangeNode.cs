using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckRangeNode")]
public class CheckRangeNode : BehaviourNode
{
    [field: SerializeField] public string RangeName { set; get;}
    [field: SerializeField] public float RangeValue { set; get;}

    private Vector3 _targetPosition;
    private Transform _targetTransform;

    public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        _nodeState = TargetInRange(controller) ? NodeState.Success : NodeState.Failure;
        return _nodeState;
    }
    
    private bool TargetInRange(AIController controller)
    {
        Vector3 target = _targetTransform ? _targetTransform.position : _targetPosition;
        float distance = Vector3.Distance(target, controller.transform.position);

        return distance <= RangeValue;
    }

    public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
    {
        Handles.color = TransparentGizmoColor;
        Handles.DrawWireDisc(controller.transform.position, controller.transform.up, RangeValue);
        Handles.Label(controller.transform.position + Vector3.right * RangeValue, $"{RangeName}: {RangeValue}");
    }
}