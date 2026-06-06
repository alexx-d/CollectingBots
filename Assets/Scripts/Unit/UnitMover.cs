using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMover : MonoBehaviour
{
    private NavMeshAgent _agent;
    private float _initialStoppingDistance;
    private bool _isTargetReached;

    public event Action DestinationReached;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _initialStoppingDistance = _agent.stoppingDistance;
    }

    private void Update()
    {
        if (_agent.pathPending || _isTargetReached)
        {
            return;
        }

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _isTargetReached = true;
            DestinationReached?.Invoke();
        }
    }

    public void SetDestination(Vector3 targetPosition, float customStoppingDistance = -1f)
    {
        _agent.stoppingDistance = customStoppingDistance >= 0f
            ? customStoppingDistance
            : _initialStoppingDistance;

        _isTargetReached = false;
        _agent.SetDestination(targetPosition);
    }

    public void Stop()
    {
        _isTargetReached = true;
        _agent.ResetPath();
    }
}