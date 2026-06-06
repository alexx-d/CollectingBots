using System.Collections.Generic;
using UnityEngine;

public class ResourceRegistry : MonoBehaviour
{
    private readonly HashSet<Resource> _availableResources = new HashSet<Resource>();
    private readonly HashSet<Resource> _reservedResources = new HashSet<Resource>();

    public void Register(Resource resource)
    {
        if (!_availableResources.Contains(resource) && !_reservedResources.Contains(resource))
        {
            _availableResources.Add(resource);
        }
    }

    public Resource GetTarget()
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

    public void Unregister(Resource resource)
    {
        if (_reservedResources.Contains(resource))
        {
            _reservedResources.Remove(resource);
        }
    }
}