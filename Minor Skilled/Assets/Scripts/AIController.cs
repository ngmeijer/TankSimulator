using FSM;
using Tank_components;
using TankComponents;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent NavAgent;
    public StateSwitcher StateSwitcher;
    public TankComponentManager ComponentManager;
    [SerializeField] private BehaviourTree _tree;
    [SerializeField] private AIBlackboard _blackboard;
    public Transform[] CoverRaycastPositions;

    protected void Awake()
    {
        if (NavAgent == null)
            NavAgent = GetComponentInChildren<NavMeshAgent>();
        ComponentManager = GetComponent<TankComponentManager>();
        _blackboard.ResetValues();
        _tree.ResetNodes();
    }

    public TankState GetState(E_TankState tankState)
    {
        switch (tankState)
        {
            case E_TankState.Combat:
                return StateSwitcher.CombatState;
        }

        return null;
    }

    private void Update()
    {
        _tree.EvaluateTree(_blackboard, this);
    }

    private void OnDrawGizmos()
    {
        if (_tree == null) return;
        
        _tree.DrawGizmos(_blackboard, this);
    }
}