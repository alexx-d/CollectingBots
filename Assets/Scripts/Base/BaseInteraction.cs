using UnityEngine;

public class BaseInteraction : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _baseLayer;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _obstacleLayer;

    private Base _selectedBase;
    private BaseSelection _currentSelection;
    private LayerMask _combinedLayerMask;

    private void Awake()
    {
        _combinedLayerMask = _baseLayer | _groundLayer | _obstacleLayer;
    }

    private void OnEnable()
    {
        _inputReader.LeftClickPressed += HandleLeftClick;
    }

    private void OnDisable()
    {
        _inputReader.LeftClickPressed -= HandleLeftClick;
    }

    private void HandleLeftClick()
    {
        Ray ray = _camera.ScreenPointToRay(_inputReader.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _combinedLayerMask) == false)
        {
            return;
        }

        GameObject hitObject = hitInfo.collider.gameObject;

        if (hitObject.TryGetComponent(out Base clickedBase))
        {
            if (_selectedBase == clickedBase)
            {
                _selectedBase.ResetFlag();
                DeselectCurrent();
            }
            else
            {
                SelectBase(clickedBase);
            }
            return;
        }

        if (_selectedBase != null && IsGroundLayer(hitObject.layer))
        {
            _selectedBase.SetFlag(hitInfo.point);
            DeselectCurrent();
            return;
        }
    }

    private void SelectBase(Base newBase)
    {
        DeselectCurrent();
        _selectedBase = newBase;

        if (_selectedBase.TryGetComponent(out BaseSelection selection))
        {
            _currentSelection = selection;
            _currentSelection.Select();
        }
    }

    private void DeselectCurrent()
    {
        if (_currentSelection != null)
        {
            _currentSelection.Deselect();
            _currentSelection = null;
        }
        _selectedBase = null;
    }

    private bool IsGroundLayer(int layer)
    {
        return ((1 << layer) & _groundLayer) != 0;
    }
}