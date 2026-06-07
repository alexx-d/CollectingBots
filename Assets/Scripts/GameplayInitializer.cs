using UnityEngine;

public class GameplayInitializer : MonoBehaviour
{
    [SerializeField] private GlobalResourceRegistry _globalRegistry;
    [SerializeField] private BaseFactory _baseFactory;
    [SerializeField] private Base _startBase;

    private void Start()
    {
        _baseFactory.Initialize(_globalRegistry);
        
        _startBase.Initialize(_globalRegistry, _baseFactory.CreateNewBase);
        _startBase.SpawnInitialUnits();
    }
}