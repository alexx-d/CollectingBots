using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStorage : MonoBehaviour
{
    [SerializeField] private float _deliveryRadius = 3f;

    private readonly List<Unit> _allUnits = new List<Unit>();
    private int _resourceCount;

    public event Action<int> OnResourceCountChanged;
    public event Action<Unit> OnUnitRegistered;

    public float DeliveryRadius => _deliveryRadius;
    public int ResourceCount => _resourceCount;
    public IReadOnlyList<Unit> AllUnits => _allUnits;

    public void RegisterUnit(Unit unit)
    {
        _allUnits.Add(unit);
        OnUnitRegistered?.Invoke(unit);
    }

    public void AddResource()
    {
        _resourceCount++;
        OnResourceCountChanged?.Invoke(_resourceCount);
    }

    public void SpendResources(int amount)
    {
        _resourceCount -= amount;
        OnResourceCountChanged?.Invoke(_resourceCount);
    }
}