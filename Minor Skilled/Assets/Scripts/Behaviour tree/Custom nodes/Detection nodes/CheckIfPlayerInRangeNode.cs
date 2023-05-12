using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckIfPlayerInRangeNode")]
public class CheckIfPlayerInRangeNode : BehaviourNode
{
    public CustomKeyValue MaxRange;
    
    public override NodeState Evaluate()
    {
        _nodeState = PlayerInRange() ? NodeState.Success : NodeState.Failure;

        return _nodeState;
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();

        if (_blackboard.TurretTrans == null)
            return;
        if (_blackboard.ThisTrans == null)
            return;
        
        Handles.color = new Color(0,0,0, DEFAULT_ALPHA);
        float invertedAngle = (360 - _blackboard.ViewAngle) / 2;
        Handles.color = TransparentGizmoColor;
        Handles.DrawSolidArc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, -_blackboard.TurretTrans.forward, -invertedAngle, MaxRange.RangeValue);
        Handles.DrawSolidArc(_blackboard.ThisTrans.position, _blackboard.ThisTrans.up, -_blackboard.TurretTrans.forward, invertedAngle, MaxRange.RangeValue);
        Handles.Label(_blackboard.ThisTrans.position + _blackboard.ThisTrans.right * 
            MaxRange.RangeValue, $"{MaxRange.RangeName}: {MaxRange.RangeValue}");
    }

    private bool PlayerInRange()
    {
        float distance =
            Vector3.Distance(GameManager.Instance.Player.EntityOrigin.position, _blackboard.ThisTrans.position);

        return distance <= MaxRange.RangeValue;
    }
}

[Serializable]
public class CustomKeyValue
{
    [field: SerializeField] public string RangeName { set; get;}
    [field: SerializeField] public float RangeValue { set; get;}
}