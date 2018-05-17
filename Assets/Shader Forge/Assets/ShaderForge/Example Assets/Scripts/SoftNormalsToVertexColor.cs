using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshFilter))]
public class SoftNormalsToVertexColor : MonoBehaviour
{
    public enum Method
    {
        Simple,
        AngularDeviation
    }

    public bool generateNow;
    public bool generateOnAwake = false;

    public Method method = Method.AngularDeviation;

    private void OnDrawGizmos()
    {
        if (generateNow)
        {
            generateNow = false;
            TryGenerate();
        }
    }

    private void Awake()
    {
        if (generateOnAwake)
            TryGenerate();
    }

    private void TryGenerate()
    {
        var mf = GetComponent<MeshFilter>();
        if (mf == null)
        {
            Debug.LogError("MeshFilter missing on the vertex color generator", gameObject);
            return;
        }

        if (mf.sharedMesh == null)
        {
            Debug.LogError("Assign a mesh to the MeshFilter before generating vertex colors", gameObject);
            return;
        }

        Generate(mf.sharedMesh);
        Debug.Log("Vertex colors generated", gameObject);
    }

    private void Generate(Mesh m)
    {
        var n = m.normals;
        var v = m.vertices;
        var colors = new Color[n.Length];
        var groups = new List<List<int>>();

        for (var i = 0; i < v.Length; i++)
        {
            // Group verts at the same location
            var added = false;
            foreach (var group in groups) // Add to exsisting group if possible
                if (v[group[0]] == v[i])
                {
                    group.Add(i);
                    added = true;
                    break;
                }

            if (!added)
            {
                // Create new group if necessary
                var newList = new List<int>();
                newList.Add(i);
                groups.Add(newList);
            }
        }

        foreach (var group in groups)
        {
            // Calculate soft normals
            var avgNrm = Vector3.zero;
            foreach (var i in group) // TODO: This can actually be improved. Averaging will not give the best outline.
                avgNrm += n[i];
            avgNrm.Normalize(); // Average normal done
            if (method == Method.AngularDeviation)
            {
                var avgDot = 0f; // Calculate deviation to alter length
                foreach (var i in group) avgDot += Vector3.Dot(n[i], avgNrm);
                avgDot /= group.Count;
                var angDeviation = Mathf.Acos(avgDot) * Mathf.Rad2Deg;
                var aAng = 180f - angDeviation - 90;
                var lMult = 0.5f / Mathf.Sin(aAng * Mathf
                                                 .Deg2Rad); // 0.5f looks better empirically, but mathematically it should be 1f
                avgNrm *= lMult;
            }

            foreach (var i in group) colors[i] = new Color(avgNrm.x, avgNrm.y, avgNrm.z); // Save normals as colors
        }

        m.colors = colors; // Assign as vertex colors

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        SceneView.RepaintAll();
#endif
    }
}