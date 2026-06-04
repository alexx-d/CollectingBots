using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius = 50f;
    [SerializeField] private float _scanInterval = 1f;
    [SerializeField] private LayerMask _resourceLayer;
    [SerializeField] private int _maxScanResults = 30;

    private Collider[] _scanBuffer;
    private readonly List<Resource> _discoveredResources = new List<Resource>();

    public event Action<List<Resource>> ResourcesDiscovered;

    private void Awake()
    {
        _scanBuffer = new Collider[_maxScanResults];
    }

    private void Start()
    {
        StartCoroutine(ScanRoutine());
    }

    private IEnumerator ScanRoutine()
    {
        var wait = new WaitForSeconds(_scanInterval);

        while (enabled)
        {
            yield return wait;
            Scan();
        }
    }

    private void Scan()
    {
        _discoveredResources.Clear();

        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            _scanRadius,
            _scanBuffer,
            _resourceLayer
        );

        for (int i = 0; i < hitCount; i++)
        {
            Collider collider = _scanBuffer[i];

            if (collider.TryGetComponent<Resource>(out var resource))
            {
                _discoveredResources.Add(resource);
            }

            _scanBuffer[i] = null;
        }

        if (_discoveredResources.Count > 0)
        {
            ResourcesDiscovered?.Invoke(_discoveredResources);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
}