using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitMover _mover;
    [SerializeField] private Transform _backpackAttachPoint;

    private BaseStorage _assignedStorage;
    private Coroutine _currentRoutine;

    public event Action<Unit> BecameFree;
    public event Action<Resource> ResourceDelivered;

    public void Initialize(BaseStorage homeBaseStorage)
    {
        _assignedStorage = homeBaseStorage;
        ResetToIdle();
    }

    public void AssignResource(Resource resource)
    {
        if (_currentRoutine != null)
        {
            StopCoroutine(_currentRoutine);
        }

        _currentRoutine = StartCoroutine(ProcessDeliveryRoutine(resource));
    }

    private IEnumerator ProcessDeliveryRoutine(Resource resource)
    {
        yield return _mover.MoveTo(resource.transform.position);

        resource.PickUp(_backpackAttachPoint);

        yield return _mover.MoveTo(_assignedStorage.transform.position, _assignedStorage.DeliveryRadius);

        resource.Collect();
        _assignedStorage.AddResource();
        ResourceDelivered?.Invoke(resource);

        ResetToIdle();
    }

    private void ResetToIdle()
    {
        _currentRoutine = null;
        BecameFree?.Invoke(this);
    }
}