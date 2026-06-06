using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitRegistry : MonoBehaviour
{
    private readonly List<Unit> _allUnits = new List<Unit>();

    public event Action<Unit> Registered;
    public event Action<Unit> Unregistered;

    public IReadOnlyList<Unit> AllUnits => _allUnits;

    public void Register(Unit unit)
    {
        _allUnits.Add(unit);
        Registered?.Invoke(unit);
    }

    public void Unregister(Unit unit)
    {
        if (_allUnits.Contains(unit))
        {
            _allUnits.Remove(unit);
            Unregistered?.Invoke(unit);
        }
    }
}