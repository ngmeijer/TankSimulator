using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private BehaviourTree _tree;
    private AIBlackboard _blackboard;
    private NavMeshAgent _agent;
    private PatrolNode _patrolNode;
    private InvestigateNode _investigateNode;
    private ShootNode _shootNode;
    private CoverNode _coverNode;

    protected void Awake()
    {
        _agent = GetComponentInParent<NavMeshAgent>();
        _blackboard = GetComponentInParent<AIBlackboard>();
        _tree = new BehaviourTree();
    }
    
    protected void Start()
    {
        SelectorNode rootNode = new(_blackboard);
        _tree.SetRootNode(rootNode);

        _coverNode = new CoverNode(_blackboard);
        rootNode.AddChildNode(_coverNode);

        _investigateNode = new InvestigateNode(_blackboard);
        rootNode.AddChildNode(_investigateNode);
        
        _shootNode = new ShootNode(_blackboard);
        rootNode.AddChildNode(_shootNode);
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