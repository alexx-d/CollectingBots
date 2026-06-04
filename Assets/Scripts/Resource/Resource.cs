using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Resource : MonoBehaviour
{
    private Collider _collider;
    private Transform _transform;

    public event Action<Resource> OnResourceCollected;

    public bool IsTargeted { get; private set; }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _transform = transform;
    }

    public void PrepareForSpawn()
    {
        _collider.enabled = true;
        IsTargeted = false;
    }

    public void Target()
    {
        IsTargeted = true;
    }

    public void Untarget()
    {
        IsTargeted = false;
    }

    public void Collect(Transform attachPoint)
    {
        _collider.enabled = false;
        _transform.SetParent(attachPoint);
        _transform.localPosition = Vector3.zero;
        _transform.localRotation = Quaternion.identity;
    }

    public void Disable()
    {
        OnResourceCollected?.Invoke(this);
    }
}