using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Shooting/CheckShellCountNode")]
public class CheckShellCountNode : CombatNode
{
    public override NodeState Evaluate()
    {
        _nodeState = NodeState.Failure;

        if (HasShells())
            _nodeState = NodeState.Success;
        
        return _nodeState;
    }

    private bool HasShells()
    {
        return _combatState.HasShells();
    }
}