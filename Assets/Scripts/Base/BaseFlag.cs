using UnityEngine;

public class BaseFlag : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private float _heightOffset = 1f;

    private Flag _currentFlag;
    private Vector3 _targetPosition;

    public Vector3 Position => _targetPosition;

    public void Set(Vector3 position)
    {
        _targetPosition = position;

        Vector3 spawnPosition = position + (Vector3.up * _heightOffset);

        if (_currentFlag == null)
        {
            _currentFlag = Instantiate(_flagPrefab, spawnPosition, _flagPrefab.transform.rotation);
        }
        else
        {
            _currentFlag.MoveTo(spawnPosition);
        }
    }

    public void Clear()
    {
        if (_currentFlag != null)
        {
            Destroy(_currentFlag.gameObject);
            _currentFlag = null;
        }
    }
}