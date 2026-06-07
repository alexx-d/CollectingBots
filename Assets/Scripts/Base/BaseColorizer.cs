using UnityEngine;

public class BaseColorizer : MonoBehaviour
{
    private Renderer[] _renderers;

    public Color CurrentColor { get; private set; }

    public void GenerateAndApplyColor()
    {
        CurrentColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);

        _renderers = GetComponentsInChildren<Renderer>();

        foreach (var renderer in _renderers)
        {
            if (renderer != null)
            {
                renderer.material.color = CurrentColor;
            }
        }
    }

    public void ColorizeUnit(Unit unit)
    {
        if (unit != null && unit.TryGetComponent(out UnitColorizer unitColorizer))
        {
            unitColorizer.ApplyColor(CurrentColor);
        }
    }
}