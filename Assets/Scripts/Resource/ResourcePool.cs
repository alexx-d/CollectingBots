public class ResourcePool : ObjectPool<Resource>
{
    private void Start()
    {
        InitializePool();
    }
}