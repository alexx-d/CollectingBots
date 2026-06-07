using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitRegistry : MonoBehaviour
{
    private readonly List<Unit> _allUnits = new List<Unit>();

    public event Action<Unit> Registered;
    public event Action<Unit> AnyUnitBecameFree;
    public event Action<Resource> AnyUnitResourceDelivered;

    public IReadOnlyList<Unit> AllUnits => _allUnits;

    public void Register(Unit unit)
    {
        _allUnits.Add(unit);

        unit.BecameFree += HandleUnitBecameFree;
        unit.ResourceDelivered += HandleResourceDelivered;

        Registered?.Invoke(unit);
    }

    public void Unregister(Unit unit)
    {
        if (_allUnits.Contains(unit))
        {
            _allUnits.Remove(unit);

            unit.BecameFree -= HandleUnitBecameFree;
            unit.ResourceDelivered -= HandleResourceDelivered;
        }
    }

    private void HandleUnitBecameFree(Unit unit) => AnyUnitBecameFree?.Invoke(unit);
    private void HandleResourceDelivered(Resource resource) => AnyUnitResourceDelivered?.Invoke(resource);
}