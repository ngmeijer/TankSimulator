using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Shooting/CheckReloadingNode")]
public class CheckReloadingNode : CombatNode
{
    public override NodeState Evaluate()
    {
        _nodeState = IsReloading() ? NodeState.Running : NodeState.Success;

        return _nodeState;
    }

    private bool IsReloading()
    {
        return _combatState.IsReloading();
    }
}