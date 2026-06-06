using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnRadius = 2f;

    public Unit Spawn()
    {
        Vector3 centerPosition = _spawnPoint != null ? _spawnPoint.position : transform.position;

        Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;

        Vector3 spawnPosition = new Vector3(
            centerPosition.x + randomOffset.x,
            centerPosition.y,
            centerPosition.z + randomOffset.y
        );

        return Instantiate(_unitPrefab, spawnPosition, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 centerPosition = _spawnPoint != null ? _spawnPoint.position : transform.position;
        Gizmos.DrawWireSphere(centerPosition, _spawnRadius);
    }
}