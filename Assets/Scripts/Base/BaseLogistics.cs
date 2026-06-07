using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLogistics : MonoBehaviour
{
    [SerializeField] private float _dispatchInterval = 0.2f;

    private readonly List<Unit> _freeUnits = new List<Unit>();
    private Func<Resource> _resourceProvider;

    public int FreeUnitCount => _freeUnits.Count;

    public void Initialize(Func<Resource> resourceProvider)
    {
        _resourceProvider = resourceProvider;
        StartCoroutine(DispatchRoutine());
    }

    public void AddFreeUnit(Unit unit)
    {
        if (_freeUnits.Contains(unit) == false)
        {
            _freeUnits.Add(unit);
        }
    }

    public bool TryExtractFreeUnit(out Unit freeUnit)
    {
        if (_freeUnits.Count > 0)
        {
            freeUnit = _freeUnits[0];
            _freeUnits.RemoveAt(0);
            return true;
        }

        freeUnit = null;
        return false;
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
        if (_resourceProvider == null) return;

        while (_freeUnits.Count > 0)
        {
            Resource resource = _resourceProvider.Invoke();

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