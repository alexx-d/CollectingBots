using UnityEngine;

public class BaseSelection : MonoBehaviour
{
    [SerializeField] private GameObject _selectionMarker;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        _selectionMarker.SetActive(true);
    }

    public void Deselect()
    {
        _selectionMarker.SetActive(false);
    }
}