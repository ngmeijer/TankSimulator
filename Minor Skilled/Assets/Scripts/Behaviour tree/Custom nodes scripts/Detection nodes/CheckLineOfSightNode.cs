using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Detection/CheckLineOfSightNode")]
public class CheckLineOfSightNode : BehaviourNode
{
    public override NodeState Evaluate(AIBlackboard blackboard, AIController controller)
    {
        _nodeState = CanSeePlayer(controller) ? NodeState.Success : NodeState.Failure;

        return _nodeState;
    }

    private bool CanSeePlayer(AIController controller)
    {
        bool hitCollider = Physics.Linecast(
            controller.ComponentManager.Raycaster.position,
            GameManager.Instance.Player.EntityOrigin.position, out RaycastHit hit, 1);

        return hitCollider && hit.collider.transform.root.CompareTag("Player");
    }

    public override void DrawGizmos(AIBlackboard blackboard, AIController controller)
    {
        if (_nodeState == NodeState.Success)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        if (GameManager.Instance == null)
            return;

        Gizmos.DrawLine(controller.ComponentManager.Raycaster.position,
            GameManager.Instance.Player.EntityOrigin.position);
    }
}