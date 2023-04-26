using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckIfCanSeePlayer : BehaviourNode
{
    private Vector3 _playerDirection;
    private KeyValuePair<string, float> _maxRange;

    public CheckIfCanSeePlayer(KeyValuePair<string, float> maxRange, AIBlackboard blackboard) : base(blackboard)
    {
        _maxRange = maxRange;

    }

    public override NodeState Evaluate()
    {
        _nodeState = IsPlayerInVisionRange() ? NodeState.Success : NodeState.Running;

        return _nodeState;
    }

    private bool IsPlayerInVisionRange()
    {
        GizmoColor = new Color(255,0,0,DEFAULT_ALPHA);

        if (!PlayerInRange()) return false;
        Debug.Log("player in range");
        if (!IsPlayerInsideFOV()) return false;
        Debug.Log("player is inside fov");
        if (!CanSeePlayer()) return false;
        Debug.Log("raycast has hit player");
        
        GizmoColor = new Color(0, 255, 0, DEFAULT_ALPHA);
        return true;
    }

    private bool IsPlayerInsideFOV()
    {
        _playerDirection = GameManager.Instance.Player.EntityOrigin.position - _blackboard.ThisTrans.position;
        float totalAngle = Vector3.Angle(_blackboard.ThisTrans.forward, _playerDirection);
        return totalAngle <= _blackboard.ViewAngle / 2;
    }

    private bool CanSeePlayer()
    {
        bool hitCollider = Physics.Raycast(
            _blackboard.Raycaster.position,
            _playerDirection, out RaycastHit hit,
            _blackboard.MaxShootingRange);
        
        Debug.Log(hitCollider);
        
        if (!hitCollider) return false;
        if (hit.collider.transform.root.CompareTag("Player"))
            return true;
        Debug.Log(hit.collider.transform.root.name);
        return false;
    }
    
    private bool PlayerInRange()
    {
        float distance = Vector3.Distance(GameManager.Instance.Player.EntityOrigin.position, _blackboard.ThisTrans.position);

        return distance <= _maxRange.Value;
    }

    public override void DrawGizmos()
    {
        if (_nodeState == NodeState.Success)
            Gizmos.color = Color.green;
        else Gizmos.color = Color.red;
        Gizmos.DrawRay(_blackboard.ThisTrans.position, _playerDirection * _blackboard.MaxShootingRange);
            
        if (_blackboard.TurretTrans == null) return;

        if (_nodeState == NodeState.Success)
            Handles.color = new Color(255,0,0,DEFAULT_ALPHA);
        else Handles.color = new Color(0,255,0,DEFAULT_ALPHA);
        Handles.DrawSolidArc(_blackboard.TurretTrans.position, _blackboard.TurretTrans.up, _blackboard.TurretTrans.forward, -_blackboard.ViewAngle / 2, _blackboard.MaxShootingRange);
        Handles.DrawSolidArc(_blackboard.TurretTrans.position, _blackboard.TurretTrans.up, _blackboard.TurretTrans.forward, _blackboard.ViewAngle / 2, _blackboard.MaxShootingRange);
        
        //Area behind where the agent cannot see
        Handles.color = new Color(0,0,0, DEFAULT_ALPHA);
        float invertedAngle = (360 - _blackboard.ViewAngle) / 2;
        Handles.DrawSolidArc(_blackboard.TurretTrans.position, _blackboard.TurretTrans.up, -_blackboard.TurretTrans.forward, -invertedAngle, _blackboard.MaxShootingRange);
        Handles.DrawSolidArc(_blackboard.TurretTrans.position, _blackboard.TurretTrans.up, -_blackboard.TurretTrans.forward, invertedAngle, _blackboard.MaxShootingRange);
        Handles.Label(_blackboard.ThisTrans.position + _blackboard.ThisTrans.right * 
            _maxRange.Value, $"{_maxRange.Key}: {_maxRange}");
    }
}