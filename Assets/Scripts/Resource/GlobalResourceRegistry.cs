using System.Collections.Generic;
using UnityEngine;

public class GlobalResourceRegistry : MonoBehaviour
{
    private readonly HashSet<Resource> _trackedResources = new HashSet<Resource>();

    public bool TryRegister(Resource resource)
    {
        if (resource == null || _trackedResources.Contains(resource))
        {
            return false;
        }

        _trackedResources.Add(resource);
        return true;
    }

    public void Unregister(Resource resource)
    {
        if (resource != null && _trackedResources.Contains(resource))
        {
            _trackedResources.Remove(resource);
        }
    }
}