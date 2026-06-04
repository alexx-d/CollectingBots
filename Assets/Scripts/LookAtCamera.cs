using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _mainCameraTransform;

    private void Start()
    {
        if (UnityEngine.Camera.main != null)
        {
            _mainCameraTransform = UnityEngine.Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        if (_mainCameraTransform != null)
        {
            transform.rotation = _mainCameraTransform.rotation;
        }
    }
}