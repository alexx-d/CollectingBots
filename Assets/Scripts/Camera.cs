#if UNITY_EDITOR
using UnityEditor; // Нужно для работы с Handles
#endif

using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _edgeThreshold = 20f;
    [SerializeField] private bool _enableScreenEdgeMovement = true;

    [Header("Настройки ограничений камеры")]
    [SerializeField] private float _minX = -50f;
    [SerializeField] private float _maxX = 50f;
    [SerializeField] private float _minZ = -50f;
    [SerializeField] private float _maxZ = 50f;

    [Header("Настройки зума")]
    [SerializeField] private float _zoomSpeed = 50f;
    [SerializeField] private float _minY = 5f;
    [SerializeField] private float _maxY = 30f;
    [SerializeField] private float _zoomSmoothing = 10f;

    private Transform _transform;
    private float _screenWidth;
    private float _screenHeight;
    private float _targetY;

    private void Awake()
    {
        _transform = transform;
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;

        _targetY = _transform.position.y;
    }

    private void Update()
    {
        bool isMouseActive = _inputReader.MousePosition != Vector2.zero;
        Vector3 moveDirection = CalculateKeyboardMovement();

        if (_enableScreenEdgeMovement && isMouseActive)
        {
            moveDirection += CalculateEdgeMovement();
        }

        Vector3 newPosition = _transform.position;
        if (moveDirection != Vector3.zero)
        {
            newPosition += moveDirection * (_moveSpeed * Time.deltaTime);
            newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
            newPosition.z = Mathf.Clamp(newPosition.z, _minZ, _maxZ);
        }


        float zoomValue = _inputReader.ZoomInput;
        if (Mathf.Abs(zoomValue) > 0.1f)
        {
            _targetY -= zoomValue * _zoomSpeed * Time.deltaTime;
            _targetY = Mathf.Clamp(_targetY, _minY, _maxY);
        }

        newPosition.y = Mathf.Lerp(newPosition.y, _targetY, _zoomSmoothing * Time.deltaTime);

        _transform.position = newPosition;
    }

    private Vector3 CalculateKeyboardMovement()
    {
        Vector2 input = _inputReader.MovementInput;
        return new Vector3(input.x, 0f, input.y);
    }

    private Vector3 CalculateEdgeMovement()
    {
        Vector2 mousePosition = _inputReader.MousePosition;
        Vector3 edgeDirection = Vector3.zero;

        if (mousePosition.x >= _screenWidth - _edgeThreshold)
        {
            edgeDirection.x = 1f;
        }
        else if (mousePosition.x <= _edgeThreshold)
        {
            edgeDirection.x = -1f;
        }

        if (mousePosition.y >= _screenHeight - _edgeThreshold)
        {
            edgeDirection.z = 1f;
        }
        else if (mousePosition.y <= _edgeThreshold)
        {
            edgeDirection.z = -1f;
        }

        return edgeDirection.normalized;
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Handles.color = Color.red;
        float thickness = 6f;

        Vector3 topLeft = new Vector3(_minX, 0f, _maxZ);
        Vector3 topRight = new Vector3(_maxX, 0f, _maxZ);
        Vector3 bottomLeft = new Vector3(_minX, 0f, _minZ);
        Vector3 bottomRight = new Vector3(_maxX, 0f, _minZ);

        Vector3[] points = new Vector3[] { topLeft, topRight, bottomRight, bottomLeft, topLeft };

        Handles.DrawAAPolyLine(thickness, points);
#endif
    }
}