using System.Collections.Generic;
using UnityEngine;

public class ResourceRegistry : MonoBehaviour
{
    private readonly List<Resource> _targets = new List<Resource>();
    private GlobalResourceRegistry _globalRegistry;

    public int QueuedCount => _targets.Count;

    public void Initialize(GlobalResourceRegistry globalRegistry)
    {
        _globalRegistry = globalRegistry;
    }

    public void AddResources(List<Resource> resources)
    {
        foreach (var resource in resources)
        {
            if (_globalRegistry.TryRegister(resource))
            {
                _targets.Add(resource);
            }
        }
    }

    public Resource GetResource()
    {
        if (_targets.Count > 0)
        {
            Resource target = _targets[0];
            _targets.RemoveAt(0);
            return target;
        }
        return null;
    }
}