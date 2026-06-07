using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProceduralLineRing : MonoBehaviour
{
    [SerializeField] private int _segments = 64;
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _lineWidth = 0.1f;
    [SerializeField] private Color _color = Color.green;

    void Awake()
    {
        LineRenderer line = GetComponent<LineRenderer>();

        line.useWorldSpace = false;
        line.startWidth = _lineWidth;
        line.endWidth = _lineWidth;
        line.positionCount = _segments + 1;
        line.loop = true;

        line.material = new Material(Shader.Find("Sprites/Default"));

        line.startColor = _color;
        line.endColor = _color;

        float deltaTheta = (2f * Mathf.PI) / _segments;
        float theta = 0f;

        for (int i = 0; i < _segments + 1; i++)
        {
            float x = _radius * Mathf.Cos(theta);
            float z = _radius * Mathf.Sin(theta);

            line.SetPosition(i, new Vector3(x, 0, z));

            theta += deltaTheta;
        }
    }
}