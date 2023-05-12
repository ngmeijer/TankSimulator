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
    private PatrolNode _patrolNode;
    private InvestigateNode _investigateNode;
    private ShootNode _shootNode;
    private CoverNode _coverNode;

    protected void Awake()
    {
        if (NavAgent == null)
            NavAgent = GetComponentInChildren<NavMeshAgent>();
        ComponentManager = GetComponent<TankComponentManager>();
        _blackboard.AIController = this;
        _blackboard.ThisTrans = transform;
        _tree.AssignBlackboardToTreeNodes(_blackboard);
    }

    private void Update()
    {
        _tree.EvaluateTree();
    }

    private void OnDrawGizmos()
    {
        if (_tree == null) return;
        
        _tree.DrawGizmos();
    }
}