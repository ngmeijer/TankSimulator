using UnityEditor;
using UnityEngine;

public class CheckFOVNode : BehaviourNode
{
    private Vector3 _playerDirection;

    public CheckFOVNode(AIBlackboard blackboard) : base(blackboard)
    {
    }

    public override NodeState Evaluate()
    {
        _nodeState = !IsPlayerInsideFOV() ? NodeState.Failure : NodeState.Success;
        
        return _nodeState;
    }
    
    private bool IsPlayerInsideFOV()
    {
        _playerDirection = GameManager.Instance.Player.EntityOrigin.position - _blackboard.TurretTrans.position;
        float totalAngle = Vector3.Angle(_blackboard.TurretTrans.forward, _playerDirection.normalized);
        
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
        Handles.DrawSolidArc(_blackboard.TurretTrans.position, _blackboard.TurretTrans.up, _blackboard.TurretTrans.forward, -_blackboard.ViewAngle / 2, _blackboard.MaxInstantVisionRange);
        Handles.DrawSolidArc(_blackboard.TurretTrans.position, _blackboard.TurretTrans.up, _blackboard.TurretTrans.forward, _blackboard.ViewAngle / 2, _blackboard.MaxInstantVisionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(_blackboard.TurretTrans.position, _blackboard.TurretTrans.forward * _blackboard.MaxInstantVisionRange);
        
        //Area behind where the agent cannot see
        Handles.color = new Color(0,0,0, DEFAULT_ALPHA);
        float invertedAngle = (360 - _blackboard.ViewAngle) / 2;
        Handles.DrawSolidArc(_blackboard.TurretTrans.position, _blackboard.TurretTrans.up, -_blackboard.TurretTrans.forward, -invertedAngle, _blackboard.MaxInstantVisionRange);
        Handles.DrawSolidArc(_blackboard.TurretTrans.position, _blackboard.TurretTrans.up, -_blackboard.TurretTrans.forward, invertedAngle, _blackboard.MaxInstantVisionRange);
        Handles.Label(_blackboard.ThisTrans.position + _blackboard.ThisTrans.right * 
            _blackboard.MaxVisionInvestigationRange, $"Max view range: {_blackboard.MaxVisionInvestigationRange}");
    }
}