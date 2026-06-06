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
    [SerializeField] private Transform _backpackAttachPoint;

    private UnitState _currentState = UnitState.Idle;
    private BaseStorage _assignedStorage;
    private Resource _targetResource;

    public event Action<Unit> BecameFree;
    public event Action<Resource> ResourceDelivered;

    private void OnEnable()
    {
        _mover.DestinationReached += HandleDestinationReached;
    }

    private void OnDisable()
    {
        _mover.DestinationReached -= HandleDestinationReached;
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

    private void HandleDestinationReached()
    {
        switch (_currentState)
        {
            case UnitState.MovingToResource:
                PickUpResource();
                break;

            case UnitState.ReturningToBase:
                DeliverResource();
                break;

            default:
                ResetToIdle();
                break;
        }
    }

    private void PickUpResource()
    {
        _targetResource.PickUp(_backpackAttachPoint);
        _currentState = UnitState.ReturningToBase;
        _mover.SetDestination(_assignedStorage.transform.position, _assignedStorage.DeliveryRadius);
    }

    private void DeliverResource()
    {
        _targetResource.Collect();
        _assignedStorage.AddResource();

        ResourceDelivered?.Invoke(_targetResource);
        _targetResource = null;

        ResetToIdle();
    }

    private void ResetToIdle()
    {
        _currentState = UnitState.Idle;
        _mover.Stop();
        BecameFree?.Invoke(this);
    }
}