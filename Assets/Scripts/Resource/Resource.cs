using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider))]
public class Resource : MonoBehaviour
{
    private Transform _currentParent;

    public event Action<Resource> ResourceCollected;

    public void PickUp(Transform attachPoint)
    {
        _currentParent = transform.parent;
        transform.SetParent(attachPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Collect()
    {
        ResourceCollected?.Invoke(this);
    }
}