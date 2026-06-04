using System;
using UnityEngine;

public class UnitCollector : MonoBehaviour
{
    public event Action<Resource> OnResourceContacted;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Resource>(out var resource))
        {
            OnResourceContacted?.Invoke(resource);
        }
    }
}