using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckIfPlayerInRangeNode")]
public class CheckIfPlayerInRangeNode : BehaviourNode
{
    public CustomKeyValue MaxRange;
    public CustomKeyValue FOVRange;
    
    public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        _nodeState = PlayerInRange(controller) ? NodeState.Success : NodeState.Failure;

        return _nodeState;
    }

    public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
    {
        base.DrawGizmos(blackboard, controller);
        
        if (MaxRange.RangeName == "")
            return;
        Handles.Label(controller.transform.position + controller.transform.right * 
            MaxRange.RangeValue, $"{MaxRange.RangeName}: {MaxRange.RangeValue}");
    }

    private bool PlayerInRange(AIController controller)
    {
        float distance =
            Vector3.Distance(GameManager.Instance.Player.EntityOrigin.position, controller.transform.position);

        return distance <= MaxRange.RangeValue;
    }
}

[Serializable]
public class CustomKeyValue
{
    [field: SerializeField] public string RangeName { set; get;}
    [field: SerializeField] public float RangeValue { set; get;}
}