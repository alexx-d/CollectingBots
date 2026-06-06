using System.Collections;
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

    public IEnumerator MoveTo(Vector3 targetPosition, float customStoppingDistance = -1f)
    {
        _agent.stoppingDistance = customStoppingDistance >= 0f
            ? customStoppingDistance
            : _initialStoppingDistance;

        _agent.SetDestination(targetPosition);

        yield return null;

        while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)
        {
            yield return null;
        }
    }
}