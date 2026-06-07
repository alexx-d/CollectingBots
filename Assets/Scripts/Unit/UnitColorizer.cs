using UnityEngine;

public class UnitColorizer : MonoBehaviour
{
    private Renderer[] _renderers;

    public void ApplyColor(Color color)
    {
        _renderers = GetComponentsInChildren<Renderer>();

        foreach (var renderer in _renderers)
        {
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }
}