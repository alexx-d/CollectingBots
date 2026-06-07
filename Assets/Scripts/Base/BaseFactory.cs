using UnityEngine;

public class BaseFactory : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private FishFactory _fishFactory;

    private GlobalResourceRegistry _globalRegistry;

    public void Initialize(GlobalResourceRegistry globalRegistry)
    {
        _globalRegistry = globalRegistry;
    }

    public void CreateNewBase(Vector3 position, Unit builder)
    {
        Base newBase = Instantiate(_basePrefab, position, Quaternion.identity);
        newBase.transform.SetParent(_container, true);

        newBase.Initialize(_globalRegistry, CreateNewBase);

        newBase.RegisterUnit(builder);

        _fishFactory.SpawnFishNear(position, _container);
    }
}