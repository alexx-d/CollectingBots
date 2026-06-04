using TMPro;
using UnityEngine;

public class BaseResourceUI : MonoBehaviour
{
    [SerializeField] private BaseStorage _storage;
    [SerializeField] private TextMeshProUGUI _counterText;

    private void OnEnable()
    {
        _storage.OnResourceCountChanged += UpdateText;

        UpdateText(_storage.ResourceCount);
    }

    private void OnDisable()
    {
        _storage.OnResourceCountChanged -= UpdateText;
    }

    private void UpdateText(int currentResources)
    {
        _counterText.text = currentResources.ToString();
    }
}