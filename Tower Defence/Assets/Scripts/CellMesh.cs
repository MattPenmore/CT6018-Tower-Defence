using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CellMesh : MonoBehaviour
{

    Mesh cellMesh;
    List<Vector3> vertices;
    List<int> triangles;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = cellMesh = new Mesh();
        cellMesh.name = "CellMesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    public void Triangulate(GridCell[] cells)
    {
        //Clear old data then loop through all cells
        cellMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        cellMesh.vertices = vertices.ToArray();
        cellMesh.triangles = triangles.ToArray();
        cellMesh.RecalculateNormals();
    }

    void Triangulate ( GridCell cell)
    {
        //Create 6 triangles that make up a hexagon
        Vector3 center = cell.transform.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(center, center + GridMetrics.corners[i], center + GridMetrics.corners[i + 1]);
        }        
    }

    //Adds a triangle given three vertex positions.
    void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        //Add vertices in order
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);

        //Add indices of vertices to form a triangle
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
}
