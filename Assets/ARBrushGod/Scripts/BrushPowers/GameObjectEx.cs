using UnityEngine;

//A class to draw circles
public static class GameObjectEx
{
    public static void DrawCircle(this GameObject container, float radius, float lineWidth, Color color)
    {
        var segments = 360;
        LineRenderer line = container.GetComponent<LineRenderer>();
        if (line == null)
        {
            Debug.Log("IM GOING IN");
            line = container.AddComponent<LineRenderer>();
            line.useWorldSpace = false;
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.positionCount = segments + 1;
            line.startColor = color;
            line.endColor = color;
            Material material = new Material(Shader.Find("Mobile/Particles/Additive"));
            line.material = material;
        }
        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0.01f, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }
}