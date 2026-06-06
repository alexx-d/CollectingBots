using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BaseStorage _storage;
    [SerializeField] private UnitRegistry _unitRegistry;
    [SerializeField] private ResourceRegistry _resourceRegistry;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private BaseScanner _scanner;

    [SerializeField] private int _initialUnitCount = 3;
    [SerializeField] private float _dispatchInterval = 0.2f;

    private readonly List<Unit> _freeUnits = new List<Unit>();

    private void OnEnable()
    {
        _scanner.ResourcesDiscovered += HandleResourcesDiscovered;
        _unitRegistry.Registered += HandleUnitRegistered;
    }

    private void OnDisable()
    {
        _scanner.ResourcesDiscovered -= HandleResourcesDiscovered;
        _unitRegistry.Registered -= HandleUnitRegistered;

        foreach (var unit in _unitRegistry.AllUnits)
        {
            unit.BecameFree -= HandleUnitBecameFree;
            unit.ResourceDelivered -= HandleResourceDelivered;
        }
    }

    private void Start()
    {
        InitializeInitialUnits();
        StartCoroutine(DispatchRoutine());
    }

    private void InitializeInitialUnits()
    {
        for (int i = 0; i < _initialUnitCount; i++)
        {
            CreateAndRegisterUnit();
        }
    }

    private void CreateAndRegisterUnit()
    {
        Unit newUnit = _unitSpawner.Spawn();
        _unitRegistry.Register(newUnit);
    }

    private void HandleUnitRegistered(Unit unit)
    {
        unit.Initialize(_storage);
        unit.BecameFree += HandleUnitBecameFree;
        unit.ResourceDelivered += HandleResourceDelivered;
        _freeUnits.Add(unit);
    }

    private void HandleResourcesDiscovered(List<Resource> resources)
    {
        foreach (var resource in resources)
        {
            _resourceRegistry.Register(resource);
        }
    }

    private void HandleUnitBecameFree(Unit unit)
    {
        if (_freeUnits.Contains(unit) == false)
        {
            _freeUnits.Add(unit);
        }
    }

    private void HandleResourceDelivered(Resource resource)
    {
        _resourceRegistry.Unregister(resource);
    }

    private IEnumerator DispatchRoutine()
    {
        var wait = new WaitForSeconds(_dispatchInterval);

        while (enabled)
        {
            yield return wait;
            TryAssignCommands();
        }
    }

    private void TryAssignCommands()
    {
        while (_freeUnits.Count > 0)
        {
            Resource resource = _resourceRegistry.GetTarget();

            if (resource == null)
            {
                break;
            }

            Unit unit = _freeUnits[0];
            _freeUnits.RemoveAt(0);

            unit.AssignResource(resource);
        }
    }
}