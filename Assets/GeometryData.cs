using System.Collections.Generic;
using UnityEngine;

public class GeometryData
{
    public GeometryData()
    {
        Vertices = new List<Vector3>();
        Triangles = new List<int>();
        UV = new List<Vector2>();
    }

    public void Clear()
    {
        Vertices.Clear();
        Triangles.Clear();
        UV.Clear();
    }

    public List<Vector3> Vertices;
    public List<int> Triangles;
    public List<Vector2> UV;
}
