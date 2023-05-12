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

    public override NodeState Evaluate()
    {
        _nodeState = TargetInRange() ? NodeState.Success : NodeState.Failure;
        return _nodeState;
    }
    
    private bool TargetInRange()
    {
        Vector3 target = _targetTransform ? _targetTransform.position : _targetPosition;
        float distance = Vector3.Distance(target, _blackboard.ThisTrans.position);

        return distance <= RangeValue;
    }

    public override void DrawGizmos()
    {
        if (_nodeState == NodeState.Success)
        {
            SolidGizmoColor = Color.red;
            TransparentGizmoColor = new Color(255, 0, 0, DEFAULT_ALPHA);
        }
        else
        {
            SolidGizmoColor = Color.green;
            TransparentGizmoColor = new Color(0, 255, 0, DEFAULT_ALPHA);    
        }

        Handles.color = TransparentGizmoColor;
        Handles.DrawWireDisc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, RangeValue);
        Handles.Label(_blackboard.ThisTrans.position + Vector3.right * RangeValue, $"{RangeName}: {RangeValue}");
    }
}