using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMover : MonoBehaviour
{
    private NavMeshAgent _agent;
    private float _initialStoppingDistance;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _initialStoppingDistance = _agent.stoppingDistance;
    }

    public void SetDestination(Vector3 targetPosition, float customStoppingDistance = -1f)
    {
        _agent.stoppingDistance = customStoppingDistance >= 0f
            ? customStoppingDistance
            : _initialStoppingDistance;

        _agent.SetDestination(targetPosition);
    }

    public bool HasReachedDestination()
    {
        if (_agent.pathPending)
        {
            return false;
        }

        return _agent.remainingDistance <= _agent.stoppingDistance;
    }
    
    public void Stop()
    {
        _agent.ResetPath();
    }
}