using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Behaviour tree/Movement/MoveToPositionNode")]
public class MoveToPositionNode : BehaviourNode
{
    private Vector3 _targetPos;
    private float _currentTime;
    [SerializeField] private CustomKeyValue MaxStoppingDistance;

    public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        float distanceToTargetPos = Vector3.Distance(controller.transform.position, blackboard.MoveToPosition);
        if (distanceToTargetPos <= MaxStoppingDistance.RangeValue)
        {
            controller.NavAgent.ResetPath();
            _nodeState = NodeState.Success;
            return _nodeState;
        }

        _nodeState = NodeState.Running;
        _currentTime += Time.deltaTime;
        if (_currentTime > blackboard.PathCalculationInterval)
        {
            NavMeshPath path = new();
            _currentTime = 0f;
            controller.NavAgent.CalculatePath(blackboard.MoveToPosition, path);
            controller.NavAgent.SetPath(path);
        }
        
        return _nodeState;
    }

    public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(controller.transform.position, controller.transform.up, MaxStoppingDistance.RangeValue);
        Handles.Label(controller.transform.position + Vector3.right * MaxStoppingDistance.RangeValue, $"{MaxStoppingDistance.RangeName}: {MaxStoppingDistance.RangeValue}");

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(blackboard.MoveToPosition, 2f);

        if (controller.NavAgent.path != null)
        {
            Vector3[] pathPositions = controller.NavAgent.path.corners;
            
            Gizmos.color = Color.yellow;
            foreach (var pathPoint in pathPositions)
            {
                Gizmos.DrawSphere(pathPoint, 0.5f);
            }
        }
    }
}