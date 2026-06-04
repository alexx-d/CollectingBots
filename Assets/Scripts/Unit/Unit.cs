using DG.Tweening;
using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private enum UnitState
    {
        Idle,
        MovingToResource,
        ReturningToBase
    }

    [SerializeField] private UnitMover _mover;
    [SerializeField] private UnitCollector _collector;
    [SerializeField] private Transform _backpackAttachPoint;

    private UnitState _currentState = UnitState.Idle;
    private BaseStorage _assignedStorage;
    private Resource _targetResource;

    public event Action<Unit> UnitBecameFree;
    public event Action<Resource> ResourceCollected;

    public bool IsFree => _currentState == UnitState.Idle;

    private void OnEnable()
    {
        _collector.ResourceContacted += HandleResourceContact;
    }

    private void OnDisable()
    {
        _collector.ResourceContacted -= HandleResourceContact;
    }

    public void Initialize(BaseStorage homeBaseStorage)
    {
        _assignedStorage = homeBaseStorage;
        ResetToIdle();
    }

    public void AssignResource(Resource resource)
    {
        _targetResource = resource;
        _currentState = UnitState.MovingToResource;
        _mover.SetDestination(_targetResource.transform.position);
    }

    private void HandleResourceContact(Resource resource)
    {
        if (_currentState != UnitState.MovingToResource)
        {
            return;
        }

        if (resource != _targetResource)
        {
            return;
        }

        PickUpResource();
    }

    private void PickUpResource()
    {
        _targetResource.PickUp(_backpackAttachPoint);
        _currentState = UnitState.ReturningToBase;
        _mover.SetDestination(_assignedStorage.transform.position, _assignedStorage.DeliveryRadius);
    }

    private void Update()
    {
        if (_currentState == UnitState.ReturningToBase && _mover.HasReachedDestination())
        {
            DeliverResource();
        }
    }

    private void DeliverResource()
    {
        _targetResource.Collect();
        
        _assignedStorage.AddResource();
        _assignedStorage.RemoveResourceFromMap(_targetResource);
        _targetResource = null;

        ResetToIdle();
    }

    private void ResetToIdle()
    {
        _currentState = UnitState.Idle;
        _mover.Stop();
        UnitBecameFree?.Invoke(this);
    }
}