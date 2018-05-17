using System.Collections.Generic;
using UnityEngine;

public class GeometryData
{
    public List<Vector3> Normals;
    public List<int> Triangles;
    public List<Vector2> UV;

    public List<Vector3> Vertices;

    public GeometryData()
    {
        Vertices = new List<Vector3>();
        Normals = new List<Vector3>();
        Triangles = new List<int>();
        UV = new List<Vector2>();
    }

    public void Clear()
    {
        Vertices.Clear();
        Normals.Clear();
        Triangles.Clear();
        UV.Clear();
    }
}