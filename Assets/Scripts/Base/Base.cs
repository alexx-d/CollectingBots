using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private enum BasePriority
    {
        BuildUnits,
        BuildBase
    }

    [SerializeField] private BaseScanner _scanner;
    [SerializeField] private BaseStorage _storage;
    [SerializeField] private UnitRegistry _unitRegistry;
    [SerializeField] private ResourceRegistry _resourceRegistry;
    [SerializeField] private BaseLogistics _logistics;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private BaseColorizer _colorizer;
    [SerializeField] private BaseFlag _flag;

    [SerializeField] private int _initialUnitCount = 3;
    [SerializeField] private int _unitCost = 3;
    [SerializeField] private int _baseCost = 5;

    private GlobalResourceRegistry _globalRegistry;
    private Action<Vector3, Unit> _onBaseBuildRequested;
    private BasePriority _currentPriority = BasePriority.BuildUnits;

    private bool _isScanningPaused = false;

    private void OnEnable()
    {
        _unitRegistry.Registered += HandleUnitRegistered;
        _unitRegistry.AnyUnitBecameFree += HandleUnitBecameFree;
        _unitRegistry.AnyUnitResourceDelivered += HandleResourceDelivered;

        _scanner.ResourcesDiscovered += HandleResourcesDiscovered;
    }

    private void OnDisable()
    {
        _unitRegistry.Registered -= HandleUnitRegistered;
        _unitRegistry.AnyUnitBecameFree -= HandleUnitBecameFree;
        _unitRegistry.AnyUnitResourceDelivered -= HandleResourceDelivered;

        _scanner.ResourcesDiscovered -= HandleResourcesDiscovered;
    }

    public void Initialize(GlobalResourceRegistry globalRegistry, Action<Vector3, Unit> onBaseBuildRequested)
    {
        _globalRegistry = globalRegistry;
        _onBaseBuildRequested = onBaseBuildRequested;

        _resourceRegistry.Initialize(globalRegistry);

        _logistics.Initialize(_resourceRegistry.GetResource);

        _colorizer.GenerateAndApplyColor();

        _scanner.CanScanPredicate = EvaluateCanScan;
    }

    public void SpawnInitialUnits()
    {
        for (int i = 0; i < _initialUnitCount; i++)
        {
            CreateAndRegisterUnit();
        }
    }

    public void SetFlag(Vector3 position)
    {
        _flag.Set(position);
        _currentPriority = BasePriority.BuildBase;
        CheckEconomyRequirements();
    }

    public void ResetFlag()
    {
        _flag.Clear();
        _currentPriority = BasePriority.BuildUnits;
        CheckEconomyRequirements();
    }

    private void CreateAndRegisterUnit()
    {
        Unit newUnit = _unitSpawner.Spawn();
        _unitRegistry.Register(newUnit);
    }

    public void RegisterUnit(Unit unit)
    {
        _unitRegistry.Register(unit);
    }

    private bool EvaluateCanScan()
    {
        int queuedResources = _resourceRegistry.QueuedCount;
        int totalUnits = _unitRegistry.AllUnits.Count;

        if (_isScanningPaused == false && queuedResources > totalUnits)
        {
            _isScanningPaused = true;
        }
        else if (_isScanningPaused && queuedResources == 0)
        {
            _isScanningPaused = false;
        }

        return _isScanningPaused == false;
    }

    private void HandleUnitRegistered(Unit unit)
    {
        unit.Initialize(_storage);
        _colorizer.ColorizeUnit(unit);

        _logistics.AddFreeUnit(unit);
    }

    private void HandleUnitBecameFree(Unit unit)
    {
        _logistics.AddFreeUnit(unit);

        CheckEconomyRequirements();
    }

    private void HandleResourceDelivered(Resource resource)
    {
        _globalRegistry.Unregister(resource);
    }

    private void HandleResourcesDiscovered(List<Resource> resources)
    {
        _resourceRegistry.AddResources(resources);
    }

    private void CheckEconomyRequirements()
    {
        int currentResources = _storage.ResourceCount;

        if (_currentPriority == BasePriority.BuildBase && currentResources >= _baseCost && _unitRegistry.AllUnits.Count > 1)
        {
            if (_logistics.TryExtractFreeUnit(out Unit builder))
            {
                _storage.SpendResources(_baseCost);
                _unitRegistry.Unregister(builder);

                Vector3 targetPosition = _flag.Position;
                _flag.Clear();

                _currentPriority = BasePriority.BuildUnits;

                builder.AssignBuildOrder(targetPosition, SendBuildRequest);
            }
        }
        else if (_currentPriority == BasePriority.BuildUnits && currentResources >= _unitCost)
        {
            _storage.SpendResources(_unitCost);
            CreateAndRegisterUnit();
        }
    }

    private void SendBuildRequest(Vector3 position, Unit builder)
    {
        _onBaseBuildRequested?.Invoke(position, builder);
    }
}