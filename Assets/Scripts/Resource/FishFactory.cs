using UnityEngine;

public class FishFactory : MonoBehaviour
{
    [SerializeField] private GameObject _fishSpotPrefab;

    [SerializeField] private float _baseSpawnRadius = 15f;
    [SerializeField] private float _fishSpotRadius = 7.5f;

    [SerializeField] private LayerMask _oceanLayer;
    [SerializeField] private LayerMask _groundLayer;

    public void SpawnFishNear(Vector3 centerPosition, Transform container)
    {
        LayerMask combinedMask = _oceanLayer | _groundLayer;

        int ringsCount = 3;
        int pointsPerRing = 12;
        float radiusStep = 5f;

        Vector3 fallbackPosition = Vector3.zero;
        bool foundAnyWater = false;

        for (int ring = 0; ring < ringsCount; ring++)
        {
            float currentRadius = _baseSpawnRadius + (ring * radiusStep);

            for (int i = 0; i < pointsPerRing; i++)
            {
                float angle = i * (360f / pointsPerRing) * Mathf.Deg2Rad;

                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * currentRadius;
                Vector3 rayStart = new Vector3(centerPosition.x + offset.x, 100f, centerPosition.z + offset.z);

                if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hitInfo, 120f, combinedMask))
                {
                    if (((1 << hitInfo.collider.gameObject.layer) & _oceanLayer) != 0)
                    {
                        if (!foundAnyWater)
                        {
                            fallbackPosition = hitInfo.point;
                            foundAnyWater = true;
                        }

                        Vector3 boxHalfExtents = new Vector3(_fishSpotRadius, 0.1f, _fishSpotRadius);
                        Vector3 boxCenter = hitInfo.point + Vector3.up * 0.05f;

                        bool isTooCloseToShore = Physics.CheckBox(boxCenter, boxHalfExtents, Quaternion.identity, _groundLayer);

                        if (!isTooCloseToShore)
                        {
                            SpawnPrefab(hitInfo.point, container);
                            return;
                        }
                    }
                }
            }
        }

        if (foundAnyWater)
        {
            SpawnPrefab(fallbackPosition, container);
        }
    }

    private void SpawnPrefab(Vector3 position, Transform container)
    {
        GameObject fishSpot = Instantiate(_fishSpotPrefab, position, Quaternion.identity);
        fishSpot.transform.SetParent(container, true);
    }
}