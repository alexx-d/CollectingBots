using TMPro;
using UnityEngine;

public class BaseResourceUI : MonoBehaviour
{
    [SerializeField] private BaseStorage _storage;
    [SerializeField] private TextMeshProUGUI _counterText;

    private void OnEnable()
    {
        _storage.ResourceCountChanged += UpdateText;

        UpdateText();
    }

    private void OnDisable()
    {
        _storage.ResourceCountChanged -= UpdateText;
    }

    private void UpdateText()
    {
        _counterText.text = _storage.ResourceCount.ToString();
    }
}