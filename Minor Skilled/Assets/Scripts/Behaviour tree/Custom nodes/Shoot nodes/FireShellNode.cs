using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Shooting/FireShellNode")]
public class FireShellNode : CombatNode
{
    public override NodeState Evaluate()
    {
        _combatState.TestFunc();

        _nodeState = NodeState.Success;
        return _nodeState;
    }
}