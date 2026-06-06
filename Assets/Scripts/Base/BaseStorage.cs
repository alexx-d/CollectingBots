using System;
using UnityEngine;

public class BaseStorage : MonoBehaviour
{
    [SerializeField] private float _deliveryRadius = 3f;

    public event Action<int> ResourceCountChanged;

    public int ResourceCount { get; private set; }
    public float DeliveryRadius => _deliveryRadius;

    public void AddResource()
    {
        ResourceCount++;
        ResourceCountChanged?.Invoke(ResourceCount);
    }

    public void SpendResources(int amount)
    {
        ResourceCount -= amount;
        ResourceCountChanged?.Invoke(ResourceCount);
    }
}