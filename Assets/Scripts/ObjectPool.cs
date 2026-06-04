using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] private T[] _prefabs;
    [SerializeField] private int _initialSize;

    private readonly Queue<T> _pool = new Queue<T>();

    protected void InitializePool()
    {
        if (_prefabs == null || _prefabs.Length == 0)
        {
            return;
        }

        for (int i = 0; i < _initialSize; i++)
        {
            T randomPrefab = _prefabs[Random.Range(0, _prefabs.Length)];
            T newObject = Instantiate(randomPrefab, transform);
            newObject.gameObject.SetActive(false);
            _pool.Enqueue(newObject);
        }
    }

    public T Get()
    {
        if (_pool.Count > 0)
        {
            T obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        T randomPrefab = _prefabs[Random.Range(0, _prefabs.Length)];

        return Instantiate(randomPrefab, transform);
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        _pool.Enqueue(obj);
    }
}