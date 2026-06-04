using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceSpawner : ObjectPool<Resource>
{
    [SerializeField] private Vector3 _spawnAreaSize;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private float _spawnHeightY = 0.5f;

    public static event Action<Resource> OnResourceSpawned;

    private Coroutine _spawnCoroutine;

    private void Start()
    {
        InitializePool();
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        var wait = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            yield return wait;
            SpawnResource();
        }
    }

    private void SpawnResource()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        Resource resource = Get();
        resource.transform.position = spawnPosition;
        resource.transform.rotation = Quaternion.identity;
        resource.PrepareForSpawn();

        resource.OnResourceCollected += HandleResourceCollected;

        OnResourceSpawned?.Invoke(resource);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float halfWidth = _spawnAreaSize.x / 2f;
        float halfLength = _spawnAreaSize.z / 2f;

        float randomX = Random.Range(-halfWidth, halfWidth);
        float randomZ = Random.Range(-halfLength, halfLength);

        Vector3 randomOffset = new Vector3(randomX, _spawnHeightY, randomZ);

        return transform.position + randomOffset;
    }

    private void HandleResourceCollected(Resource resource)
    {
        resource.OnResourceCollected -= HandleResourceCollected;
        ReturnToPool(resource);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 gizmoCenter = transform.position + Vector3.up * _spawnHeightY;
        Gizmos.DrawWireCube(gizmoCenter, _spawnAreaSize);
    }
}