using System;
using UnityEngine;

public class UnitCollector : MonoBehaviour
{
    public event Action<Resource> ResourceContacted;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Resource>(out var resource))
        {
            ResourceContacted?.Invoke(resource);
        }
    }
}