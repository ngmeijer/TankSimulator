using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour tree/Cover/FindSuitableCoverNode")]
public class FindSuitableCoverNode : CombatNode
{
    public override NodeState Evaluate()
    {
        return _nodeState;
    }

    public override void DrawGizmos()
    {
        
    }
}