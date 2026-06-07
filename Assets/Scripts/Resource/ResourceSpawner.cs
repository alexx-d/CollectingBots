using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private ResourcePool _pool;
    [SerializeField] private Vector3 _spawnAreaSize;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private float _spawnHeightY = 0.5f;

    [SerializeField] private float _minScaleMultiplier = 0.8f;
    [SerializeField] private float _maxScaleMultiplier = 1.2f;
    [SerializeField] private float _minRotationAngle = 0f;
    [SerializeField] private float _maxRotationAngle = 360f;

    private void Start()
    {
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

        Resource resource = _pool.Get();
        resource.transform.position = spawnPosition;

        float randomAngleY = Random.Range(_minRotationAngle, _maxRotationAngle);
        resource.transform.rotation = Quaternion.Euler(0f, randomAngleY, 0f);

        float randomScaleMultiplier = Random.Range(_minScaleMultiplier, _maxScaleMultiplier);
        resource.transform.localScale = Vector3.one * randomScaleMultiplier;

        resource.ResourceCollected += HandleResourceCollected;
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
        resource.ResourceCollected -= HandleResourceCollected;
        _pool.ReturnToPool(resource);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 gizmoCenter = transform.position + Vector3.up * _spawnHeightY;
        Gizmos.DrawWireCube(gizmoCenter, _spawnAreaSize);
    }
}