using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RectMeshCutter : MonoBehaviour
{
    public void Slice(GameObject target, Vector2 lineStart, Vector2 lineEnd)
    {
        MeshFilter mf = target.GetComponent<MeshFilter>();
        if (mf == null) return;

        // 1. World -> Local Position
        Vector2 localA = target.transform.InverseTransformPoint(lineStart);
        Vector2 localB = target.transform.InverseTransformPoint(lineEnd);

        Vector3[] vertices = mf.sharedMesh.vertices;
        Vector2[] uvs = mf.sharedMesh.uv;

        // Quad (0, 1, 3, 2)
        int[] rectIndices = { 0, 1, 3, 2 };
        if (vertices.Length > 4)
        {
            rectIndices = Enumerable.Range(0, vertices.Length).ToArray();
        }

        List<VertexData> upperPoints = new List<VertexData>();
        List<VertexData> lowerPoints = new List<VertexData>();

        // 2. CUT / Sorting
        for (int i = 0; i < rectIndices.Length; i++)
        {
            int curr = rectIndices[i];
            int next = rectIndices[(i + 1) % rectIndices.Length];

            VertexData p1 = new VertexData(vertices[curr], uvs[curr]);
            VertexData p2 = new VertexData(vertices[next], uvs[next]);

            if (IsUpper(p1.pos, localA, localB)) upperPoints.Add(p1);
            else lowerPoints.Add(p1);

            if (GetIntersection(p1.pos, p2.pos, localA, localB, out Vector2 intersect, out float t))
            {
                VertexData intersectData = new VertexData(intersect, Vector2.Lerp(p1.uv, p2.uv, t));
                upperPoints.Add(intersectData);
                lowerPoints.Add(intersectData);
            }
        }

        // 3. Generate Pieces
        if (upperPoints.Count >= 3) CreateSlicedObject(target, upperPoints, "Upper_Piece");
        if (lowerPoints.Count >= 3) CreateSlicedObject(target, lowerPoints, "Lower_Piece");

        Destroy(target); // Destroy Original Object
    }

    void CreateSlicedObject(GameObject original, List<VertexData> points, string name)
    {
        var uniquePoints = points.GroupBy(p => p.pos).Select(g => g.First()).ToList();
        if (uniquePoints.Count < 3) return;

        Vector3 center = Vector3.zero;
        foreach (var point in uniquePoints) center += point.pos;
        center /= uniquePoints.Count;

        var sorted = uniquePoints.OrderBy(p => Mathf.Atan2(p.pos.y - center.y, p.pos.x - center.x)).ToList();

        GameObject piece = new GameObject(name);
        piece.transform.SetPositionAndRotation(original.transform.position, original.transform.rotation);
        piece.transform.localScale = original.transform.localScale;

        MeshFilter mf = piece.AddComponent<MeshFilter>();
        MeshRenderer mr = piece.AddComponent<MeshRenderer>();
        mr.sharedMaterial = original.GetComponent<MeshRenderer>().sharedMaterial;

        Mesh mesh = new Mesh(); // 
        mesh.vertices = sorted.Select(p => p.pos).ToArray();
        mesh.uv = sorted.Select(p => p.uv).ToArray();

        // Triangle Indices
        int[] triangles = new int[(sorted.Count - 2) * 3];
        for (int i = 0; i < sorted.Count - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mf.mesh = mesh;

        // # Collider Set
        var col = piece.AddComponent<PolygonCollider2D>();
        col.points = sorted.Select(p => (Vector2)p.pos).ToArray();

        var rb = piece.AddComponent<Rigidbody2D>();
        // Push To Cut Direction
        rb.AddForce((center - Vector3.zero).normalized * 2f, ForceMode2D.Impulse);
    }

    private bool IsUpper(Vector2 point, Vector2 a, Vector2 b) => (b.x - a.x) * (point.y - a.y) - (b.y - a.y) * (point.x - a.x) > 0;

    private bool GetIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection, out float t)
    {
        intersection = Vector2.zero; t = 0;
        float d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);
        if (Mathf.Abs(d) < 0.0001f) return false;
        t = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
        float v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;
        if (t >= 0 && t <= 1 && v >= 0 && v <= 1)
        {
            intersection = p1 + t * (p2 - p1);
            return true;
        }
        return false;
    }

    private struct VertexData
    {
        public Vector3 pos;
        public Vector2 uv;
        public VertexData(Vector3 p, Vector2 u) { pos = p; uv = u; }
    }
}