using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStorage : MonoBehaviour
{
    [SerializeField] private float _deliveryRadius = 3f;

    public event Action<int> ResourceCountChanged;
    public event Action<Unit> UnitRegistered;

    public int ResourceCount { get; private set; }
    public float DeliveryRadius => _deliveryRadius;
    public IReadOnlyList<Unit> AllUnits => _allUnits;

    private readonly List<Unit> _allUnits = new List<Unit>();
    private readonly HashSet<Resource> _availableResources = new HashSet<Resource>();
    private readonly HashSet<Resource> _reservedResources = new HashSet<Resource>();

    public void RegisterUnit(Unit unit)
    {
        _allUnits.Add(unit);
        UnitRegistered?.Invoke(unit);
    }

    public void AddResource()
    {
        ResourceCount++;
        ResourceCountChanged?.Invoke(ResourceCount);
    }

    public void SpendResources(int amount)
    {
        ResourceCount -= amount;
        ResourceCountChanged?.Invoke(ResourceCount);
    }

    public void RegisterFoundResource(Resource resource)
    {
        if (!_availableResources.Contains(resource) && !_reservedResources.Contains(resource))
        {
            _availableResources.Add(resource);
        }
    }

    public Resource GetTargetResourceForUnit()
    {
        if (_availableResources.Count == 0)
        {
            return null;
        }

        using var enumerator = _availableResources.GetEnumerator();
        enumerator.MoveNext();
        Resource target = enumerator.Current;

        _availableResources.Remove(target);
        _reservedResources.Add(target);

        return target;
    }

    public void RemoveResourceFromMap(Resource resource)
    {
        if (_reservedResources.Contains(resource))
        {
            _reservedResources.Remove(resource);
        }
    }
}